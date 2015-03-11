using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class FysiikkaPeli2 : PhysicsGame
{
    Image pelaajanKuva = LoadImage("Freddy");
    Image taustanKuva = LoadImage("taustankuva");
    Image olionKuva = LoadImage("guard");
    Image reunaKuva = LoadImage("reuna");
    Image kameranKuva = LoadImage("kamera1");
    Image kameranKuva2 = LoadImage("kameratoinen1");
    private Image[] kameranValo = LoadImages("kamera1", "kamera2");
    private Image[] kameranValo2 = LoadImages("kameratoinen1", "kameratoinen2");
    PlatformCharacter freddy;
    PhysicsObject guard;
    PhysicsObject kamera;
    PhysicsObject kamera2;
    Timer aikaLaskuri;
    Label aikaNaytto;

    public override void Begin()
    {
        LuoKentta();
        LuoAikaLaskuri();
        
        LuoNappaimet();

        

        AddCollisionHandler<PlatformCharacter, PhysicsObject>(freddy, guard, PelaajatTormaavat);
        Gravity = new Vector(0.0, -800.0);
        Camera.Follow(freddy);
        Camera.Zoom(2.0);
        
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    
    void LuoKentta()
    {
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta");
        
        ruudut.SetTileMethod(Color.FromHexCode("56FF49"), LuoPelaaja);
        ruudut.SetTileMethod(Color.Black, LuoReunat);
        ruudut.SetTileMethod(Color.Red, LuoOlio);
        ruudut.SetTileMethod(Color.FromHexCode("0505FF"), LuoKamerat);
        ruudut.SetTileMethod(Color.FromHexCode("FF6A00"), LuoKamerat2);
        ruudut.SetTileMethod(Color.FromHexCode("808080"), LuoTausta);
        ruudut.Optimize(Color.FromHexCode("808080"));
        ruudut.Execute(20, 20);
        Camera.Zoom(0.55);

    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        freddy = new PlatformCharacter(40, 30);
        freddy.Shape = Shape.Rectangle;
        freddy.Image = pelaajanKuva;
        freddy.Position = paikka;
        freddy.Tag = "freddy";
        freddy.CollisionIgnoreGroup = 1;
        freddy.CanRotate = false;
        freddy.Restitution = 0.0;
        freddy.IgnoresPhysicsLogics = true;
        Add(freddy);

    }

    void LuoReunat(Vector paikka,double leveys, double korkeus)
    {
        PhysicsObject reunat = PhysicsObject.CreateStaticObject(leveys, korkeus);
        reunat.Image = reunaKuva;
        reunat.Position = paikka;
        Add(reunat);

        
    }

    void LuoTausta(Vector paikka,double leveys, double korkeus)
    {
        PhysicsObject tausta = PhysicsObject.CreateStaticObject(leveys, korkeus);
        tausta.Image = taustanKuva;
        tausta.Position = paikka;
        tausta.CollisionIgnoreGroup = 1;
        Add(tausta);

    }


    void LuoOlio(Vector paikka,double leveys, double korkeus)
    {
        guard = new PhysicsObject(80, 60);
        guard.Shape = Shape.Rectangle;
        guard.Image = olionKuva;
        guard.Position = paikka;
        guard.Tag = "guard";
        Add(guard);

    }

    void LuoAikaLaskuri()
    {
        aikaLaskuri = new Timer();
        aikaLaskuri.Start();

        aikaNaytto = new Label();
        aikaNaytto.TextColor = Color.White;
        aikaNaytto.DecimalPlaces = 1;
        aikaNaytto.BindTo(aikaLaskuri.SecondCounter);
        Add(aikaNaytto);
    }

    void LuoNappaimet()
    {
        Keyboard.Listen(Key.Left, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(-240, 0));
        Keyboard.Listen(Key.Right, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(240, 0));
        Keyboard.Listen(Key.Up, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(0, 240));
        Keyboard.Listen(Key.Down, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(0, -240));
        Keyboard.Listen(Key.Up, ButtonState.Released,
        LiikutaPelaajaa, null, new Vector(0, 0));
        Keyboard.Listen(Key.Down, ButtonState.Released,
        LiikutaPelaajaa, null, new Vector(0, 0));
        Keyboard.Listen(Key.Left, ButtonState.Released,
        LiikutaPelaajaa, null, new Vector(0, 0));
        Keyboard.Listen(Key.Right, ButtonState.Released,
        LiikutaPelaajaa, null, new Vector(0, 0));


    }

    void LiikutaPelaajaa(Vector vektori)
    {
        freddy.Velocity = vektori;
        

    }

    void PelaajatTormaavat(PlatformCharacter tormaaja, PhysicsObject kohde)
    {
        aikaLaskuri.Stop();
        aikaNaytto.Stop();
    }

    void LuoKamerat(Vector paikka, double leveys, double korkeus)
    {
        kamera = new PhysicsObject(60, 60);
        kamera.Image = kameranKuva;
        kamera.CollisionIgnoreGroup = 1;
        kamera.Position = paikka;
        kamera.IgnoresPhysicsLogics = true;
        kamera.Animation = new Animation(kameranValo);
        kamera.Animation.Start();
        kamera.Animation.FPS = 1;
        Add(kamera);

    }

    void LuoKamerat2(Vector paikka, double leveys, double korkeus)
    {
        kamera2 = new PhysicsObject(60, 60);
        kamera2.Image = kameranKuva2;
        kamera2.CollisionIgnoreGroup = 1;
        kamera2.Position = paikka;
        kamera2.IgnoresPhysicsLogics = true;
        kamera2.Animation = new Animation(kameranValo2);
        kamera2.Animation.Start();
        kamera2.Animation.FPS = 1;
        Add(kamera2);
    }
}


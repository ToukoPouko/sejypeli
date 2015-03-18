using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class FysiikkaPeli2 : PhysicsGame
{
    EasyHighScore topLista = new EasyHighScore();

    Image pelaajanKuva = LoadImage("Freddy");
    Image taustanKuva = LoadImage("taustankuva");
    Image olionKuva = LoadImage("guard");
    Image reunaKuva = LoadImage("reuna");
    Image kameranKuva = LoadImage("kamera1");
    Image kameranKuva2 = LoadImage("kameratoinen1");
    Image kameranKuva3 = LoadImage("kamerakolmas1");
    Image kameranKuva4 = LoadImage("kameraneljäs1");
    Image valonKuva1 = LoadImage("valo1kuva1");
    Image valonKuva2 = LoadImage("valo2kuva1");
    Image valonKuva3 = LoadImage("valo3kuva1");
    Image valonKuva4 = LoadImage("valo4kuva1");
    private Image[] kameranValo = LoadImages("kamera1", "kamera2");
    private Image[] kameranValo2 = LoadImages("kameratoinen1", "kameratoinen2");
    private Image[] kameranValo3 = LoadImages("kamerakolmas1", "kamerakolmas2");
    private Image[] kameranValo4 = LoadImages("kameraneljäs1", "kameraneljäs2");
    PlatformCharacter freddy;
    PhysicsObject guard;
    PhysicsObject kamera;
    PhysicsObject kamera2;
    PhysicsObject kamera3;
    PhysicsObject kamera4;
    PhysicsObject valo1;
    PhysicsObject valo2;
    PhysicsObject valo3;
    PhysicsObject valo4;
    Timer aikaLaskuri;
    Label aikaNaytto;

    public override void Begin()
    {
        LuoKentta();
        LuoAikaLaskuri();
        
        LuoNappaimet();


        AddCollisionHandler<PlatformCharacter, PhysicsObject>(freddy, "valo", ValoTormaava);
        AddCollisionHandler<PlatformCharacter, PhysicsObject>(freddy, guard, PelaajatTormaavat);
        Gravity = new Vector(0.0, -800.0);
        Camera.Follow(freddy);
        Camera.Zoom(2.0);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    public void AloitaPeli(Window sender)
    {
    }

    void PelaajaVoitti()
    {
      
        topLista.EnterAndShow(aikaLaskuri.CurrentTime);
        topLista.HighScoreWindow.Closed += AloitaPeli;
    }

    void LuoKentta()
    {
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta");

        ruudut.SetTileMethod(Color.FromHexCode("56FF49"), LuoPelaaja);
        ruudut.SetTileMethod(Color.Black, LuoReunat);
        ruudut.SetTileMethod(Color.Red, LuoOlio);
        ruudut.SetTileMethod(Color.FromHexCode("0505FF"), LuoKamerat);
        ruudut.SetTileMethod(Color.FromHexCode("FF6A00"), LuoKamerat2);
        ruudut.SetTileMethod(Color.FromHexCode("5E0089"), LuoKamerat3);
        ruudut.SetTileMethod(Color.FromHexCode("B200FF"), LuoKamerat4);
        ruudut.SetTileMethod(Color.FromHexCode("808080"), LuoTausta);
        ruudut.SetTileMethod(Color.FromHexCode("FFD800"), LuoValot);
        ruudut.SetTileMethod(Color.FromHexCode("FFB642"), LuoValot2);
        ruudut.SetTileMethod(Color.FromHexCode("AD6500"), LuoValot3);
        ruudut.SetTileMethod(Color.FromHexCode("C18632"), LuoValot4);
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

    void LuoReunat(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject reunat = PhysicsObject.CreateStaticObject(leveys, korkeus);
        reunat.Image = reunaKuva;
        reunat.Position = paikka;
        Add(reunat);


    }

    void LuoTausta(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject tausta = PhysicsObject.CreateStaticObject(leveys, korkeus);
        tausta.Image = taustanKuva;
        tausta.Position = paikka;
        tausta.CollisionIgnoreGroup = 1;

        Add(tausta);

    }


    void LuoOlio(Vector paikka, double leveys, double korkeus)
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
        LiikutaPelaajaa, null, new Vector(-300, 0));
        Keyboard.Listen(Key.Right, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(300, 0));
        Keyboard.Listen(Key.Up, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(0, 300));
        Keyboard.Listen(Key.Down, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(0, -300));
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
        PelaajaVoitti();
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

    void LuoKamerat3(Vector paikka, double leveys, double korkeus)
    {
        kamera3 = new PhysicsObject(60, 60);
        kamera3.Image = kameranKuva3;
        kamera3.CollisionIgnoreGroup = 1;
        kamera3.Position = paikka;
        kamera3.IgnoresPhysicsLogics = true;
        kamera3.Animation = new Animation(kameranValo3);
        kamera3.Animation.Start();
        kamera3.Animation.FPS = 1;
        Add(kamera3);
    }

    void LuoKamerat4(Vector paikka, double leveys, double korkeus)
    {
        kamera4 = new PhysicsObject(60, 60);
        kamera4.Image = kameranKuva4;
        kamera4.CollisionIgnoreGroup = 1;
        kamera4.Position = paikka;
        kamera4.IgnoresPhysicsLogics = true;
        kamera4.Animation = new Animation(kameranValo4);
        kamera4.Animation.Start();
        kamera4.Animation.FPS = 1;
        Add(kamera4);
    }

    void LuoValot(Vector paikka, double leveys, double korkeus)
    {
        valo1 = new PhysicsObject(120, 120);
        valo1.Image = valonKuva1;
        valo1.Tag = "valo";
        valo1.Position = paikka;
        valo1.IgnoresPhysicsLogics = true;
        Add(valo1);

        

    }

    void LuoValot2(Vector paikka, double leveys, double korkeus)
    {
        valo2 = new PhysicsObject(120, 120);
        valo2.Image = valonKuva2;
        valo2.Tag = "valo";
        valo2.Position = paikka;
        valo2.IgnoresPhysicsLogics = true;
        Add(valo2);
    }

    void LuoValot3(Vector paikka, double leveys, double korkeus)
    {
        valo3 = new PhysicsObject(120, 120);
        valo3.Image = valonKuva3;
        valo3.Tag = "valo";
        valo3.Position = paikka;
        valo3.IgnoresPhysicsLogics = true;
        Add(valo3);
    }

    void LuoValot4(Vector paikka, double leveys, double korkeus)
    {
        valo4 = new PhysicsObject(120, 120);
        valo4.Image = valonKuva4;
        valo4.Tag = "valo";
        valo4.Position = paikka;
        valo4.IgnoresPhysicsLogics = true;
        Add(valo4);
    }



    void ValoTormaava(PlatformCharacter tormaaja, PhysicsObject kohde)
    {
        ClearAll();
        Begin();
    }
}


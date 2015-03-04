using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class FysiikkaPeli2 : TopDownPhysicsGame
{
    Image pelaajanKuva = LoadImage("Freddy");
    Image taustanKuva = LoadImage("taustankuva");
    Image olionKuva = LoadImage("guard");
    Image reunaKuva = LoadImage("reuna");
    PhysicsObject freddy;
    PhysicsObject guard;
    Timer aikaLaskuri;
    Label aikaNaytto;

    public override void Begin()
    {
        LuoKentta();
        LuoAikaLaskuri();
        
        LuoNappaimet();

        

        AddCollisionHandler(freddy, guard, PelaajatTormaavat);
        Gravity = 0.0;
        
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    
    void LuoKentta()
    {
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta");
        
        ruudut.SetTileMethod(Color.FromHexCode("56FF49"), LuoPelaaja);
        ruudut.SetTileMethod(Color.Black, LuoReunat);
        ruudut.SetTileMethod(Color.Red, LuoOlio);
        ruudut.SetTileMethod(Color.FromHexCode("808080"), LuoTausta);
        ruudut.Optimize(Color.FromHexCode("808080"));
        ruudut.Execute(20, 20);
        Camera.Zoom(0.55);

    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        freddy = new PhysicsObject(40, 30);
        freddy.Shape = Shape.Rectangle;
        freddy.Image = pelaajanKuva;
        freddy.Position = paikka;
        freddy.Tag = "freddy";
        freddy.CollisionIgnoreGroup = 1;
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
        LiikutaPelaajaa, null, new Vector(-200, 0));
        Keyboard.Listen(Key.Right, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(200, 0));
        Keyboard.Listen(Key.Up, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(0, 200));
        Keyboard.Listen(Key.Down, ButtonState.Down,
        LiikutaPelaajaa, null, new Vector(0, -200));
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

    void PelaajatTormaavat(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        aikaLaskuri.Stop();
        aikaNaytto.Stop();
    }
}


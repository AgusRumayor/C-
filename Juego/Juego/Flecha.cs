using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Juego
{
    class Flecha
    {
        public Vector3 posicion,viento;
        float v0,vX,vY,angulo,anguloR,tiempo,angulo1,anguloR1,iniY;
        float g = 9.81f;
        TimeSpan t0, tActual;
        public string s;
        public bool lanzamiento;
        KeyboardState tecladoPasado = new KeyboardState();
        KeyboardState tecladoActual = new KeyboardState();
        public BoundingBox flechaBox;
        public bool blanco = false;
        public bool term = false;
        
        public Flecha()
        {
            iniY = 10;
            posicion = new Vector3(0f, iniY, 0f);
            viento = new Vector3(0f, 0f, 0f);
            //posicion = new Vector3(0f, 0f, 0f);
            v0 = 100;
            angulo= 0;
            anguloR= angulo*(float)Math.PI/180;
            vX = v0 * (float)(Math.Cos(anguloR));
            vY = v0 * (float)(Math.Sin(anguloR));
            t0 = new TimeSpan();
            lanzamiento = false;
            Vector3 pmax= posicion;
            //pmax.X -= 15;
            pmax.Y += 10;
            pmax.Z += 10;
            Vector3 pmin = posicion;
            pmin.X -= 10;
            pmin.Y += 10;
            pmin.Z += 10;
            flechaBox = new BoundingBox(pmin, pmax);
        }
        public Flecha(float vel0,float ang){

        }
        public Vector3 getPosicion()
        {
            return posicion;
        }
        public void updateValores(float incZ, float incY)
        {
            angulo = incZ;
            angulo1 = incY;

            //v0 = 100;
            anguloR = angulo * (float)Math.PI / 180;
            anguloR1 = angulo1 * (float)Math.PI / 180;
            vX = v0 * (float)(Math.Cos(anguloR));
            vY = v0 * (float)(Math.Sin(anguloR));
            
            s = "Angulo Horizontal: " + angulo1 + "  Angulo Vertical: " + angulo;

        }
        public void Update(GameTime gametime)
        {
            tecladoPasado = tecladoActual;
            tecladoActual = Keyboard.GetState();
            if (lanzamiento)
            {
                tiempo= ((tActual.Seconds+(tActual.Milliseconds/1000f))-(t0.Seconds+(t0.Milliseconds/1000f)));
                posicion.X = (vX+viento.X) * tiempo;
                //posicion.X = (X * tiempo;
                posicion.Y = (float)((vY * tiempo) - (4.905f * (Math.Pow(tiempo, 2))))+iniY;
                //posicion.Y = (float)(((vY+viento.Y) * tiempo) - (4.905f * (Math.Pow(tiempo, 2)))) + iniY;
                //posicion.Z = (posicion.X * (float)Math.Tan(anguloR1));
                posicion.Z = (posicion.X * (float)Math.Tan(anguloR1)) + viento.Z;
                s = "X: " + posicion.X + " Y: " + posicion.Y;
            }
            //if (tiempo > (2*vY/g)||posicion.Y<0)
            if (posicion.Y < 0 || blanco)
            {
                lanzamiento = false;
                term = true;
            }
            MouseState raton = Mouse.GetState();
            //if (ButtonState.Pressed == raton.LeftButton)
            if (tecladoPasado.IsKeyUp(Keys.Space) && (tecladoActual.IsKeyDown(Keys.Space)))
            {
                t0 = gametime.TotalGameTime;
                lanzamiento = true;
            }
            if (tecladoPasado.IsKeyUp(Keys.Q) && (tecladoActual.IsKeyDown(Keys.Q)))
            {
                posicion.X += 1;
            }
            if (tecladoPasado.IsKeyUp(Keys.E) && (tecladoActual.IsKeyDown(Keys.E)))
            {
                posicion.X -= 1;
            }
            if (tecladoPasado.IsKeyUp(Keys.Z) && (tecladoActual.IsKeyDown(Keys.Z)))
            {
                posicion.Y += 2;
            }
            if (tecladoPasado.IsKeyUp(Keys.C) && (tecladoActual.IsKeyDown(Keys.C)))
            {
                posicion.Y -= 2;
            }
            if (tecladoPasado.IsKeyUp(Keys.F) && (tecladoActual.IsKeyDown(Keys.F)))
            {
                 v0+= 2;
                 updateValores(angulo, angulo1);
            }
            tActual = gametime.TotalGameTime;
            Vector3 pmin = posicion;
            pmin.X -= 10;
            pmin.Y += 10;
            pmin.Z += 10;
            flechaBox.Min = pmin;
            Vector3 pmax= posicion;
            //pmax.X -= 15;
            pmax.Y += 10;
            pmax.Z += 10;
            flechaBox.Max = pmax;
            //s = "X: " + posicion.X + " Y: " + posicion.Y + " "; ;
        }
        public void Reset()
        {
            iniY = 10;
            posicion = new Vector3(0f, iniY, 0f);
            //posicion = new Vector3(0f, 0f, 0f);
            v0 = 100;
            angulo = 0;
            anguloR = angulo * (float)Math.PI / 180;
            vX = v0 * (float)(Math.Cos(anguloR));
            vY = v0 * (float)(Math.Sin(anguloR));
            t0 = new TimeSpan();
            lanzamiento = false;
            Vector3 pmax = posicion;
            //pmax.X -= 15;
            pmax.Y += 10;
            pmax.Z += 10;
            Vector3 pmin = posicion;
            pmin.X -= 10;
            pmin.Y += 10;
            pmin.Z += 10;
            flechaBox = new BoundingBox(pmin, pmax);
        }
    }
}

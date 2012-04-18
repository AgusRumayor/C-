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
    class Blanco
    {
        public Vector3 posicion;
        public BoundingBox blancoBox;
        public Blanco()
        {
            posicion = new Vector3();
            Vector3 pmax = posicion;
            //pmax.X += 10;
            pmax.Y += 10;
            pmax.Z += 20;
            blancoBox = new BoundingBox(posicion,pmax);
        }
    }
}

using System;
using System.Collections.Generic;
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
    class SkySphere
    {
        private Model _skySphere;

        public void Load(Game game)
        {
            // load the .fbx file exported from Blender (www.blender.org)
            // I've included the .blend file in the Content\Models folder as
            // well if you want to look at what I've done. It's a basic UV Sphere
            // with it's normals flipped (so we see the inside of the sphere), and
            // with it's UV Texture coordinates set.

            // If you click on the little 'present' looking icon in Blender next to
            // the texture filename (in the texture panel), Blender will make 
            // reference to the texture, but for some reason puts it in an fbx folder
            // - so we have to do that in the project so the textures load successfully.
            _skySphere = game.Content.Load<Model>("Models\\skysphere");
        }

        public void Draw(Vector3 cameraPosition, Matrix cameraProjectionMatrix, Matrix cameraViewMatrix, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.RenderState.DepthBufferWriteEnable = false;

            // the sky sphere is drawn slightly above the camera's position

            // if changing this, please make sure you apply the scale 
            // transformation first - otherwise you will get strange results...

            Matrix worldMatrix =
                Matrix.CreateScale(10000.0f) *
                Matrix.CreateTranslation(cameraPosition) *
                Matrix.CreateTranslation(0.0f, -40.03f, 0.0f);

            foreach (ModelMesh mesh in _skySphere.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = cameraViewMatrix;
                    effect.Projection = cameraProjectionMatrix;
                }

                mesh.Draw();
            }

            graphicsDevice.RenderState.DepthBufferWriteEnable = true;
        }

    }
}

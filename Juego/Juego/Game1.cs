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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        KeyboardState tecladoPasado = new KeyboardState();
        KeyboardState tecladoActual = new KeyboardState();
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        Model modeloFlecha;
        Model modeloCampo;
        Model modeloBlanco;
        Model modeloPasto;
        Model modeloArco;
        Matrix worldFlecha;
        Matrix worldBlanco;
        Matrix worldArco;
        Vector3 cameraPosition = new Vector3(-1050.0f, 700.0f, 350.0f);
        //Vector3 cameraPosition = new Vector3(0f, 0f, 0f);
        Vector3 lookAt = new Vector3(1220.0f, 135.0f, 210.0f);
        //Vector3 lookAt = Vector3.Forward;
        Vector3 posicionFlecha = new Vector3(3f, 55f, 10f);
        Random aleatorio = new Random();
        //Vector3 posicionBlanco = new Vector3(11133f, 455f, (float)(aleatorio.NextDouble()*1000));
        Vector3 posicionBlanco = new Vector3(700f, 455f, 0f);
        SpriteFont spriteFont;
        Flecha flecha;
        Blanco blanco;
        BoundingSphere esferaf, esferab;
        float inclinacionY, inclinacionZ;
        public string text;
        //SkySphere skySphere = new SkySphere();
        public int puntos;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            device= graphics.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);
            //worldFlecha = Matrix.Identity;
            worldFlecha = Matrix.CreateRotationY((float)(Math.PI / 2));
            worldBlanco = Matrix.CreateRotationZ((float)(Math.PI / 2));
            worldArco = worldFlecha;
            inclinacionY = 0;
            inclinacionZ = 0;
            flecha = new Flecha();
            blanco = new Blanco();
            puntos = 0;
            int signo = 1;
            if (aleatorio.NextDouble() > .5)
                signo = -1;
            posicionBlanco = new Vector3(11133f, 455f, (float)(aleatorio.NextDouble() * 1000)*signo);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            modeloFlecha = Content.Load<Model>("arrow");
            modeloCampo = Content.Load<Model>("edificio");
            modeloBlanco = Content.Load<Model>("Blanco");
            modeloPasto = Content.Load<Model>("Ground");
            modeloArco = Content.Load<Model>("bow");
            spriteFont = Content.Load<SpriteFont>("Arial");
            //skySphere.Load(this);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            tecladoPasado = tecladoActual;
            tecladoActual = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState raton = Mouse.GetState();

            if(tecladoActual.IsKeyDown(Keys.W))
                cameraPosition.Y+=5;
            if (tecladoActual.IsKeyDown(Keys.S))
                cameraPosition.Y-=5;
            if (tecladoActual.IsKeyDown(Keys.A))
                cameraPosition.X-=5;
            if (tecladoActual.IsKeyDown(Keys.D))
                cameraPosition.X+=5;
            cameraPosition.Z = raton.ScrollWheelValue / 4;

            
            if (tecladoPasado.IsKeyUp(Keys.Up) && (tecladoActual.IsKeyDown(Keys.Up)))
            {
                inclinacionZ++;
                flecha.updateValores(inclinacionZ,-inclinacionY);
            }
            if (tecladoPasado.IsKeyUp(Keys.Down) && (tecladoActual.IsKeyDown(Keys.Down)))
            {
                inclinacionZ--;
                flecha.updateValores(inclinacionZ,-inclinacionY);
            }
            if (tecladoPasado.IsKeyUp(Keys.Right) && (tecladoActual.IsKeyDown(Keys.Right)))
            {
                inclinacionY--;
                flecha.updateValores(inclinacionZ,-inclinacionY);
            }
            if (tecladoPasado.IsKeyUp(Keys.Left) && (tecladoActual.IsKeyDown(Keys.Left)))
            {
                inclinacionY++;
                flecha.updateValores(inclinacionZ,-inclinacionY);
            }
            if (tecladoPasado.IsKeyUp(Keys.Enter) && (tecladoActual.IsKeyDown(Keys.Enter)))
            {
                flecha= new Flecha();
                int signo = 1;
                if (aleatorio.NextDouble() > .5)
                    signo = -1;
                posicionBlanco = new Vector3((float)(aleatorio.NextDouble() * 15000)+200, 455f, (float)(aleatorio.NextDouble() * 1000) * signo);
            }
            
            worldFlecha = Matrix.CreateRotationY((float)((inclinacionY) * Math.PI / 180)) * Matrix.CreateRotationZ((float)((-inclinacionZ) * Math.PI / 180)) * Matrix.CreateRotationY((float)Math.PI);
                //worldFlecha = Matrix.CreateFromAxisAngle(new Vector3(0f, 1f, 1f),(float) (inclinacionY*inclinacionZ / Math.PI));   
                posicionFlecha=flecha.getPosicion()*50;
                worldFlecha.Translation=posicionFlecha;
                worldBlanco.Translation = posicionBlanco;
                flecha.Update(gameTime);
                if (flecha.lanzamiento)
                {
                    //cameraPosition.X = cameraPosition.X + (posicionFlecha.X / 100);
                    cameraPosition.X = posicionFlecha.X - 1250;
                    //lookAt = posicionFlecha;
                    //worldArco = Matrix.Identity;
                }
                else
                {
                    worldArco = worldFlecha;
                }
                lookAt = posicionFlecha;
                cameraPosition.X = posicionFlecha.X - 1250;
                if (flecha.term)
                    worldArco = Matrix.Identity;
                //cameraPosition.Y = cameraPosition.Y + posicionFlecha.Y;
                //cameraPosition.Z = cameraPosition.Z + posicionFlecha.Z;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DibujaModelo(modeloCampo, Matrix.CreateScale(1000f)*Matrix.CreateTranslation(0f,-425f,0f));
            DibujaModelo(modeloPasto, Matrix.Identity);
            //DibujaModelo(modeloArco, Matrix.CreateTranslation(-200f,305f,0f)*Matrix.CreateRotationY((float)Math.PI));
            worldArco = worldArco * Matrix.CreateTranslation(0f, (float)inclinacionZ * 2.5f, -(float)inclinacionY * 2.95f);
            DibujaModelo(modeloArco, worldArco * Matrix.CreateTranslation(150f, 0f, 0f));
            //DibujaModelo(modeloArco, worldFlecha);
            DibujaModelo(modeloFlecha, worldFlecha);
            DibujaModelo(modeloBlanco, worldBlanco);
            //cambiar proyeccion
            //skySphere.Draw(cameraPosition, Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), graphics.GraphicsDevice.Viewport.AspectRatio,1.0f,1000.0f), Matrix.CreateLookAt(cameraPosition, lookAt, Vector3.Up), device);
            if (CollidesWith(modeloBlanco, worldBlanco, modeloFlecha, worldFlecha))
            {
                if(!flecha.blanco)
                puntos++;
                flecha.blanco = true;
                flecha.term = true;
                
            }
            DibujaTexto();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
        private void DibujaModelo(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            //float aspectRatio = 1f;
            foreach (ModelMesh mesh in model.Meshes)
            {
                
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    //effect.World = mesh.ParentBone.Transform * world;
                    effect.View = Matrix.CreateLookAt(cameraPosition, lookAt, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f);
                    //effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10f);
                    
                    //effect.View = camera.View;
                    //effect.Projection = camera.Projection;
                }
                mesh.Draw();
                if (model.Equals(modeloFlecha))
                    esferaf = mesh.BoundingSphere;
                if (model.Equals(modeloBlanco))
                    esferab = mesh.BoundingSphere;
            }
            
        }
        public bool CollidesWith(Model modelo1, Matrix mundo1, Model modelo2, Matrix mundo2)
        {
            // Loop through each ModelMesh in both objects and compare
            // all bounding spheres for collisions
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            foreach (ModelMesh myModelMeshes in modelo1.Meshes)
            {
                foreach (ModelMesh hisModelMeshes in modelo2.Meshes)
                {
                    BoundingSphere esfera1, esfera2;
                    //radio*155
                    Vector3 centro2 = hisModelMeshes.BoundingSphere.Center;
                    centro2.X -= 360;
                    esfera1= new BoundingSphere(myModelMeshes.BoundingSphere.Center,(myModelMeshes.BoundingSphere.Radius*300));
                    esfera2= new BoundingSphere(centro2,(hisModelMeshes.BoundingSphere.Radius*25));

                    BoundingSphereRenderer.Render(esfera1.Transform(mundo1), device, Matrix.CreateLookAt(cameraPosition, lookAt, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f), Color.Blue);
                    BoundingSphereRenderer.Render(esfera2.Transform(mundo2), device, Matrix.CreateLookAt(cameraPosition, lookAt, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f), Color.Red);


                    //BoundingBox caja =new BoundingBox(new Vector3(30f,99f,110f),posicionBlanco);

                    //BoundingBoxRenderer.Render(caja, device, Matrix.CreateLookAt(cameraPosition, lookAt, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f), Color.Blue);
                    
                    
                    if (esfera1.Transform(
                        mundo1).Intersects(
                        esfera2.Transform(mundo2)))
                        return true;
                    /*if (myModelMeshes.BoundingSphere.Transform(
                        mundo1).Intersects(
                        hisModelMeshes.BoundingSphere.Transform(mundo2)))
                        return true;*/
                }
            }
            return false;
        }
        private void DibujaTexto()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.SaveState);
                text = ""+flecha.s;
            spriteBatch.DrawString(spriteFont, text, new Vector2(65, 65), Color.Black);
            spriteBatch.DrawString(spriteFont, text, new Vector2(64, 64), Color.White);
            string sp = "Puntos: " + puntos;
            spriteBatch.DrawString(spriteFont, sp, new Vector2(665, 65), Color.Black);
            spriteBatch.DrawString(spriteFont, sp, new Vector2(664, 64), Color.White);
            


            spriteBatch.End();
        }


    }
}

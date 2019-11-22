using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame;
using Tempora.Engine;

namespace TestGame
{
    public class TestGame : GameState
    {
        //The currently loaded map
        public static Map TestMap;
        public static World TestWorld;

        //Render state
        private RasterizerState rs = new RasterizerState { MultiSampleAntiAlias = true };

        public override void Initialize()
        {
           
        }

        public override void Load()
        {
            //Load all entity data
            EntityManager.LoadEntities();

            //Load the map
            TestMap = MapManager.LoadMap("test", 3);

            //Create the world
            TestWorld = new World("Test World");

             for (int i = 0; i < 30; i++)
             {
                for (int x = 0; x < 8; x++)
                {
                    //Create an entity
                    ETestEntity e = EntityManager.CreateEntity<ETestEntity>(TestWorld);
                    e.transform.Scale = Vector2.One * 43;
                    e.transform.Position = new Vector2(-200 + (40 * i), -40 * x);
                }
             }

            //Create a player
            EPlayer ply = EntityManager.CreateEntity<EPlayer>(TestWorld);

            //Make the player twice as large
            ply.transform.Scale = new Vector2(3,3);
            ply.transform.Position = new Vector2(-200, 300);

            //Start simulating the world
            TestWorld.Start();

            //Create a physics body for the ground
            ESolidBlock groundOne = EntityManager.CreateEntity<ESolidBlock>(TestWorld);
            groundOne.transform.Position = new Vector2(-800, 300);
            groundOne.transform.Rotation = 0.8f;
            groundOne.transform.Scale = new Vector2(10000, 10);
            groundOne.InitializePhysics();

            ESolidBlock groundTwo = EntityManager.CreateEntity<ESolidBlock>(TestWorld);
            groundTwo.transform.Position = new Vector2(800, 300);
            groundTwo.transform.Rotation = -0.8f;
            groundTwo.transform.Scale = new Vector2(10000, 10);
            groundTwo.InitializePhysics();

            ERotatingSolidBlock spinner = EntityManager.CreateEntity<ERotatingSolidBlock>(TestWorld);
            spinner.transform.Position = new Vector2(0, 700);
            spinner.transform.Scale = new Vector2(300, 10);
            spinner.Speed = 4.0f;
            spinner.InitializePhysics();

        }

        public override void Tick(GameTime gameTime)
        {
            //Tick the active worlds
            WorldManager.TickWorlds(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, rs, null, CameraManager.GetActiveMatrix());

            //Draw background and collision of active map
            TestMap.RenderLayer(spriteBatch, MapLayer.Background);
            TestMap.RenderLayer(spriteBatch, MapLayer.Collision);

            //Draw the worlds
            WorldManager.DrawWorlds(spriteBatch, gameTime);

            //Draw rest of layers that are on top
            TestMap.RenderLayer(spriteBatch, MapLayer.Lights);
            TestMap.RenderLayer(spriteBatch, MapLayer.Entities);

            spriteBatch.End();

        }

        public override void DrawUI(SpriteBatch spriteBatch, GameTime gameTime, int ScrW, int ScrH)
        {
            base.DrawUI(spriteBatch, gameTime, ScrW, ScrH);

            //Draw the UI layer
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, rs);

            WorldManager.DrawWorldsUI(spriteBatch, gameTime, GameManager.GraphicsDeviceInstance.Viewport.Width, GameManager.GraphicsDeviceInstance.Viewport.Height);

            spriteBatch.Draw(GameManager.WHITE_SOLID, new Rectangle(10, 10, 500, 100), Color.Green);

            spriteBatch.End();
        }
    }
}

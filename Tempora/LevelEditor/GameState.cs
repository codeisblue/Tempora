using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine.LevelEditor.Entities;
using Tempora.Engine;

namespace Tempora.LevelEditor
{
    class LevelEditor : GameState
    {
        //The currently loaded map
        public static Map TestMap;
        public static World TestWorld;

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


            //Create a player
            EEditorController ply = EntityManager.CreateEntity<EEditorController>(TestWorld);


            //Start simulating the world
            TestWorld.Start();
        }

        public override void Tick(GameTime gameTime)
        {
            //Tick the active worlds
            WorldManager.TickWorlds(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            RasterizerState rs = new RasterizerState { MultiSampleAntiAlias = true };
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
    }
}

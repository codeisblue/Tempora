using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine;

namespace TestGame
{
    class ETestEntity : Entity
    {
        private static Texture2D ballSprite;

        //Load assets
        public static void Load()
        {
            ballSprite = GameManager.ContentManager.Load<Texture2D>("ball2");
        }

        //Initialize the entity
        public override void Initialize(World world)
        {
            base.Initialize(world);

            Random r = new Random();

            //Create our physics
            PhysicsObject physObj = CreateSpherePhysics((this.transform.Scale.X / 2.0f) * 0.9f, ChipmunkSharp.cpBodyType.DYNAMIC, 1, 1000);
        }

        //Tick the entity
        public override void Tick(World world, GameTime gameTime)
        {
            base.Tick(world, gameTime);
        }

        //Draw it
        public override void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(world, spriteBatch, gameTime);

            Vector2 offset = new Vector2(ballSprite.Width / 2, ballSprite.Height / 2);
            spriteBatch.Draw(ballSprite, this.transform.Position, null, ballSprite.Bounds, offset, this.transform.Rotation, new Vector2((1.0f / ballSprite.Width) * this.transform.Scale.X, (1.0f / ballSprite.Height) * this.transform.Scale.Y), Color.White, SpriteEffects.None);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine;
using ChipmunkSharp;

namespace TestGame
{
    class ERotatingSolidBlock : Entity
    {
        public float Speed = 1.0f;

        public Color color = Color.White;

        public void InitializePhysics(float density = 1.0f, float bounciness = 0f, float friction = 0.5f)
        {
            PhysicsObject phyObj = CreateBoxPhysics(this.transform.Scale, cpBodyType.KINEMATIC, cp.Infinity, cp.Infinity);
            phyObj.Body.Activate();
        }

        public override void Tick(World world, GameTime gameTime)
        {
            base.Tick(world, gameTime);
            PhysicalBody.Body.SetAngularVelocity(Speed);
        }


        public override void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(world, spriteBatch, gameTime);

            spriteBatch.Draw(GameManager.WHITE_SOLID, new Rectangle(this.transform.Position.ToPoint(), this.transform.Scale.ToPoint()), GameManager.WHITE_SOLID.Bounds, color, this.transform.Rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }
    }
}

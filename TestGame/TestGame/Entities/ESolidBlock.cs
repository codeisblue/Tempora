using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine;
using ChipmunkSharp;

namespace TestGame
{
    class ESolidBlock : Entity
    {
        public Color color = Color.White;
        
        public static void Load()
        {

        }

        public void InitializePhysics()
        {
            PhysicsObject phyObj = CreateBoxPhysics(this.transform.Scale, cpBodyType.STATIC, cp.Infinity, 0);
            phyObj.Body.Activate();
        }

        public override void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(world, spriteBatch, gameTime);

            spriteBatch.Draw(GameManager.WHITE_SOLID, new Rectangle(this.transform.Position.ToPoint(), this.transform.Scale.ToPoint()), GameManager.WHITE_SOLID.Bounds, color, this.transform.Rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }

    }
}

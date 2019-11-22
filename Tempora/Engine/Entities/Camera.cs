using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tempora.Engine.Entities
{
    public class ECamera : Entity
    {
        public Matrix matrix = new Matrix();

        public ECamera()
        {

        }

        public override void Tick(World world, GameTime gameTime)
        {
            base.Tick(world, gameTime);

        }
    }
}

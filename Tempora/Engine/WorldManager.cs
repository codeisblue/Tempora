using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tempora.Engine
{
    /// <summary>
    /// A static class for managing worlds
    /// </summary>
    public class WorldManager
    {
        /// <summary>
        /// A list of all worlds that currently exist
        /// </summary>
        public static List<World> WorldInstances = new List<World>();

        /// <summary>
        /// Ticks all active worlds
        /// </summary>
        /// <param name="gameTime">The gametime to use to tick them</param>
        public static void TickWorlds(GameTime gameTime)
        {
            foreach(World w in WorldInstances)
            {
                if (w.IsActive)
                {
                    w.Tick(gameTime);
                }
            }
        }

        /// <summary>
        /// Renders all active worlds
        /// </summary>
        /// <param name="spriteBatch">spritebatch reference</param>
        /// <param name="gameTime">game time</param>
        public static void DrawWorlds(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (World w in WorldInstances)
            {
                if (w.IsActive)
                {
                    w.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}

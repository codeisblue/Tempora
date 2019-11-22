using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tempora.Engine
{
    /// <summary>
    /// Base class for the game to be built on
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Called when the gamestate should load its content
        /// </summary>
        public virtual void Load()
        {

        }

        /// <summary>
        /// Called when the game starts
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Called each tick of the game
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Tick(GameTime gameTime)
        {

        }

        /// <summary>
        /// Called each time the frame should be redrawn
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        /// <summary>
        /// Called each frame after the draw even.
        /// When drawing during this time, the camera matrix is reset
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference</param>
        /// <param name="gameTime">Game Time</param>
        /// <param name="ScrW">The width of the window in pixels</param>
        /// <param name="ScrH">The height of the window in pixels</param>
        public virtual void DrawUI(SpriteBatch spriteBatch, GameTime gameTime, int ScrW, int ScrH)
        {

        }
    }
}

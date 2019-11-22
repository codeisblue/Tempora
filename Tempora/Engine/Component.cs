using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tempora.Engine
{
    /// <summary>
    /// A base class for a component. A component is like an entity, except it attaches to entities rather that act as a single one.
    /// Components will has a transform and most base events an entity would
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Our position and rotation in space
        /// </summary>
        public Transform transform = new Transform();

        /// <summary>
        /// The entity that owns this component
        /// </summary>
        public Entity owningEntity;

        /// <summary>
        /// Called when ever the component should initialize (when ever its attached to an entity)
        /// </summary>
        /// <param name="world">The world we exist in</param>
        /// <param name="gameTime">Game Time</param>
        public virtual void Initialize(World world, GameTime gameTime)
        {

        }

        /// <summary>
        /// Called when ever the component should tick (each frame)
        /// </summary>
        /// <param name="world"></param>
        /// <param name="gameTime"></param>
        public virtual void Tick(World world, GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Called when ever the component should draw (each frame)
        /// </summary>
        /// <param name="world">The world we are drawing in</param>
        /// <param name="spriteBatch">Sprite batch reference</param>
        /// <param name="gameTime">Game Time</param>
        public virtual void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        /// <summary>
        /// Called when ever the component should draw UI (each frame)
        /// </summary>
        /// <param name="world">The world we are drawing in</param>
        /// <param name="spriteBatch">Sprite batch reference</param>
        /// <param name="gameTime">Game Time</param>
        /// <param name="ScrW">Screen width</param>
        /// <param name="ScrH">Screen height</param>
        public virtual void DrawUI(World world, SpriteBatch spriteBatch, GameTime gameTime, int ScrW, int ScrH)
        {

        }

        /// <summary>
        /// Called when ever a component should be destroyed
        /// </summary>
        /// <param name="world">Game Time</param>
        public virtual void OnDestroy(World world)
        {

        }

    }
}

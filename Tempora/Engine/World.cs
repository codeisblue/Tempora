using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tempora.Engine
{
    /// <summary>
    /// Represents a world
    /// </summary>
    public class World
    {
        /// <summary>
        /// The unique name of the world (currently not needed)
        /// </summary>
        public string Name;

        /// <summary>
        /// Should the world tick, draw etc?
        /// </summary>
        public bool IsActive = false;

        /// <summary>
        /// Set up the world
        /// </summary>
        /// <param name="name">Unique world name</param>
        public World(string name)
        {
            Name = name;

            //Make sure that no world already exists with that name
            foreach(World w in WorldManager.WorldInstances)
                if(w.Name == name)
                    throw new System.InvalidOperationException("Can't create a world with the same name as another active world! (" + name + ")");

            //Register the world with all the necesarry system
            EntityManager.RegisterWorld(this);
            Physics.RegisterWorld(this);
            WorldManager.WorldInstances.Add(this);
        }

        /// <summary>
        /// Clean up the world before destroying it
        /// </summary>
        ~World() 
        {
            //Unregister the world with all the necesary systems
            EntityManager.UnregisterWorld(this);
            Physics.UnregisterWorld(this);
            WorldManager.WorldInstances.Remove(this);
        }

        /// <summary>
        /// Starts simulating the world and initializes all the entities that exist in it
        /// </summary>
        public void Start()
        {
            if (IsActive)
                return;

            IsActive = true;
            EntityManager.InitializeEntities(this);
        }

        /// <summary>
        /// Sets IsActive to false to stop the world from ticking and drawing
        /// </summary>
        public void Stop()
        {
            if (!IsActive)
                return;

            IsActive = false;
        }

        /// <summary>
        /// Ticks all the entities and physics in this world
        /// </summary>
        /// <param name="gameTime"></param>
        public void Tick(GameTime gameTime)
        {
            Physics.TickSimulation(this, gameTime);
            EntityManager.TickEntities(this, gameTime);
        }

        /// <summary>
        /// Draws all the entities in this world
        /// </summary>
        /// <param name="spriteBatch">spritebatch reference</param>
        /// <param name="gameTime">game time</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            EntityManager.DrawEntities(spriteBatch, gameTime, this);
        }

        /// <summary>
        /// Draws all the ui for entities in this world
        /// </summary>
        /// <param name="spriteBatch">spritebatch reference</param>
        /// <param name="gameTime">game time</param>
        /// <param name="ScrW"></param>
        /// <param name="ScrH"></param>
        public void DrawUI(SpriteBatch spriteBatch, GameTime gameTime, int ScrW, int ScrH)
        {
            EntityManager.DrawUIEntities(spriteBatch, gameTime, ScrW, ScrH, this);
        }
    }
}

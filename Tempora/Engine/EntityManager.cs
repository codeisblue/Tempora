using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Reflection;

namespace Tempora.Engine
{
    /// <summary>
    /// A static classed used for creating, ticking and destroying of entities
    /// </summary>
    public class EntityManager
    {
        /// <summary>
        /// A list of entities indexed by the worlds they exist in
        /// </summary>
        private static Dictionary<World, List<Entity>> Entities = new Dictionary<World, List<Entity>>();

        /// <summary>
        /// Creates an entity of type T, adds it to the world and then returns it
        /// NOTE: Initialize() is not called on the entity if you create it after the game has started unless you pass TRUE
        /// </summary>
        /// <typeparam name="T">The type of entity to create</typeparam>
        /// <param name="world">The world to create it in</param>
        /// <param name="shouldInitialize">Should we call Entity.Intialize() now or let the game do it for us?</param>
        /// <returns></returns>
        public static T CreateEntity<T>(World world, bool shouldInitialize = false) where T : Entity, new()
        {
            //Check the world has room for us
            T entity = new T
            {
                //Set the owning world on the entity so it knows who it belongs to
                OwningWorld = world
            };

            //Add it to the world
            EntityManager.Entities[world].Add(entity);

            //If we should intitialize it
            if (shouldInitialize)
                entity.Initialize(world);

            //Return it
            return entity;
        }

        /// <summary>
        /// Registers a world so that it can contain entities
        /// </summary>
        /// <param name="world">The world to register</param>
        public static void RegisterWorld(World world)
        {
            Entities.Add(world, new List<Entity>());
        }

        /// <summary>
        /// Unregisters the world, Destroying all entities that exist in it
        /// </summary>
        /// <param name="world">The world to unregister</param>
        public static void UnregisterWorld(World world)
        {
            //Check if the world exists
            if (Entities.ContainsKey(world))
            {
                //Call event for destroying entities before destruction
                foreach (Entity e in Entities[world])
                    e.OnDestroy(world);

                //Remove the world
                Entities.Remove(world);
            }
        }

        /// <summary>
        /// Gets the entity with that index in that world
        /// </summary>
        /// <param name="world">The world to check</param>
        /// <param name="index">The entities index</param>
        /// <returns>The entity, null if it doesn't exist</returns>
        public static Entity GetEntity(World world, int index)
        {
            if (Entities.ContainsKey(world))
                return Entities[world][index];

            return null;
        }

        /// <summary>
        /// Initializes all the entities
        /// NOTE: If world is null then it will draw all world!
        /// </summary>
        /// <param name="world">The world to draw them in</param>
        public static void InitializeEntities(World world = null)
        {
            if (world != null)
            {
                Entity[] ents = new Entity[Entities[world].Count]; 
                Entities[world].CopyTo(ents);

                foreach (Entity e in ents)
                {
                    if (e != null)
                        e.Initialize(world);
                }
            }
            else
            {
                foreach (KeyValuePair<World, List<Entity>> arr in Entities)
                {
                    foreach (Entity e in arr.Value)
                    {
                        if (e != null)
                            e.Initialize(arr.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Ticks all the entities in that world
        /// NOTE: If world is null, it ticks all worlds!
        /// </summary>
        /// <param name="world">The world to tick entities in</param>
        /// <param name="gameTime">game time</param>
        public static void TickEntities(World world, GameTime gameTime)
        {
            if (world != null)
            {
                foreach (Entity e in Entities[world])
                {
                    if (e != null)
                        e.Tick(world, gameTime);
                }
            }
            else
            {
                foreach (KeyValuePair<World, List<Entity>> arr in Entities)
                {
                    foreach (Entity e in arr.Value)
                    {
                        if (e != null)
                            e.Tick(arr.Key, gameTime);
                    }
                }
            }
        }

        /// <summary>
        /// Draws all the entities in that world
        /// NOTE: If world is
        /// </summary>
        /// <param name="spriteBatch">Our spritebatch reference</param>
        /// <param name="gameTime">Game Time</param>
        /// <param name="world">The world to draw</param>
        public static void DrawEntities(SpriteBatch spriteBatch, GameTime gameTime, World world = null)
        {
            if (world != null)
            {
                foreach (Entity e in Entities[world])
                {
                    if (e != null)
                        e.Draw(world, spriteBatch, gameTime);
                }
            }
            else
            {
                foreach (KeyValuePair<World, List<Entity>> arr in Entities)
                {
                    foreach (Entity e in arr.Value)
                    {
                        if (e != null)
                            e.Draw(arr.Key, spriteBatch, gameTime);
                    }
                }
            }
        }

        public static void DrawUIEntities(SpriteBatch spriteBatch, GameTime gameTime, int ScrW, int ScrH, World world = null)
        {
            if (world != null)
            {
                foreach (Entity e in Entities[world])
                {
                    if (e != null)
                        e.DrawUI(world, spriteBatch, gameTime, ScrW, ScrH);
                }
            }
            else
            {
                foreach (KeyValuePair<World, List<Entity>> arr in Entities)
                {
                    foreach (Entity e in arr.Value)
                    {
                        if (e != null)
                            e.DrawUI(arr.Key, spriteBatch, gameTime, ScrW, ScrH);
                    }
                }
            }
        }

        /// <summary>
        /// This functions will loop over all classes that are based on Entity and call the static Load() function on them
        /// </summary>
        public static void LoadEntities()
        {
            //Get all types in assembly that use our class as a base
            var types = Assembly.GetCallingAssembly().GetTypes().Where(x => x.BaseType == typeof(Entity));
            foreach (var x in types)
            {
                //Find the method Load
                MethodInfo loadMethod = x.GetMethod("Load");

                //Invoke if it exists
                if (loadMethod != null)
                    loadMethod.Invoke(x, new object[] { });
            }
        }

    }
}

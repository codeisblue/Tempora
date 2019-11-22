using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ChipmunkSharp;

namespace Tempora.Engine
{
    /// <summary>
    /// A physics object constists of a Body and a Shape that was used to create that body
    /// </summary>
    public struct PhysicsObject
    {
        public cpBody Body;
        public cpShape Shape;
    }

    /// <summary>
    /// A static class to handle creating and destroying physics objects as well as simulating them
    /// </summary>
    public class Physics
    {
        /// <summary>
        /// A list of the worlds and there copies
        /// </summary>
        private static Dictionary<Tempora.Engine.World, cpSpace> Worlds = new Dictionary<Tempora.Engine.World, cpSpace>();

        /// <summary>
        /// The scale at which the physic simulation runs at
        /// </summary>
        public const float PHYSICS_TRANSFORM_SCALE = 32;

        /// <summary>
        /// Maximum DT allowed in a single frame
        /// </summary>
        public const float PHYSICS_MAX_DT = 0.128f;

        /// <summary>
        /// Used to initialize physics stuff pre-game
        /// </summary>
        public static void Initialize()
        {

        }

        /// <summary>
        /// Creates a copy of the registered world to run physics simulations in
        /// </summary>
        /// <param name="world">The world to create a copy of</param>
        public static void RegisterWorld(World world)
        {
            if (Worlds.ContainsKey(world))
                throw new System.InvalidOperationException("Tried to register physics world when one is already registered!");
            else
            {
                //Create the space
                cpSpace space = new cpSpace();
                space.SetGravity(new cpVect(0, 10));

                Worlds.Add(world, space);
            }
        } 

        /// <summary>
        /// Removes a world and all of its bodies.
        /// </summary>
        /// <param name="world">The world to unregister</param>
        public static void UnregisterWorld(World world)
        {
            if (Worlds.ContainsKey(world))
            {
                Worlds[world].Clear();
                Worlds.Remove(world);
            }
        }

        //TODO CREATE FIXED TIMESTEP LOOP FOR PHYSICS

        /// <summary>
        /// Ticks the simulation in that game world
        /// NOTE: The physics will not simulate past the max DT. Calling this too slow will cause the physics to slow down even if the dt is acurate
        /// </summary>
        /// <param name="world"></param>
        /// <param name="gameTime"></param>
        public static void TickSimulation(World world, GameTime gameTime)
        {
            if(gameTime.ElapsedGameTime.TotalSeconds > PHYSICS_MAX_DT)
            {
                Console.WriteLine("WARNING! SIMULATION GONE ABOVE MAX DT, SIMULATING AT MAX DT INSTEAD");
                Worlds[world].Step(PHYSICS_MAX_DT);
                return;
            }
            Worlds[world].Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Creates a physics body and adds it to the world.
        /// It also adds shape to the body, unless it is null
        /// NOTE: Be sure to call activate on the body when you are ready for it to be simulated!
        /// </summary>
        /// <param name="world">The world to add to the body too</param>
        /// <param name="bodyType">The type of body to create</param>
        /// <param name="mass">The mass of the object</param>
        /// <param name="moment">The moment of inertia</param>
        /// <returns></returns>
        public static cpBody CreateBody(World world, cpBodyType bodyType = cpBodyType.DYNAMIC, float mass = 1f, float moment = 1f)
        {
            //Create the body and set the type
            cpBody body = new cpBody(mass, moment);
            body.SetBodyType(bodyType);

            //Add it to the world
            Worlds[world].AddBody(body);

            //Return it
            return body;
        }

        /// <summary>
        /// Create a physics body and a circle shape and adds it to the world
        /// NOTE: Be sure to call activate on the body when you are ready for it to be simulated!
        /// </summary>
        /// <param name="world">The world to add it to</param>
        /// <param name="position">The starting position for the circle</param>
        /// <param name="rotation">The starting rotation for the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="bodyType">The body type of the physics body</param>
        /// <param name="mass">The mass of the physics body</param>
        /// <param name="moment">The moment of inertia for the physics body</param>
        /// <returns></returns>
        public static PhysicsObject CreateCircle(World world, Vector2 position, float rotation, float radius, cpBodyType bodyType = cpBodyType.DYNAMIC, float mass = 1, float moment = 1)
        {
            //Create a body and set its transform
            cpBody body = CreateBody(world, bodyType, mass, moment);
            body.SetPosition(new cpVect(position.X / PHYSICS_TRANSFORM_SCALE, position.Y / PHYSICS_TRANSFORM_SCALE));
            body.SetAngle(rotation);

            //Create the circle shape and add it to the world
            cpCircleShape circleShape = new cpCircleShape(body, radius / PHYSICS_TRANSFORM_SCALE, cpVect.Zero);
            Worlds[world].AddShape(circleShape);

            //Return the phyics object
            return new PhysicsObject() { Body = body, Shape = circleShape };
        }

        /// <summary>
        /// Create a physics body and a box shape and adds it to the world
        /// NOTE: Be sure to call activate on the body when you are ready for it to be simulated!
        /// </summary>
        /// <param name="world">The world to add it to</param>
        /// <param name="position">The position of the center of the box</param>
        /// <param name="rotation">The rotation of the box</param>
        /// <param name="size">The size of the box</param>
        /// <param name="bodyType">The body type of the physics body</param>
        /// <param name="mass">The mass of the physics body</param>
        /// <param name="moment">The moment of inertia for the physics body</param>
        /// <returns></returns>
        public static PhysicsObject CreateBox(World world, Vector2 position, float rotation, Vector2 size, cpBodyType bodyType = cpBodyType.DYNAMIC, float mass = 1, float moment = 1)
        {
            //Create body
            cpBody body = CreateBody(world, bodyType, mass, moment);
            body.SetPosition(new cpVect(position.X / PHYSICS_TRANSFORM_SCALE, position.Y / PHYSICS_TRANSFORM_SCALE));
            body.SetAngle(rotation);

            cpPolyShape box = cpPolyShape.BoxShape(body, size.X / PHYSICS_TRANSFORM_SCALE, size.Y / PHYSICS_TRANSFORM_SCALE, 0.1f);
            Worlds[world].AddShape(box);

            //Return the Physics body
            return new PhysicsObject() { Body = body, Shape = box };
        }
    }
}

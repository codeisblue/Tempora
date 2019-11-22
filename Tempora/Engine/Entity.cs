using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChipmunkSharp;


namespace Tempora.Engine
{
    //The entity base class
    public class Entity
    {
        /////////////////////////
        /// COMPONENT RELATED ///
        /////////////////////////

        ///A list of all the components on this entity
        private List<Component> components = new List<Component>();

        /// <summary>
        /// Returns the first component the matches the type
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            foreach(Component c in components)
            {
                if((T)c != null)
                {
                    return (T)c;
                }
            }

            //We didn't find a component that matched
            return null;
        }

        /// <summary>
        /// Will create and return the component of type T and adds it to the entity
        /// </summary>
        /// <typeparam name="T">The type of component to create</typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : Component, new()
        {
            T comp = new T
            {
                //Set the component owner to us
                owningEntity = this
            };

            //Set the component transform parent to us
            comp.transform.parent = this.transform;

            //Add the component to the list
            components.Add(comp);

            return comp;
        }

        /// <summary>
        /// Tries to remove the component from the game object
        /// </summary>
        /// <param name="c">The component to remove</param>
        public void RemoveComponent(Component c)
        {
            components.Remove(c);
        }


        /////////////////////
        /// EVENT RELATED ///
        /////////////////////

        /// <summary>
        /// Called when the entity should initialize
        /// </summary>
        /// <param name="world">The world we initialized in</param>
        public virtual void Initialize(World world)
        {

        }

        /// <summary>
        /// Called when the entity should tick
        /// </summary>
        /// <param name="world">The world we ticked in</param>
        /// <param name="gameTime">The game time</param>
        public virtual void Tick(World world, GameTime gameTime)
        {
            //Update the transform to the physics
            this.transform.UpdateToBodyTransform();

            foreach(Component c in components)
            {
                c.Tick(world, gameTime);
            }
        }

        /// <summary>
        /// Called when the entity should draw
        /// </summary>
        /// <param name="world">The world we are being drawn in</param>
        /// <param name="spriteBatch">Reference to the sprite batch</param>
        /// <param name="gameTime">The game time</param>
        public virtual void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Component c in components)
            {
                c.Draw(world, spriteBatch, gameTime);
            }
        }

        public virtual void DrawUI(World world, SpriteBatch spriteBatch, GameTime gameTime, int ScrW, int ScrH)
        {
            foreach (Component c in components)
            {
                c.DrawUI(world, spriteBatch, gameTime, ScrW, ScrH);
            }
        }

        /// <summary>
        /// Called just before the entity is destroyed
        /// </summary>
        /// <param name="world">The world we was destroyed in</param>
        public virtual void OnDestroy(World world)
        {
            foreach (Component c in components)
            {
                c.OnDestroy(world);
            }
        }

        /////////////////////
        /// INPUT RELATED ///
        /////////////////////

        public bool Possesable = false;
        public InputController Posseser;
        public bool IsPossesed = false;
        public World OwningWorld;

        //Called when a player tries to posses this object
        public virtual bool OnPossesed(InputController posseser)
        {
            //Don't let more than one thing posses us! 
            if (IsPossesed)
                return false;

            //If we are possible, then posses else don't ¯\_(ツ)_/¯
            if (Possesable)
            {
                Posseser = posseser;
                IsPossesed = true;
                return true;
            }
            else
                return false;
        }

        public virtual void OnUnpossesed()
        {
            IsPossesed = false;
            Posseser = null;
        }

        public virtual void OnInputEvent(InputAction action, InputEventType type, float metaData, GameTime gameTime)
        {
            
        }


        /////////////////////////
        /// TRANSFORM RELATED ///
        /////////////////////////

        //The transform component that represents this entity
        public Transform transform = new Transform();

        //Sets the position of the entity
        public virtual void SetPosition(Vector2 newPosition)
        {
            transform.Position = newPosition;
        }

        //Gets the position
        public virtual Vector2 GetPosition()
        {
            return transform.Position;
        }

        //Sets the rotation
        public virtual void SetRotation(float newRotation)
        {
            transform.Rotation = newRotation;
        }

        //Gets the rotation
        public virtual float GetRotation()
        {
            return transform.Rotation;
        }


        ///////////////////////
        /// PHYSICS RELATED ///
        ///////////////////////

        ///The physical body related to us
        public PhysicsObject PhysicalBody;

        /// <summary>
        /// Returns the physical velocity of the entity if it has physics, zero otherwise
        /// </summary>
        /// <returns>The velocity</returns>
        public Vector2 GetVelocity()
        {
            if(PhysicalBody.Body != null)
            {
                cpVect vel = PhysicalBody.Body.GetVelocity();
                return new Vector2(vel.x, vel.y);
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Returns the angular velocity of the entity if it has physics, zero otherwise
        /// </summary>
        /// <returns></returns>
        public float GetAngularVelocity()
        {
            if (PhysicalBody.Body != null)
                return PhysicalBody.Body.GetAngularVelocity();
            return 0;
        }

        //Create a cricle physics body at the location of the 
        public PhysicsObject CreateSpherePhysics(float radius, cpBodyType bodyType = cpBodyType.DYNAMIC, float mass = 1, float moment = 1)
        {
            //Create the body and activate it
            PhysicalBody = Physics.CreateCircle(OwningWorld, this.transform.Position, this.transform.Rotation, radius, bodyType, mass, moment);

            transform.HasPhysicsAttached = true;
            transform.AttachedPhysicsBody = PhysicalBody.Body;

            return PhysicalBody;
        }

        //Create a cricle physics body at the location of the 
        public PhysicsObject CreateBoxPhysics(Vector2 size, cpBodyType bodyType = cpBodyType.DYNAMIC, float mass = 1, float moment = 1)
        {
            PhysicalBody = Physics.CreateBox(OwningWorld, this.transform.Position, this.transform.Rotation, size, bodyType, mass, moment);

            transform.HasPhysicsAttached = true;
            transform.AttachedPhysicsBody = PhysicalBody.Body;

            return PhysicalBody;
        }

        //Destroys any physics bodies owned by this entity
        public void DestroyPhysics()
        {

        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using ChipmunkSharp;

namespace Tempora.Engine
{
    /// <summary>
    /// Represents a location, rotation and scale in 2D space
    /// Transforms can also be controlled from by a physics body or through parenting
    /// </summary>
    public class Transform
    {
        //internal values
        private Vector2 _position = Vector2.Zero;
        private Vector2 _scale = Vector2.One;
        private float _rotation = 0;

        /// <summary>
        /// Our parent
        /// </summary>
        public Transform parent = null;

        /// <summary>
        /// A list of all transforms that are children to this one
        /// </summary>
        public List<Transform> children;

        /// <summary>
        /// Is there a physics object attached to this transform
        /// </summary>
        public bool HasPhysicsAttached = false;

        /// <summary>
        /// The attached physics body if one exists
        /// </summary>
        public cpBody AttachedPhysicsBody;

        /// <summary>
        /// The position of the transform
        /// </summary>
        public Vector2 Position
        {
            get {
                if (HasPhysicsAttached)
                {
                    cpVect bodyPos = AttachedPhysicsBody.GetPosition();
                    return new Vector2(bodyPos.x, bodyPos.y) * Physics.PHYSICS_TRANSFORM_SCALE;
                }

                if (parent == null)
                    return _position;

                return parent.Position + _position;
            }
            set
            {
                if (HasPhysicsAttached)
                    AttachedPhysicsBody.SetPosition(new cpVect(value.X / Physics.PHYSICS_TRANSFORM_SCALE, value.Y / Physics.PHYSICS_TRANSFORM_SCALE));
                _position = value;
            }
        }

        /// <summary>
        /// The scale of the transform
        /// </summary>
        public Vector2 Scale //TODO: CHANGE PHYSICS SHAPES TO MATCH NEW SCALE!
        {
            get
            {
                if (parent == null)
                    return _scale;

                return parent.Scale * _scale;
            }
            set
            {
                _scale = value;
            }
        }

        /// <summary>
        /// The rotation of the transform
        /// </summary>
        public float Rotation
        {
            get
            {
                if (HasPhysicsAttached)
                    return AttachedPhysicsBody.GetAngle();

                if (parent == null)
                    return _rotation;

                return parent.Rotation + _rotation;
            }
            set
            {
                if (HasPhysicsAttached)
                    AttachedPhysicsBody.SetTransform(AttachedPhysicsBody.GetPosition(), value);
                _rotation = value;
            }
        }

        /// <summary>
        /// Returns the forward facing vector relative to the transforms rotation
        /// </summary>
        /// <returns>Forward facing vector</returns>
        public Vector2 Forward()
        {
            return new Vector2(MathF.Sin(Rotation), MathF.Cos(Rotation));
        }

        /// <summary>
        /// Returns the right facing vector relative to the transforms rotation
        /// </summary>
        /// <returns>Right facing vector</returns>
        public Vector2 Right()
        {
            return new Vector2(MathF.Sin(Rotation + 90), MathF.Cos(Rotation + 90)); //wrong :)
        }

        /// <summary>
        /// Updates the transforms position to the physics bodys position
        /// NOTE: When settings the position directly of a transform, you are also setting the position of the physics object
        /// This create unwanted behavoir so use this function to set the position without changing the rigidbodys transform
        /// </summary>
        public void UpdateToBodyTransform()
        {
            if (HasPhysicsAttached)
            {
                cpVect pos = AttachedPhysicsBody.GetPosition();
                _position = new Vector2(pos.x * Physics.PHYSICS_TRANSFORM_SCALE, pos.y * Physics.PHYSICS_TRANSFORM_SCALE);
                _rotation = AttachedPhysicsBody.GetAngle();
            }
        }
    }
}

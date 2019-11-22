using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine;
using Tempora.Engine.Entities;
using Tempora.Engine.Components;

namespace Tempora.Engine.LevelEditor.Entities
{
    class EEditorController : Entity
    {
        //The input controller
        private InputController inputController;

        //The sprite that represents our player
        private static Texture2D ControllerSprite;

        //Our sprite animation
        private CSpriteAnimation animation;

        //Player Properties
        public float Speed = 0.3f; //Our max speed
        public float Acceleration = 16f; //Rate at which we get to max speed (units per second)

        private float zoom = 0.5f;


        private Vector2 MoveDirection = Vector2.Zero;

        public ECamera PlayerCamera;
        public bool FastMode = false;

        public static void Load()
        {
            ControllerSprite = GameManager.ContentManager.Load<Texture2D>("dino");
        }

        public override void Initialize(World world)
        {
            base.Initialize(world);

            //Make it so we can be possesed
            Possesable = true;

            //Create the input controller
            inputController = new InputController();

            //Attach us to the input controller
            bool attached = inputController.Posses(this);

            if (!attached)
                throw new System.InvalidOperationException("Failed to attach input controller to player entity!");

            //Create the camera to represent the player
            PlayerCamera = EntityManager.CreateEntity<ECamera>(OwningWorld, true);
            CameraManager.SetActiveCamera(PlayerCamera);

            //Create the sprite that represents us
            animation = AddComponent<CSpriteAnimation>();
            animation.SetupAnimation(ControllerSprite, 24, 1);
            animation.SetAnimationFrames(new int[] { 1, 2, 3, 4});
            animation.SetSpeed(2);
            animation.Play();

            transform.Scale = new Vector2(4, 4);
        }


        public override void Tick(World world, GameTime gameTime)
        {
            base.Tick(world, gameTime);

            //Tick the input controller
            inputController.TickInput(gameTime);

            //Move the player in the direction
            if (MoveDirection.Length() != 0)
            {

                MoveDirection.Normalize();

                float _speed = Speed;

                if (FastMode)
                    _speed *= 3;

                this.transform.Position += (MoveDirection * ((_speed * zoom) + 0.1f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                //Clear move direction
                MoveDirection = Vector2.Zero;

            }


            PlayerCamera.transform.Position = this.transform.Position;
            float result = MathHelper.Lerp(PlayerCamera.transform.Scale.X, MathHelper.Lerp(0.3f, 6f, zoom), 5 * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));
            PlayerCamera.transform.Scale = new Vector2(result, result);
        }

        public override void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(world, spriteBatch, gameTime);

            //spriteBatch.Draw(ControllerSprite, new Rectangle(transform.position.ToPoint(), new Vector2(ControllerSprite.Bounds.Width * transform.scale.X, ControllerSprite.Bounds.Height * transform.scale.Y).ToPoint()), ControllerSprite.Bounds, Color.White);
        }


        public override void OnInputEvent(InputAction action, InputEventType type, float metaData, GameTime gameTime)
        {
            base.OnInputEvent(action, type, metaData, gameTime);

            switch (action)
            {
                case InputAction.FORWARD: if (type == InputEventType.Pressed || type == InputEventType.Held) { MoveDirection += new Vector2(0, -1); } break;
                case InputAction.BACKWARD: if (type == InputEventType.Pressed || type == InputEventType.Held) { MoveDirection += new Vector2(0, 1); } break;
                case InputAction.LEFT: if (type == InputEventType.Pressed || type == InputEventType.Held) { MoveDirection += new Vector2(-1, 0); } break;
                case InputAction.RIGHT: if (type == InputEventType.Pressed || type == InputEventType.Held) { MoveDirection += new Vector2(1, 0); } break;
                case InputAction.MWHEEL_DOWN: zoom += 0.025f; zoom = Math.Clamp(zoom, 0.0f, 1.0f); break;
                case InputAction.MWHEEL_UP: zoom -= 0.025f; zoom = Math.Clamp(zoom, 0.0f, 1.0f); break;
                case InputAction.RUN: if (type == InputEventType.Pressed) FastMode = true; else if (type == InputEventType.Released) FastMode = false; break;
            }
        }
    }
}

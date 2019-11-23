using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine;
using Tempora.Engine.Entities;
using Tempora.Engine.Components;


namespace TestGame
{
    public class EPlayer : Entity
    {
        //The input controller
        private InputController inputController;
        

        //The sprite that represents our player
        private static Texture2D PlayerSprite;

        //Our animation component
        private CSpriteAnimation animation;


        //animation tests
        private static Dictionary<string, int[]> animations = new Dictionary<string, int[]>();
        private string currentAnimation = "idle";

        //Player Properties
        public float Speed = 0.3f; //Our max speed
        public float Acceleration = 16f; //Rate at which we get to max speed (units per second)


        //Private properties
        private float playerAcceleration = 0;

        private Vector2 MoveDirection = Vector2.Zero;

        public ECamera PlayerCamera;

        public static void Load()
        {
            PlayerSprite = GameManager.ContentManager.Load<Texture2D>("dino");

            //Set up animation
            animations.Add("idle", new int[] { 1, 2, 3, 4 });
            animations.Add("run_sideways", new int[] { 5, 6, 7, 8, 9, 10});
            animations.Add("run", new int[] { 18, 19, 20, 21, 22, 23, 24 });
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
            animation.SetupAnimation(PlayerSprite, 24, 1);
            animation.SetAnimationFrames(animations["idle"]);
            currentAnimation = "idle";
            animation.SetSpeed(2);
            animation.Play();

            transform.Scale = new Vector2(4, 4);

            //CreateSpherePhysics(10);
            PhysicsObject b = CreateSpherePhysics(32, ChipmunkSharp.cpBodyType.DYNAMIC, 10000, 1000000000);
            
            

        }

        float camZoom = 0;
        string runType = "run_sideways";
        public override void Tick(World world, GameTime gameTime)
        {
            

            Console.WriteLine(this.transform.Position);

            //PhysicalBody.Body.UpdatePosition((float)gameTime.ElapsedGameTime.TotalSeconds);


            //Tick the input controller
            inputController.TickInput(gameTime);


            float zoomLerpSpeed = 0.1f;

            

            //Move the player in the direction
            if (MoveDirection.Length() != 0)
            {
                playerAcceleration = MathHelper.Lerp(playerAcceleration, Speed, Acceleration * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));

                Vector2 result = (MoveDirection * playerAcceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                MoveDirection.Normalize();
                PhysicalBody.Body.SetVelocity(new ChipmunkSharp.cpVect(result.X, PhysicalBody.Body.GetVelocity().y));
                //this.transform.Position += (MoveDirection * playerAcceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                //Handle flipping our animation based on our move direction
                float dot = Vector2.Dot(MoveDirection, new Vector2(1, 0));
                if (dot < 0)
                    animation.ShouldFlipAnimation = true;
                else if (dot != 0)
                    animation.ShouldFlipAnimation = false;
                else
                    runType = "run_sideways";

                if (dot != 0)
                    runType = "run";

                //Clear move direction
                MoveDirection = Vector2.Zero;

            }
            else
            {
                PhysicalBody.Body.SetVelocity(new ChipmunkSharp.cpVect(0, PhysicalBody.Body.GetVelocity().y));
                playerAcceleration = MathHelper.Lerp(playerAcceleration, 0, Acceleration * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));
                zoomLerpSpeed = 0.2f;
            }

            base.Tick(world, gameTime);

            //Animation switching
            if (playerAcceleration > 0.1 && currentAnimation != runType)
            {
                animation.SetAnimationFrames(animations[runType]);
                currentAnimation = runType;
                animation.Reset();
            }

            if (playerAcceleration < 0.1 && currentAnimation != "idle")
            {
                animation.SetAnimationFrames(animations["idle"]);
                currentAnimation = "idle";
            }

            //Update camera to follow player?

            PlayerCamera.transform.Position = Vector2.Lerp(PlayerCamera.transform.Position, this.transform.Position, 2 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
            PlayerCamera.transform.Rotation = (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 800) / 64.0f;

            ChipmunkSharp.cpVect vel = PhysicalBody.Body.GetVelocity();

            float zoom = MathHelper.Lerp(0.2f, 1f, (float)Math.Clamp(((vel.x * vel.x) + (vel.y * vel.y)) / Speed, 0, 1.0f));
            camZoom = MathHelper.Lerp(camZoom, zoom, zoomLerpSpeed * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));
            PlayerCamera.transform.Scale = new Vector2(camZoom * 1000, camZoom * 1000);

        }

        public override void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(world, spriteBatch, gameTime);

            //spriteBatch.Draw(PlayerSprite, new Rectangle(transform.Position.ToPoint(), new Vector2(PlayerSprite.Bounds.Width * transform.Scale.X, PlayerSprite.Bounds.Height * transform.Scale.Y).ToPoint()), PlayerSprite.Bounds, Color.White);
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
                case InputAction.JUMP: if (type == InputEventType.Pressed) { PhysicalBody.Body.SetVelocity(new ChipmunkSharp.cpVect(PhysicalBody.Body.GetVelocity().x, -10)); } break;
            }
        }
    }
}

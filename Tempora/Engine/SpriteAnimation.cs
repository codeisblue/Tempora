using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
 

namespace Tempora.Engine
{
    /// <summary>
    /// A class that handles sprite animations
    /// </summary>
    class SpriteAnimation
    {
        /// <summary>
        /// How fast should the animation play
        /// </summary>
        public float AnimationSpeed = 1.0f;

        /// <summary>
        /// The frames of the currently loaded sequence
        /// </summary>
        public int[] AnimationFrames;

        /// <summary>
        /// The current animation time (0-1)
        /// </summary>
        public float AnimationTime = 0;

        /// <summary>
        /// Should the animation draw horizontally flipped
        /// </summary>
        public bool AnimationIsFlipped = false;

        /// <summary>
        /// The atlas the sprite animation is based on
        /// </summary>
        public Texture2D SpriteAnimationTexture;

        /// <summary>
        /// Is the animation currently playing
        /// </summary>
        public bool IsPlaying = false;

        /// <summary>
        /// The X size in pixels of the cell size
        /// </summary>
        public int CellSizeX = 0;

        /// <summary>
        /// The Y size in pixels of the cell size
        /// </summary>
        public int CellSizeY = 0;

        /// <summary>
        /// An array of rectangles that convert an index to a frame rect
        /// </summary>
        private Rectangle[] Frames;

        /// <summary>
        /// Set up the sprite animation
        /// </summary>
        /// <param name="texture">The texture to use</param>
        /// <param name="gridX">How many cells horizontally</param>
        /// <param name="gridY">How many cells vertically</param>
        /// <param name="animationFrames">The frames to use for the animation</param>
        public SpriteAnimation(Texture2D texture, int gridX, int gridY, int[] animationFrames)
        {
            //Construct frames array
            Frames = new Rectangle[gridX * gridY];

            //Set texture
            SpriteAnimationTexture = texture;

            //Store the image dimentions for easier access below
            int imageWidth = SpriteAnimationTexture.Width;
            int imageHeight = SpriteAnimationTexture.Height;

            //Set cell sizes
            CellSizeX = (int)(imageWidth / gridX);
            CellSizeY = (int)(imageHeight / gridY);

            //Store the animation frames for later
            AnimationFrames = animationFrames;

            //Populate the frames so we can reference them easily later
            int index = 0;
            for (int y = 0; y < gridY; y++)
                for (int x = 0; x < gridX; x++)
                {
                    Frames[index] = new Rectangle(new Vector2((imageWidth / gridX) * x, (imageHeight / gridY) * y).ToPoint(), 
                                    new Vector2(imageWidth / gridX, imageWidth / gridX).ToPoint());

                    index++;
                }
        }

        /// <summary>
        /// Returns a rectangle that matches the current frame of the animation
        /// </summary>
        /// <returns>Frame</returns>
        public Rectangle GetCurrentFrame()
        {
            int frame = (int)Math.Floor(AnimationTime * AnimationFrames.Length);

            return Frames[AnimationFrames[frame] - 1];
        }

        /// <summary>
        /// Ticks the animation, advancing it's animation time based on speed
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void TickAnimation(GameTime gameTime)
        {
            if (IsPlaying)
            {
                AnimationTime += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) * AnimationSpeed;
                AnimationTime %= 1f; //Limit animation time between 0 and one
            }
        }
    }
}

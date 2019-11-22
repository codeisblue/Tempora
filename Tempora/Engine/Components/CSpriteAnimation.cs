using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tempora.Engine;

namespace Tempora.Engine.Components
{
    /// <summary>
    /// A component used for display and playing animation sprite sheets
    /// </summary>
    public class CSpriteAnimation : Component
    {
        //The sprite animation class used for handling it
        private SpriteAnimation spriteAnimation;

        //Should we auto draw the sprite when the component is ticked?
        public bool ShouldAutoDraw = true;

        //The color tint of the animation
        public Color SpriteColorTint = Color.White;

        //Should it draw the animation flipped?
        public bool ShouldFlipAnimation = false;

        //Used to set up the texture and grid size of the animation, this also regenertes the sprite animation
        public void SetupAnimation(Texture2D texture, int gridX, int gridY)
        {
            spriteAnimation = new SpriteAnimation(texture, gridX, gridY, new int[] { 0 });
        }

        //Used to change the frames the animation is currently reading from
        public void SetAnimationFrames(int[] frames)
        {
            spriteAnimation.AnimationFrames = frames;
        }

        //Tells the animation to start playing
        public void Play()
        {
            spriteAnimation.IsPlaying = true;
        }

        //Pauses the animation at the current frame
        public void Pause()
        {
            spriteAnimation.IsPlaying = false;
        }

        //Stops the animation and resets its frame to frame 0
        public void Stop()
        {
            spriteAnimation.IsPlaying = false;
            spriteAnimation.AnimationTime = 0;
        }

        //Resets the animation time to 0 and starts playing if not already
        public void Reset()
        {
            spriteAnimation.IsPlaying = true;
            spriteAnimation.AnimationTime = 0;
        }

        //Sets the speed of the animation
        public void SetSpeed(float speed)
        {
            spriteAnimation.AnimationSpeed = speed;
        }

        //Used for manually drawing the sprte, if for example you wanted to draw it at a specific time
        public void DrawCurrentFrame(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (ShouldFlipAnimation)
                spriteBatch.Draw(spriteAnimation.SpriteAnimationTexture,
                                 new Rectangle(transform.Position.ToPoint(),
                                 new Vector2(spriteAnimation.CellSizeX * transform.Scale.X, spriteAnimation.CellSizeY * transform.Scale.Y).ToPoint()),
                                 spriteAnimation.GetCurrentFrame(), SpriteColorTint, this.transform.Rotation, new Vector2(spriteAnimation.CellSizeX/2, spriteAnimation.CellSizeY/2), SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatch.Draw(spriteAnimation.SpriteAnimationTexture,
                                 new Rectangle(transform.Position.ToPoint(),
                                 new Vector2(spriteAnimation.CellSizeX * transform.Scale.X, spriteAnimation.CellSizeY * transform.Scale.Y).ToPoint()),
                                 spriteAnimation.GetCurrentFrame(), SpriteColorTint, this.transform.Rotation, new Vector2(spriteAnimation.CellSizeX/2, spriteAnimation.CellSizeY/2), SpriteEffects.None, 0);

        }

        //Draw the sprite if required
        public override void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(world, spriteBatch, gameTime);

            if (ShouldAutoDraw)
            {
                DrawCurrentFrame(spriteBatch, gameTime);
            }

        }

        //Tick the sprite animation so it can advance time
        public override void Tick(World world, GameTime gameTime)
        {
            base.Tick(world, gameTime);

            spriteAnimation.TickAnimation(gameTime);
        }
    }
}

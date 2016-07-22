using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apocalyptic_Sunrise
{
    public abstract class AnimatedSprite : MobileObject
    {
        #region Variables
        public enum myDirection { none, left, right, up, down };
        protected myDirection currentDir = myDirection.none;
        public Texture2D sTexture;
        public Vector2 sPosition;
        private int frameIndex;
        private double timeElapsed;
        private double timeToUpdate;
        protected string currentAnimation;
        public Vector2 sDirection = Vector2.Zero;

        private Dictionary<string, Rectangle[]> sAnimations = new Dictionary<string, Rectangle[]>();
        private Dictionary<string, Vector2> sOffsets = new Dictionary<string, Vector2>();
        #endregion
        #region Properties
        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        #endregion

        #region collection
       
       
        public AnimatedSprite(Vector2 position)
        {
            sPosition = position;
        }
        public void AddAnimation(int frames, int yPos, int xStartFrame, string name, int width, int height, Vector2 offset)
        {

            Rectangle[] Rectangles = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
            }
            sAnimations.Add(name, Rectangles);
            sOffsets.Add(name, offset);
        }
        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                if (frameIndex < sAnimations[currentAnimation].Length - 1)
                {
                    frameIndex++;
                }
                else
                {
                    AnimationDone(currentAnimation);
                    frameIndex = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sTexture, sPosition + sOffsets[currentAnimation], sAnimations[currentAnimation][frameIndex], Color.White);
        }
        public void PlayAnimation(string name)
        {
            if (currentAnimation != name && currentDir == myDirection.none)
            {
                currentAnimation = name;
                frameIndex = 0;
            }
        }

        public abstract void AnimationDone(string animation);
#endregion

    }
}

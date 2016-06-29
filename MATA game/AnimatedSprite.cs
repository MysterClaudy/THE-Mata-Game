using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MATA_game
{
    abstract class AnimatedSprite : MobileObject
    {
        public enum myDirection { None, Left, Right, Up, Down};
        protected myDirection currentDir = myDirection.None;
        public Texture2D sTexture;
        protected Vector2 sPosition;
        private int frameIndex;
        private double timeElapsed;
        private double timetoUpdate;
        protected string currentAnimation;
        protected Vector2 sDirection = Vector2.Zero;

        private Dictionary<string, Rectangle[]> sAnimations = new Dictionary<string, Rectangle[]>();
        private Dictionary<string, Vector2> Offsets = new Dictionary<string, Vector2>();

        public int framesPerSecond
        {
            set { timetoUpdate = (1f / value); }
        }

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
            Offsets.Add(name, offset);
        }

        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if(timeElapsed > timetoUpdate)
            {
                timeElapsed = +timetoUpdate;

                if(frameIndex < sAnimations[currentAnimation].Length - 1)
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
            spriteBatch.Draw(sTexture, m_position + Offsets[currentAnimation], sAnimations[currentAnimation][frameIndex], Color.White);
        }

        public void PlayAnimation(string name)
        {
            if(currentAnimation != name && currentDir == myDirection.None)
            {
                currentAnimation = name;
                frameIndex = 0;
            }
        }

        public abstract void AnimationDone(string animation);
        
    }
}

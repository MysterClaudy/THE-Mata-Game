using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MATA_game
{
    class AnimatedSprite
    {

        #region Properties
        KeyboardState currentKBState;
        KeyboardState previousKBState;

        Texture2D spriteTexture;
        float timer = 0f;
        float interval = 200f;
        int currentFrame = 0;
        int spriteWidth = 32;
        int spriteHeight = 48;
        int spriteSpeed = 2;
        Rectangle sourceRect;
        Vector2 position;
        Vector2 origin;
        
        #endregion


        #region Collectors
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Texture2D Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.spriteTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

            if (currentKBState.GetPressedKeys().Length == 0)
            {
                if (currentFrame > 0 && currentFrame < 4)
                {
                    currentFrame = 0;
                }
                if (currentFrame > 4 && currentFrame < 8)
                {
                    currentFrame = 4;
                }
                if (currentFrame > 8 && currentFrame < 12)
                {
                    currentFrame = 8;
                }
                if (currentFrame > 12 && currentFrame < 16)
                {
                    currentFrame = 12;
                }
            }

            if(currentKBState.IsKeyDown(Keys.Space))
            {
                spriteSpeed = 3;
                interval = 100;
            }
            else
            {
                spriteSpeed = 2;
                interval = 200;
            }

            if(currentKBState.IsKeyDown(Keys.Right) == true)
            {
                AnimateRight(gameTime);
                if(position.X < 780)
                {
                    position.X += spriteSpeed;
                }
            }


            if (currentKBState.IsKeyDown(Keys.Left) == true)
            {
                AnimateLeft(gameTime);
                if (position.X > 20)
                {
                    position.X -= spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Down) == true)
            {
                AnimateUp(gameTime);
                if (position.Y < 575)
                {
                    position.Y += spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Up) == true)
            {
                AnimateUp(gameTime);
                if (position.X > 25)
                {
                    position.X -= spriteSpeed;
                }
            }
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        public void AnimateRight(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 9;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if(currentFrame > 11)
                {
                    currentFrame = 8;
                }
                timer = 0f;
            }
        }

        public void AnimateUp(GameTime gameTime)
        {
            if(currentKBState != previousKBState)
            {
                currentFrame = 13;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;

                if(currentFrame > 15)
                {
                    currentFrame = 12;
                }
                timer = 0f;
            }
        }

        public void AnimateDown(GameTime gameTime)
        {
            if (currentKBState != previousKBState)

           {
               currentFrame = 1;
            }

 

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

 
            if (timer > interval)
            {
                currentFrame++;
 
               if (currentFrame > 3)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }
 
        public void AnimateLeft(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 5;
            }
 
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
 
                if (currentFrame > 7)
                {
                    currentFrame = 4;
                }
                timer = 0f;
            }
        }

        

        #endregion
    }
}

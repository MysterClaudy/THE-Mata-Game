using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Apocalyptic_Sunrise
{
    public class Player : AnimatedSprite
    {
        bool attacking = false;
        float mySpeed = 200;
        public Level level;
        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("playerSheet");

        }

        public Level Level
        {
            get { return level; }
        }
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;
        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;
        private float previousBottom;
        public Player(Vector2 position) : base(position)
        {
            FramesPerSecond = 10;

            //Adds all the players animations
            AddAnimation(12, 0, 0, "Down", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 0, 0, "IdleDown", 50, 50, new Vector2(0, 0));
            AddAnimation(12, 50, 0, "Up", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 50, 0, "IdleUp", 50, 50, new Vector2(0, 0));
            AddAnimation(8, 100, 0, "Left", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 100, 0, "IdleLeft", 50, 50, new Vector2(0, 0));
            AddAnimation(8, 100, 8, "Right", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 100, 8, "IdleRight", 50, 50, new Vector2(0, 0));
            AddAnimation(9, 150, 0, "AttackDown", 70, 80, new Vector2(0, 0));
            AddAnimation(9, 230, 0, "AttackUp", 70, 80, new Vector2(-13, -27));
            AddAnimation(9, 310, 0, "AttackLeft", 70, 70, new Vector2(-30, -5));
            AddAnimation(9, 380, 0, "AttackRight", 70, 70, new Vector2(+15, -5));
            //Plays our start animation
            PlayAnimation("IdleDown");
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;
            HandleCollision();
            HandleInput(Keyboard.GetState());
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            sDirection *= mySpeed;
            sPosition += (sDirection * deltaTime);

            if(sPosition.X <= 0)
            {
                sPosition.X = 0;
            }
            if(sPosition.Y <= 0)
            {
                sPosition.Y = 0;
            }
            //if(sPostion.X >= 100)
            //{
            //    sPostion.X = 100;
            //}
            //if(sPostion.Y >= 100)
            //{
            //    sPostion.Y = 100;   
            //}
            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyState)
        {
            if (!attacking)
            {
                mySpeed = 100;
                if (keyState.IsKeyDown(Keys.W))
                {
                    //Move char Up
                    sDirection += new Vector2(0, -1);
                    PlayAnimation("Up");
                    currentDir = myDirection.up;

                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    //Move char Left
                    sDirection += new Vector2(-1, 0);
                    PlayAnimation("Left");
                    currentDir = myDirection.left;

                }
                if (keyState.IsKeyDown(Keys.S))
                {
                    //Move char Down
                    sDirection += new Vector2(0, 1);
                    PlayAnimation("Down");
                    currentDir = myDirection.down;

                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    //Move char Right
                    sDirection += new Vector2(1, 0);
                    PlayAnimation("Right");
                    currentDir = myDirection.right;

                }
                if(keyState.IsKeyDown(Keys.LeftShift))
                {
                    mySpeed = 200;
                    FramesPerSecond = 20;
                    if (keyState.IsKeyDown(Keys.W))
                    {
                        //Move char Up
                        sDirection += new Vector2(0, -1);
                        PlayAnimation("Up");
                        currentDir = myDirection.up;

                    }
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        //Move char Left
                        sDirection += new Vector2(-1, 0);
                        PlayAnimation("Left");
                        currentDir = myDirection.left;

                    }
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        //Move char Down
                        sDirection += new Vector2(0, 1);
                        PlayAnimation("Down");
                        currentDir = myDirection.down;

                    }
                    if (keyState.IsKeyDown(Keys.D))
                    {
                        //Move char Right
                        sDirection += new Vector2(1, 0);
                        PlayAnimation("Right");
                        currentDir = myDirection.right;

                    }
                }
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                if (currentAnimation.Contains("Down"))
                {
                    PlayAnimation("AttackDown");
                    attacking = true;
                    currentDir = myDirection.down;
                }
                if (currentAnimation.Contains("Left"))
                {
                    PlayAnimation("AttackLeft");
                    attacking = true;
                    currentDir = myDirection.left;
                }
                if (currentAnimation.Contains("Right"))
                {
                    PlayAnimation("AttackRight");
                    attacking = true;
                    currentDir = myDirection.right;
                }
                if (currentAnimation.Contains("Up"))
                {
                    PlayAnimation("AttackUp");
                    attacking = true;
                    currentDir = myDirection.up;
                }
            }
            else if (!attacking)
            {
                if (currentAnimation.Contains("Left"))
                {
                    PlayAnimation("IdleLeft");
                }
                if (currentAnimation.Contains("Right"))
                {
                    PlayAnimation("IdleRight");
                }
                if (currentAnimation.Contains("Up"))
                {
                    PlayAnimation("IdleUp");
                }
                if (currentAnimation.Contains("Down"))
                {
                    PlayAnimation("IdleDown");
                }
            }
            currentDir = myDirection.none;

        }

       

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                attacking = false;
            }
        }
        private Rectangle localBounds;
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(sPosition.X -  m_origin.X) + localBounds.X;
                int top = (int)Math.Round(sPosition.Y - m_origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }
       
        public void HandleCollision()
        {
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {


                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == TileCollision.Impassable || IsOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    sPosition = new Vector2(sPosition.X, sPosition.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                sPosition = new Vector2(sPosition.X + depth.X, sPosition.Y);

                                // Perform further collisions with the new bounds.
                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }

           
        }
    }
}

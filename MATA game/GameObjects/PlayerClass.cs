using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MATA_game
{
    class PlayerClass : AnimatedSprite
    {
        #region Properties
        public Game1 Game;
        public Level level;
        #endregion

        #region Collectors

        public Level Level
        {
            get { return level; }
        }

        public PlayerClass(Game1 game, Vector2 position)
            : base(game, position)
        {
            
            framesPerSecond = 2;

            AddAnimation(3, 170, 1, "Left", 71, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 0, "LeftIdle", 71, 80, new Vector2(0, 0));
            AddAnimation(3, 48, 1, "Right", 32, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 1, "RightIdle", 32, 48, new Vector2(0, 0));
            AddAnimation(3, 48, 1, "Up", 32, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 1, "UpIdle", 32, 48, new Vector2(0, 0));
            AddAnimation(3, 48, 1, "Down", 32, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 1, "Idle", 32, 48, new Vector2(0, 0));

            PlayAnimation("Idle");
        }

        void Initialize()
        {
            maxLimit = new Vector2(Game.CurrentScreenSize.X + (m_size.X / 2), Game.CurrentScreenSize.Y + (m_size.Y / 2));
            minLimit = new Vector2(0 - (m_size.X / 2), 0 - (m_size.Y / 2));
        }
        
        public override void Update(GameTime gameTime)
        {
            HandleCollision();
            sDirection = Vector2.Zero;
            m_position += m_velocity;
            if (m_texture == null)
            {
                
            }

            if (m_position.Y <= 0)
            {
                m_position.Y = 0;
            }
            if (m_position.X <= 0)
            {
                m_position.X = 0;
            }
        }

        public void GetInput(GameTime gameTime)
        {
            m_velocity = new Vector2(0);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 0);
                PlayAnimation("Left");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 0);
                PlayAnimation("Right");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_velocity = new Vector2(0, -5);
                PlayAnimation("Up");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                m_velocity = new Vector2(0, 5);
                PlayAnimation("Down");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, -5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, -5);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 5);
            }
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                
            }
        }
        private Rectangle localBounds;

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(sPosition.X - m_origin.X) + localBounds.X;
                int top = (int)Math.Round(sPosition.Y - m_origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        private void HandleCollision()
        {
            Rectangle bounds = BoundingRectangle;

            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            for(int y = topTile; y <= bottomTile; y++)
            {
                for(int x = leftTile; x <= rightTile; x++)
                {
                    TileCollision collision = level.GetCollision(x, y);

                    if(collision != TileCollision.Passable)
                    {
                        Rectangle tileBounds = level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if(depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);
                            
                            if(collision == TileCollision.Impassable)
                            {
                                sPosition = new Vector2(sPosition.X + depth.X, sPosition.Y);
                                bounds = BoundingRectangle;
                            }

                        }
                    }
                }
            }
            
        }
        #endregion
    }
}
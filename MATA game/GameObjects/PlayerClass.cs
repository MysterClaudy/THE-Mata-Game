using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MATA_game
{
    class PlayerClass : MobileObject
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

        public PlayerClass(Game1 game, Texture2D texture = null
            , Vector2 position = new Vector2()
            , Vector2 size = new Vector2()
            , float rotation = 0f
            , float scale = 1f,
            float speed = 3.0f)
            : base(game,
                  texture
                  , position
                  , size
                  , rotation
                  , scale
                  , speed)
        {
            m_size = size;
            position = m_position;
        }

        void Initialize()
        {
           // maxLimit = new Vector2(Game.CurrentScreenSize.X + (m_size.X / 2), Game.CurrentScreenSize.Y + (m_size.Y / 2));
            //minLimit = new Vector2(0 - (m_size.X / 2), 0 - (m_size.Y / 2));
        }
        
        public override void Update(GameTime gameTime)
        {
            HandleCollision();  
           
            m_position += m_velocity;
            if (m_texture == null)
            {
                
            }

            //if (m_position.Y <= 0)
            //{
            //    m_position.Y = 0;
            //}
            //if (m_position.X <= 0)
            //{
            //    m_position.X = 0;
            //}
        }

        public void GetInput(GameTime gameTime)
        {
            m_velocity = new Vector2(0);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_velocity = new Vector2(0, -5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                m_velocity = new Vector2(0, 5);
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

        private Rectangle localBounds;

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(m_position.X - m_origin.X) + localBounds.X;
                int top = (int)Math.Round(m_position.Y - m_origin.Y) + localBounds.Y;

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
                                m_position = new Vector2(m_position.X + depth.X, m_position.Y); 
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
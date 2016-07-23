using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apocalyptic_Sunrise
{
    class Enemy : MobileObject
    {
        Game1 game = null;
            

        float pause = 0;
        bool moveRight = true;

        public Vector2 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Enemy(Game1 game)
        {
            this.game = game;
            m_velocity = Vector2.Zero;
        }

        public void LoadContent(ContentManager Content)
        {
            m_texture = Content.Load<Texture2D>("DroneSprite");
        }
            
        public void Update(float deltaTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_texture, Position, Color.White);
        }
    }
}

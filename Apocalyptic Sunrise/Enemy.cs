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
    public class Enemy : MobileObject
    {
        public bool isdead;
        public Vector2 startposition = new Vector2();
        public Enemy(

            Texture2D texture = null,
            Vector2 position = new Vector2(),
            Vector2 size = new Vector2(),
            float rotation = 0f,
            float scale = 1f,
            float speed = 5.0f)
            : base(
                  texture,
                  position,
                  size,
                  rotation,
                  scale)
        {
            m_velocity = new Vector2(1, 0);
            m_speed = speed;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D pTexture = null)
        {
            Texture2D texture = pTexture;
            if (texture == null)
            {
                texture = m_texture;
            }
            if (texture == null) return;


            Rectangle srcRect = new Rectangle(
                                    0,
                                    0,
                                    (int)(m_size.X),
                                    (int)(m_size.Y));

            SpriteEffects effects = SpriteEffects.None;
            if (m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
            if (m_flipVertical) effects = effects | SpriteEffects.FlipHorizontally;


            spriteBatch.Draw(
                texture,
                m_position,
                srcRect,
                Color.White,
                m_rotation,
                m_origin,
                m_scale,
                effects,
                0);
        }
    }
}

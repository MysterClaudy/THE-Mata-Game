using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MATA_game
{
    class GameObject
    {
        #region properties
        public Texture2D m_texture;
        public Vector2 m_position;
        public Vector2 m_size;
        public Vector2 m_origin;

        public float m_rotation;
        public float m_scale;

        public bool m_flipHorosontal;
        public bool m_flipVertical;
        #endregion

        #region collectors
        public GameObject(Texture2D texture = null, Vector2 position = new Vector2(), Vector2 size = new Vector2(), float rotation = 0f, float scale = 1f)
        {
            m_texture = texture;
            m_position = position;
            m_size = size;
            m_origin = new Vector2(
                (int)m_size.X / 2,
                (int)m_size.Y / 2);

            m_rotation = rotation;
            m_scale = scale;
            m_flipHorosontal = false;
            m_flipVertical = false;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D pTexture = null)
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
        #endregion
    }

}

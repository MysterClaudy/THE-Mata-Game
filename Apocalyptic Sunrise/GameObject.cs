using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apocalyptic_Sunrise
{
    public class GameObject
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

        public AABB m_aabb;
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
            UpdateBounds();
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

        protected virtual void UpdateBounds()
        {
            m_aabb = new AABB(m_position, m_size * m_scale);
        }
        protected bool AABBCollisionCheck(GameObject pOther)
        {
            return m_aabb.CollisionCheck(pOther.m_aabb);
        }

        protected bool CircleCollisionCheck(Vector2 object1Position
                                          , float object1Radius
                                          , Vector2 object2Position
                                          , float object2Radius)
        {
            float distanceBetweenObjects =
                (object1Position - object2Position).Length();
            float sumOfRadii = object1Radius + object2Radius;

            if (distanceBetweenObjects < sumOfRadii)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}

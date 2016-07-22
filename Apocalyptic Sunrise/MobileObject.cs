using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apocalyptic_Sunrise
{
    public class MobileObject : GameObject
    {
        public float m_speed;
        public Vector2 m_velocity;
        public Vector2 minLimit;
        public Vector2 maxLimit;
        public TimeSpan LastShot = new TimeSpan(0, 0, 0, 0, 0);
        public TimeSpan ShotCoolDown = new TimeSpan(0, 0, 0, 0, 100);

        public MobileObject(
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
            m_velocity = new Vector2();
            m_speed = speed;
        }

        public virtual void Update(GameTime gameTime)
        {
            m_position.Y += m_velocity.Y * m_speed;
            m_position.X += m_velocity.X * m_speed;
            UpdateBounds();
        }
    }
}

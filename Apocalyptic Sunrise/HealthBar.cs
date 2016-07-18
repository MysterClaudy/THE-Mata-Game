using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apocalyptic_Sunrise
{
    class HealthBar
    {
        #region properties

        public Texture2D healthTexture;
        public Vector2 healthPosition;
        public Texture2D healthBar;
        public Texture2D changingHealth;
        public int maxHealth = 200;
        Vector2 healthScale = new Vector2(1f, 1f);
        public int currentHealth = 100;
        public SpriteFont font;
        #endregion
        public HealthBar(ContentManager content)
        {
            LoadContent(content);
            healthPosition = new Vector2(980, 0);
            //  maxHealth = healthTexture.Width;
        }

        public void LoadContent(ContentManager content)
        {
            healthTexture = content.Load<Texture2D>("Health");
            healthBar = content.Load<Texture2D>("HealthBar");
            changingHealth = content.Load<Texture2D>("HealthChanging");
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                currentHealth += 1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                currentHealth -= 1;
            }

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(healthTexture
                , null
                , new Rectangle((int)healthPosition.X, (int)healthPosition.Y, healthTexture.Width, healthTexture.Height)
                , null//new Rectangle((int)healthPosition.X, (int)healthPosition.Y, 1200, 400)
                , new Vector2(0, 0)
                , 0
                , healthScale
                , Color.White
                , SpriteEffects.None
                , 0);

            /*spriteBatch.Draw(changingHealth
                , null
                , new Rectangle((int)healthPosition.X + 10, (int)healthPosition.Y, -currentHealth, healthTexture.Height)
                , null//new Rectangle((int)healthPosition.X, (int)healthPosition.Y, 1200, 400)
                , new Vector2(0, 0)
                , 0
                    , healthScale
                    , Color.White
                , SpriteEffects.None
                , 0);*/

            spriteBatch.Draw(changingHealth
                , new Rectangle((int)healthPosition.X + 10, (int)healthPosition.Y, -(currentHealth - 200), changingHealth.Height)
                , new Rectangle(0, 0, -(currentHealth - 200), changingHealth.Height)

                , Color.White);

            spriteBatch.Draw(healthBar
                , null
                , new Rectangle((int)healthPosition.X, (int)healthPosition.Y, 300, 100)
                , null//new Rectangle((int)healthPosition.X, (int)healthPosition.Y, 1200, 400)
                , new Vector2(0, 0)
                , 0
                , healthScale
                , Color.White
                , SpriteEffects.None
                , 0);
            //   spriteBatch.DrawString(font, "health:" + (currentHealth), new Vector2( 300, 300), Color.White);


            //   spriteBatch.Draw(healthBar, healthPosition, Color.White);
            //  spriteBatch.Draw(healthTexture, healthPosition, Color.White);

        }
    }
}

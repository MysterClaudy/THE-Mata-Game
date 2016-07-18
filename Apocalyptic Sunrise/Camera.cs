using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apocalyptic_Sunrise
{
    public class Camera
    {
        #region Properties
        public Matrix viewMatrix;
        Vector2 position;
        public GraphicsDeviceManager graphics;
       // public HealthBar healthBar;
        #endregion


        #region Collectors
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public int ScreenWidth
        {
            get { return graphics.PreferredBackBufferWidth; }
        }

        public int ScreenHeight
        {
            get { return graphics.PreferredBackBufferHeight; }
        }

        public void Update(Vector2 playerPosition)
        {
            position.X = playerPosition.X - (ScreenWidth / 2);
            position.Y = playerPosition.Y - (ScreenHeight / 2);




            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;

            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
        #endregion
    }
}

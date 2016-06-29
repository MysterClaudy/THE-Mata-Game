using Microsoft.Xna.Framework;

namespace MATA_game
{
    public class Camera
    {
        #region Properties
        public Matrix viewMatrix;
        Vector2 position;
        public HealthBar healthBar;
        #endregion

        #region Collectors
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public int ScreenWidth
        {
            get { return GraphicsDeviceManager.DefaultBackBufferWidth; }
        }

        public int ScreenHeight
        {
            get { return GraphicsDeviceManager.DefaultBackBufferHeight; }
        }

        public void Update(Vector2 playerPosition, Vector2 healthBarPos)
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

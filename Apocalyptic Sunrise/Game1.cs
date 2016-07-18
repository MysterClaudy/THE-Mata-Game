using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Apocalyptic_Sunrise
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player player;
        public Camera camera;
        GameStates gameStates;
        Level level;
        HealthBar healthBar;
        public static Game1 theGame;
        private int levelIndex = -1;
        private const int numberOfLevels = 2;
        Vector2 spawningPosition;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            theGame = this;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            gameStates = new GameStates();
            LoadNextLevel();
            player = new Player(new Vector2(100,100));
            camera = new Camera();
            healthBar = new HealthBar(Content);
            base.Initialize();
        }

        public void LoadNextLevel()
        {
            levelIndex = (levelIndex + 1);//  % 1;
            bool flag = level != null;
            if (flag)
            {
                level.Dispose();
            }
            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
            {
                this.level = new Level(base.Services, fileStream, levelIndex);
            }
            if (levelIndex == 0)
            {
                spawningPosition = new Vector2(700, 300);
            }
            if (levelIndex == 1)
            {
                player.m_position = new Vector2(100, 100);
            }
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameStates.LoadContent(Content);
            player.LoadContent(Content);
            level.player = player; 
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            gameStates.player = player;
            player.level = level;
            gameStates.level = level;
            level.graphics = graphics;
            camera.graphics = graphics;
            camera.Update(player.sPosition);
            gameStates.healthBar = healthBar;   
            gameStates.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, new Matrix?(this.camera.viewMatrix));
            gameStates.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            if(gameStates.isGame)
            { 
                healthBar.Draw(spriteBatch);
            }
            gameStates.Draw2(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
    
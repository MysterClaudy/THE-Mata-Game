using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrebleSketchGameUtils;

namespace Apocalyptic_Sunrise
{
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
        public DevLogging Debug;

        int levelIndex = -1;
        const int numberOfLevels = 2;
        Vector2 spawningPosition;

        string GameVersionBuild;

        public Game1()
        {
            theGame = this;
            Debug = new DevLogging();
            File.Delete(Debug.GetCurrentDirectory());
            Debug.WriteToFile("This game proudly uses the TrebleSketch Utilities Debugger v6.2", true, false);
            DateTime thisDay = DateTime.Now;
            GameVersionBuild = "v0.2.2.67 ";
            Debug.WriteToFile("Starting The Apocalyptic Sunrise " + GameVersionBuild + thisDay.ToString("dd-MM-yyyy HH:mm:ss zzz"), true, false);

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Debug.WriteToFile("Started Initializing Game", true, false);

            IsMouseVisible = true;

            gameStates = new GameStates();
            gameStates.Game = theGame;
            player = new Player(new Vector2(100, 100));
            camera = new Camera();
            healthBar = new HealthBar(Content);
            base.Initialize();

            Debug.WriteToFile("Finished Initializing Game", true, false);
        }

        public void LoadNextLevel()
        {
            levelIndex = (levelIndex + 1);//  % 1;

            Debug.WriteToFile("Started loading level", true, false);

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

            level.player = player;

            Debug.WriteToFile("Finishied loading level " + levelIndex, true, false);
        }

        protected override void LoadContent()
        {
            Debug.WriteToFile("Started Loading Game Textures", true, false);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameStates.LoadContent(Content);
            player.LoadContent(Content);

            Debug.WriteToFile("Finished Loading Game Textures", true, false);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Debug.WriteToFile("Ending Game...", true, false);
                Exit();
            }

            gameStates.player = player;
            if (level != null)
            {
                player.level = level;
                gameStates.level = level;
                level.graphics = graphics;
            }
            camera.graphics = graphics;
            camera.Update(player.sPosition);
            gameStates.healthBar = healthBar;   
            gameStates.Update(gameTime);

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
    
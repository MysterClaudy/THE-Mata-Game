 using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Threading;
using System;

namespace MATA_game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameStates gameStates;
        PlayerClass player;
        Texture2D background;
        Camera camera;
        HealthBar healthBar;
        Level level;
        private int levelIndex = -1;
        private const int numberOfLevels = 2;
        Vector2 spawningPosition;
        KeyboardState oldKeyState;
        bool isloadingLevel = false;
        Texture2D blackScreen;
        SpriteFont textFont;

        private const float delay = 5;
        private float remainingdelay = delay;
        private const float delay2 = 1;
        private float remainingdelay2 = delay2;
        private const float delay3 = 2;     
        private float remainingdelay3 = delay3;

        public Game1()
        {
            graphics =   new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            camera = new Camera();
        }

        
        protected override void Initialize()
        {
            IsMouseVisible = true;
            // TODO: Add your initialization logic here
            healthBar = new HealthBar(Content);
            gameStates = new GameStates();

            LoadNextLevel();

            InitializePlayer();
            
            base.Initialize();
            
        }

        private void LoadNextLevel()
        {            
            levelIndex = (levelIndex + 1); //% numberOfLevels;
            if (level != null)
                level.Dispose();

            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(Services, fileStream, levelIndex);

            if (levelIndex == 0)
            {
                spawningPosition = new Vector2(700,300);
            }
            if(levelIndex == 1)
            {
                player.m_position = new Vector2(100, 100);
            }
        }

        public void InitializePlayer()
        {
            player = new PlayerClass(null, spawningPosition, new Vector2(48, 64), 0, 1.5f, 3);
            player.maxLimit = new Vector2(graphics.PreferredBackBufferWidth + (player.m_size.X / 2), graphics.PreferredBackBufferHeight + (player.m_size.Y / 2));
            player.minLimit = new Vector2(0 - (player.m_size.X / 2), 0 - (player.m_size.Y / 2));
        }

        

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameStates.font = Content.Load<SpriteFont>("Font/scoreFont");
            gameStates.startButtonTexture = Content.Load<Texture2D>("Buttons/start");
            gameStates.exitButtonTexture = Content.Load<Texture2D>("Buttons/exit");
            gameStates.resumeButton = Content.Load<Texture2D>("Buttons/resume");
            healthBar.font = Content.Load<SpriteFont>("Font/scoreFont");
            background = Content.Load<Texture2D>("Background/background");
            player.m_texture = Content.Load<Texture2D>("Player_still");
            blackScreen = Content.Load<Texture2D>("Black screen");
            textFont = Content.Load<SpriteFont>("Font/scoreFont");
            
            // TODO: use this.Content to load your game content here
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void info(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (levelIndex == 1)
            {
                var timer2 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay2 -= timer2;
                if (remainingdelay2 <= 0)
                {
                    spriteBatch.DrawString(textFont, "time: 10:45pm", new Vector2(graphics.PreferredBackBufferWidth - 300, graphics.PreferredBackBufferHeight - 100), Color.White);
                }
                var timer3 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay3 -= timer3;
                if (remainingdelay3 <= 0)
                {
                    spriteBatch.DrawString(textFont, "location: Bermuda Triangle", new Vector2(graphics.PreferredBackBufferWidth - 300, graphics.PreferredBackBufferHeight - 80), Color.White);
                }
            }
        }

        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            level.player = player;
            gameStates.healthBar = healthBar;
            gameStates.graphics = graphics;
            gameStates.player = player;
            gameStates.level = level;
            camera.healthBar = healthBar;
            camera.Update(player.m_position, healthBar.healthPosition);
           
            gameStates.camera = camera;
            gameStates.Update(gameTime);
            
            healthBar.Update();

            KeyboardState newKeyState = Keyboard.GetState();
            if(newKeyState.IsKeyDown(Keys.Home) && oldKeyState.IsKeyUp(Keys.Home))
            {
                LoadNextLevel();
                isloadingLevel = true;
            }
            oldKeyState = newKeyState;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin( SpriteSortMode.Deferred,BlendState.AlphaBlend, null,null, null,null, camera.viewMatrix);
            //  spriteBatch.Draw(background, Vector2.Zero, Color.White);
            if(gameStates.isGame == true)
            {
                player.Draw(gameTime, spriteBatch, player.m_texture);
                level.Draw(gameTime, spriteBatch);
            }
           
            
            spriteBatch.End();
            
            spriteBatch.Begin();
            if(gameStates.isGame == true)
            {
                healthBar.Draw(spriteBatch);
            }

            gameStates.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewMatrix);
            if (isloadingLevel == true)
            {
                spriteBatch.Draw(blackScreen, new Vector2(player.m_position.X - 700, player.m_position.Y - 700), Color.White);
                info(spriteBatch, gameTime);

                var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay -= timer;
                if(remainingdelay <= 0)
                {
                    isloadingLevel = false;                    
                    remainingdelay = delay;
                }
                
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

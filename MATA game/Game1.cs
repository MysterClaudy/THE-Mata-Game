using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using TrebleGameUtils;
using AIEResources.Particles;

namespace MATA_game
{
    /// <summary>
    /// Name: 
    /// Description: 
    /// Version: 0.1.7.55 (Developmental Stages)
    /// Genre: 2D Platformer
    /// Developer: Rohan Renu (Myster-Claude), Tony Lu (CroakyEngine), and Titus Huang (Treble Sketch/ILM126)
    /// Game Engine: MonoGame/XNA
    /// Language: C#
    /// Dev Notes: Our third game and the first collaborative game that either of us had ever made
    /// </summary>
    public class Game1 : Game
    {
        #region Begin
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public DevLogging Debug;

        GameObject gameObject;
        GameStates gameStates;
        PlayerClass player;
        Camera camera;
        HealthBar healthBar;
        Level level;
        Emitter emitter;

        Texture2D background;
        private int levelIndex = -1;
        private const int numberOfLevels = 2;
        Vector2 spawningPosition;
        KeyboardState oldKeyState;
        bool isloadingLevel = false;
        Texture2D blackScreen;
        public Texture2D burstTexture;
        const int numEmitterSettings = 3;
        int emitterSettingsID = 0;

        string GameVersionBuild;
        public Vector2 CentreScreen;
        public Vector2 CurrentScreenSize;

        private const float delay = 5;
        private float remainingdelay = delay;
        private const float delay2 = 1;
        private float remainingdelay2 = delay2;
        private const float delay3 = 2;     
        private float remainingdelay3 = delay3;

        int res_OriginalGameHeight;
        int res_OriginalGameWidth;
        int res_ScreenHeight;
        int res_ScreenWidth;
        float res_ScreenScaleUpDifference;
        float res_ScreenScaleDownDifference;
        bool FullScreen;
        public static Game1 theGame;
        #endregion

        public Game1()
        {
            theGame = this;
            Debug = new DevLogging();
            File.Delete(Debug.GetCurrentDirectory());
            DateTime thisDay = DateTime.Now;
            GameVersionBuild = "v0.1.7.55 ";
            Debug.WriteToFile("Starting The MATA Game " + GameVersionBuild + thisDay.ToString("dd-MM-yyyy HH:mm:ss zzz"), true, false);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            camera = new Camera();

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            CentreScreen = new Vector2(graphics.PreferredBackBufferWidth / 2,
                                            graphics.PreferredBackBufferHeight / 2);
            
            CurrentScreenSize = new Vector2(graphics.PreferredBackBufferWidth,
                                                graphics.PreferredBackBufferHeight);

            res_OriginalGameHeight = graphics.PreferredBackBufferHeight;
            res_OriginalGameWidth = graphics.PreferredBackBufferWidth;
            res_ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            res_ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            Debug.WriteToFile("Screen size: " + res_ScreenWidth + " x " + res_ScreenHeight, true, false);
            Debug.WriteToFile("Game screen size: " + res_OriginalGameWidth + " x " + res_OriginalGameHeight, true, false);

            #region Future - Fullscreen
            if (FullScreen)
            {
                res_ScreenScaleUpDifference = (float)res_ScreenHeight / (float)res_OriginalGameHeight;
                res_ScreenScaleDownDifference = (float)res_ScreenHeight / (float)res_OriginalGameHeight;
                Debug.WriteToFile("Scale up from OriginalGameHeight to ScreenHeight: " + res_ScreenScaleUpDifference, true, false);
            }
            #endregion
        }

        protected override void Initialize()
        {
            Debug.WriteToFile("Started Initializing Game", true, false);

            InitializeClasses();
            InitilizeProperties();

            IsMouseVisible = true;

            gameStates.Game = this;
            
            base.Initialize();
            Debug.WriteToFile("Finished Initializing Game", true, false);
        }

        void InitializeClasses()
        {
            healthBar = new HealthBar(Content);
            gameStates = new GameStates();
            //player = new PlayerClass(this, spawningPosition);
            player = new PlayerClass(null, null, spawningPosition, new Vector2(318, 513), 0, 0.15f, 3);
            gameObject = new GameObject(this);
        }
        
        void InitilizeProperties()  
        {
            gameStates.Game = this; 
            gameObject.Debug = Debug;
            gameObject.Game = this;
            player.Game = this;
        }

        public void LoadNextLevel()
        {
            levelIndex++; //% numberOfLevels;
            if (level != null)
                level.Dispose();

            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
            {
                level = new Level(Services, fileStream, levelIndex);
                Debug.WriteToFile("Loading Map...", true, false);
            }

            if (levelIndex == 0)
            {
                spawningPosition = new Vector2(700, 300);
            }
            if (levelIndex == 1)
            {
                player.m_position = new Vector2(100, 100);
            }
            Debug.WriteToFile("Loading Level: " + levelIndex, true, false);
        }

        protected override void LoadContent()
        {
            Debug.WriteToFile("Started Loading Game Textures", true, false);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameStates.font = Content.Load<SpriteFont>("Font/scoreFont");
            gameStates.startButtonTexture = Content.Load<Texture2D>("Buttons/start");
            gameStates.exitButtonTexture = Content.Load<Texture2D>("Buttons/exit");
            gameStates.resumeButton = Content.Load<Texture2D>("Buttons/resume");
            healthBar.font = Content.Load<SpriteFont>("Font/scoreFont");
            background = Content.Load<Texture2D>("Background/background");
            player.m_texture = Content.Load<Texture2D>("Player");
           // player.sTexture = Content.Load<Texture2D>("xeonsheet");
            blackScreen = Content.Load<Texture2D>("Black screen");
            Debug.textFont = Content.Load<SpriteFont>("Font/scoreFont");

            burstTexture = Content.Load<Texture2D>("Particles/spark");
            //emitter = Emitter.CreateBurstEmitter(burstTexture, new Vector2(0, 0));

            Debug.WriteToFile("Finished Loading Game Textures", true, false);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void Info(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (levelIndex == 1)
            {
                var timer2 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay2 -= timer2;
                if (remainingdelay2 <= 0)
                {
                    spriteBatch.DrawString(Debug.textFont, "time: 10:45pm", new Vector2(graphics.PreferredBackBufferWidth - 300, graphics.PreferredBackBufferHeight - 100), Color.White);
                }
                var timer3 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay3 -= timer3;
                if (remainingdelay3 <= 0)
                {
                    spriteBatch.DrawString(Debug.textFont, "location: Bermuda Triangle", new Vector2(graphics.PreferredBackBufferWidth - 300, graphics.PreferredBackBufferHeight - 80), Color.White);
                }
            }
        }

        void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Debug.WriteToFile("Ending Game...", true, false);
                Exit();
            }

            if (level != null)
            {
                level.player = player;

                healthBar.Update();
            }
            player.level = level;
            gameStates.level = level;
            gameStates.healthBar = healthBar;
            gameStates.graphics = graphics;
            gameStates.player = player;
            
            camera.healthBar = healthBar;
            camera.Update(player.m_position, healthBar.healthPosition);
            
            gameStates.camera = camera;
            gameStates.Update(gameTime);

            KeyboardState newKeyState = Keyboard.GetState();
            if(newKeyState.IsKeyDown(Keys.Home) && oldKeyState.IsKeyUp(Keys.Home))
            {
                LoadNextLevel();
                isloadingLevel = true;
                Debug.WriteToFile("Level is changing to: " + levelIndex, true, false);
            }
            oldKeyState = newKeyState;

            //Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //emitter.position = mousePos;
            //emitter.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin( SpriteSortMode.Deferred,BlendState.AlphaBlend, null,null, null,null, camera.viewMatrix);
            //  spriteBatch.Draw(background, Vector2.Zero, Color.White);

            //if (level != null)
            //{
            //    Rectangle tempRect = new Rectangle((int)player.m_position.X, (int)player.m_position.Y, 100, 100);
            //    Debug.WriteToFile("player Position: " + player.m_position.ToString(), false, false);
            //    DrawRectangle(tempRect, Color.Black);
            //}

            if (gameStates.isGame == true && level != null)
            {
                level.Draw(gameTime, spriteBatch);
               // player.Draw(gameTime, spriteBatch, player.m_texture);
            }
            //emitter.Draw(spriteBatch);
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
                Info(spriteBatch, gameTime);

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

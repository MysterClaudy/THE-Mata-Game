using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using TrebleGameUtils;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Apocalyptic_Sunrise
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Game1 theGame;
        public DevLogging Debug;
        public Player player;
        public Camera camera;
        GameStates gameStates;
        Level level;
        HealthBar healthBar;

        Texture2D thing;
        Video video;
        VideoPlayer vidplayer;
        Texture2D videoTexture;

        string GameVersionBuild;


        public Game1()
        {
            Debug = new DevLogging();
            File.Delete(Debug.GetCurrentDirectory());
            Debug.WriteToFile("This game proudly uses the TrebleSketch Utilities Debugger v6.2", true, false);
            GameVersionBuild = "v0.3.3.81 ";
            DateTime thisDay = DateTime.Now;
            Debug.WriteToFile("Starting Apocalyptic Sunrise " + GameVersionBuild + thisDay.ToString("dd-MM-yyyy HH:mm:ss zzz"), true, false);

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content"; 
            
            theGame = this;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Debug.WriteToFile("Started Initializing Game", true, false);

            gameStates = new GameStates();
            level = new Level();
            level.LoadNextMap(Content);  
            player = new Player(level.playerSpawningPosition);
            camera = new Camera();
            healthBar = new HealthBar(Content);
            //map = new TiledMap(GraphicsDevice, 110, 110, 32, 32, TiledMapOrientation.Orthogonal);
            base.Initialize();

            VideoPlayer vidPlayer = new VideoPlayer();

            Debug.WriteToFile("Finished Initializing Game", true, false);
        }

        protected override void LoadContent()
        {
            Debug.WriteToFile("Started Loading Game Textures", true, false);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameStates.LoadContent(Content);
            player.LoadContent(Content);

            thing = Content.Load<Texture2D>("start");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            video = Content.Load<Video>("MenuBackground");
            vidplayer = new VideoPlayer();

            vidplayer.Play(video);

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
            player.gameStates = gameStates;
            level.player = player;
            player.level = level;
            gameStates.level = level;
            
            camera.graphics = graphics;
            camera.Update(player.sPosition);
            gameStates.healthBar = healthBar;   
            gameStates.Update(gameTime);

            if(Keyboard.GetState().IsKeyDown(Keys.End))
            {
                level.LoadNextMap(Content);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);           

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, new Matrix?(this.camera.viewMatrix));

            if (gameStates.isInMenu == true && gameStates.isGame != true)
            {
                // Only call GetTexture if a video is playing or paused
                if (vidplayer.State != MediaState.Stopped)
                    videoTexture = vidplayer.GetTexture();

                // Drawing to the rectangle will stretch the 
                // video to fill the screen
                Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                    GraphicsDevice.Viewport.Y,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height);

                // Draw the video, if we have a texture to draw.
                if (videoTexture != null)
                {
                    spriteBatch.Draw(videoTexture, screen, Color.White);
                }
            }

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
    
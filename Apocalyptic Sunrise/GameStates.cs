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
    class GameStates
    {
        #region Properties
        public enum GameState
        {
            MainMenu,
            Game,
            PauseMenu,
            EndGame,
            Options
        }

        public Game1 Game;

        GameState gameState;
        public SpriteFont font;
        
        public MouseState mouseState;
        public MouseState previousMouseState;
        public GraphicsDeviceManager graphics;
        public Camera camera;
        public HealthBar healthBar;
        public Level level;
        public Game1 game;
        public Player player;
        public bool isGame = false;

        #region Player UI
        float healthScale = 0.25f;
        #endregion

        #region Buttons
        public Texture2D startButtonTexture;
        public Texture2D exitButtonTexture;
        public Texture2D pauseButton;
        public Texture2D resumeButton;
        
        public Vector2 startButtonPosition = new Vector2(400, 300);
        public Vector2 exitButtonPosition = new Vector2(400, 350);
        public Vector2 resumeButtonPosition = new Vector2(360, 250);
        public Vector2 pauseButtonPostion = new Vector2(0, 0);
        #endregion
        #endregion

        #region Collectors

        #region Content
        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("scoreFont");
            startButtonTexture = content.Load<Texture2D>("start");
            exitButtonTexture = content.Load<Texture2D>("exit");
            resumeButton = content.Load<Texture2D>("resume");
           // pauseButton = content.Load<Texture2D>("pause");
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime)
        {
            if (gameState == GameState.MainMenu)
            {
                UpdateMenu();
            }
            if (gameState == GameState.Game)
            {
                UpdateGame(gameTime);
                isGame = true;
            }
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MousedClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;
        }

        public void UpdateMenu()
        {

        }

        public void UpdateGame(GameTime gameTime)
        {
           player.Update(gameTime);
            healthBar.Update();
        }
        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (gameState == GameState.MainMenu)
            {
                
            }

            if (gameState == GameState.Game)
            {
                level.Draw(gameTime, spriteBatch);
            }

            if (gameState == GameState.PauseMenu)
            {
            }
        }

        public void Draw2(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (gameState == GameState.MainMenu)
            {
                spriteBatch.DrawString(font, "Main Menu", new Vector2(100, 100), Color.White);
                spriteBatch.Draw(startButtonTexture, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButtonTexture, exitButtonPosition, Color.White);
            }

            if (gameState == GameState.Game)
            {
             


                spriteBatch.DrawString(font, "Game", new Vector2(100, 100), Color.White);
                /*spriteBatch.Draw(player.healthTexture,new Rectangle(1130, 50, 1200, 400), new Rectangle(1130, 50, 1200, 400), Color.White);
                GameObject HealthBar = new GameObject(player.healthBar, player.healthPosition, new Vector2(1200, 400), 0, healthScale);
                HealthBar.Draw(gameTime, spriteBatch, player.healthBar);*/
            }

            if (gameState == GameState.PauseMenu)
            {
                spriteBatch.DrawString(font, "Pause", new Vector2(100, 100), Color.White);
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
            }
        }

        public void MousedClicked(int x, int y)
        {
            Rectangle mouseClickedRect = new Rectangle(x, y, 10, 10);

            if (gameState == GameState.MainMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickedRect.Intersects(startButtonRect))
                {
                    gameState = GameState.Game;
                }

                else if (mouseClickedRect.Intersects(exitButtonRect))
                {
                    Environment.Exit(0);
                }

            }
            if (gameState == GameState.Game)
            {
                Rectangle pausebuttonRect = new Rectangle(1210, 0, 70, 70);
                if (mouseClickedRect.Intersects(pausebuttonRect))
                {
                    gameState = GameState.PauseMenu;
                }
            }
            if (gameState == GameState.PauseMenu)
            { 
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);
                if (mouseClickedRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Game;
                }
            }
        }
        #endregion
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MATA_game
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
        public PlayerClass player;
        public Camera camera;
        public HealthBar healthBar;
        public Level level;
        public Game1 game;
        public bool isGame = false;

        #region Player UI
        float healthScale = 0.25f;
        #endregion

        #region Buttons
        public Texture2D startButtonTexture;
        public Texture2D exitButtonTexture;
        public Texture2D pauseButton;
        public Texture2D resumeButton;

        public Vector2 startButtonPosition = new Vector2(350, 200);
        public Vector2 exitButtonPosition = new Vector2(350, 250);
        public Vector2 resumeButtonPosition = new Vector2(360, 250);
        public Vector2 pauseButtonPostion = new Vector2(0, 0);
        #endregion
        #endregion

        #region Collectors
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
            
            player.GetInput(gameTime);
            healthBar.Update();
        }
        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(gameState == GameState.MainMenu)
            {
                spriteBatch.DrawString(font, "Main Menu", new Vector2(100, 100), Color.White);
                spriteBatch.Draw(startButtonTexture, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButtonTexture, exitButtonPosition, Color.White);
            }

            if (gameState == GameState.Game)
            {
                isGame = true;
                
                player.Draw(spriteBatch);

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

            if(gameState == GameState.MainMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if(mouseClickedRect.Intersects(startButtonRect))
                {
                    gameState = GameState.Game;
                    Game.LoadNextLevel();
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

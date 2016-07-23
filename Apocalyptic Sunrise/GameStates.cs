using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TrebleSketchGameUtils;

namespace Apocalyptic_Sunrise
{
    public class GameStates
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
        public DevLogging Debug;
        public AudioSystems Audio;

        GameState gameState;
        public SpriteFont font;
        List<Enemy> enemies = new List<Enemy>();
        List<Vector2> SpawnPositions = new List<Vector2>();
        float spawn = 0;
        public Enemy enemy;
        public MouseState mouseState;
        public MouseState previousMouseState;
        public GraphicsDeviceManager graphics;
        public Camera camera;
        public HealthBar healthBar;
        public Level level;
        public Game1 game;
        public Player player;
        public bool isGame = false;
        public bool isVisible = true;
        public bool isInMenu = false;

        #region Player UI
        float healthScale = 0.25f;
        #endregion

        #region Buttons
        public Texture2D startButtonTexture;
        public Texture2D exitButtonTexture;
        public Texture2D pauseButton;
        public Texture2D resumeButton;
        public Texture2D enemytexture;
        
        public Vector2 startButtonPosition = new Vector2(400, 300);
        public Vector2 exitButtonPosition = new Vector2(400, 350);
        public Vector2 resumeButtonPosition = new Vector2(360, 250);
        public Vector2 pauseButtonPostion = new Vector2(0, 0);
        #endregion
        #endregion

        #region Collectors

        public void InitGame()
        {
            // Positions that Enemies will spawn in
            SpawnPositions.Add(new Vector2(384, 305));
            SpawnPositions.Add(new Vector2(639, 242));
            SpawnPositions.Add(new Vector2(894, 305));
            SpawnPositions.Add(new Vector2(320, 430));
            SpawnPositions.Add(new Vector2(500, 430));
            SpawnPositions.Add(new Vector2(575, 430));
            SpawnPositions.Add(new Vector2(700, 430));

            for (int i = 0; i < 7; i++)
            {
                CreateEnemy(SpawnPositions[i]);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.startposition = enemy.m_position;
            }
        }

        public void CreateEnemy(Vector2 position)
        {
            enemy = new Enemy(enemytexture,
                       position, new Vector2(64, 64), 0, 1f, 1);
            enemy.m_velocity.X = 1;
            enemies.Add(enemy);
        }

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
            mouseState = Mouse.GetState();

            if (gameState == GameState.MainMenu)
            {
                UpdateMenu();
                isInMenu = true;
            }
            if (gameState == GameState.Game)
            {
                UpdateGame(gameTime);
                isGame = true;
                isInMenu = false;
            }
            if (gameState == GameState.Options)
            {
                UpdateOptions();
            }

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
            level.Update(gameTime);
            healthBar.Update();
            EnemyMovement(gameTime);
        }

        public void UpdateOptions()
        {

        }
        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (gameState == GameState.MainMenu)
            {
                
            }

            if (gameState == GameState.Game && level.map != null)
            {
                level.map.Draw(spriteBatch);
                if (isVisible == true)
                {
                    player.Draw(spriteBatch);
                }

                if (level.levelIndex == 1)
                {
                    DrawEnemies(spriteBatch);
                    spriteBatch.Draw(player.elevatorTexture, player.elevatorPosition, Color.White);
                }

            }

            if (gameState == GameState.PauseMenu)
            {

            }

            if (gameState == GameState.Options)
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

            if (gameState == GameState.Options)
            {

            }
        }

        #region EnemyStuff
        public void EnemyMovement(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.m_velocity.X == 0)
                {
                    Random randNum = new Random();
                    enemy.m_velocity.X = randNum.Next(-2, 2);
                }
                if (Vector2.Distance(enemy.m_position, enemy.startposition) > 80)
                {
                    enemy.m_velocity.X *= -1;
                    enemy.m_flipHorosontal = !enemy.m_flipHorosontal;
                }

                enemy.Update(gameTime);
            }
        }

        public void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                SpriteEffects fx = SpriteEffects.None;
                if (enemy.m_flipHorosontal)
                    fx = SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(enemytexture,
                enemy.m_position,
                null,
                Color.White,
                0,
                new Vector2(enemytexture.Width / 2, enemytexture.Height / 2),
                new Vector2(enemy.m_size.X / enemytexture.Width, enemy.m_size.Y / enemytexture.Height),
                fx,
                0);
            }
        }
        #endregion

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
                    AudioSystems.StartPlayingAudio(1, 0.35f, false);
                    Debug.WriteToFile("AUDIO SHOULD BE PLAYING", true, false);
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
                    Debug.WriteToFile("Pause Menu has been loaded and game has been paused", false, false);
                }
            }

            if (gameState == GameState.PauseMenu)
            { 
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);
                if (mouseClickedRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Game;
                    Debug.WriteToFile("Pause Menu has been unloaded and game has been resumed", false, false);
                }
            }

            if (gameState == GameState.Options)
            {

            }
        }
        #endregion
    }
}

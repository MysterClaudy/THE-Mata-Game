using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoGame.Extended.Maps.Tiled;
using TrebleSketchGameUtils;

namespace Apocalyptic_Sunrise
{
    public class Level
    {
        public GraphicsDeviceManager graphics;
        public TiledMap map = null;
        public TiledTileLayer collisionLayer;
        public Player player;
        Game1 game = null;
        public SpriteFont font;
        List<Enemy> enemies = new List<Enemy>();
        public GameStates gameState;
        public Vector2 playerSpawningPosition;
        public int levelIndex = 0;

        public DevLogging Debug;

        private const int numberOfLevels = 3;

        public static int tile = 32;
        public static float meter = tile;
       
        public const float delay = 5;
        public float remainingdelay = delay;
        private const float delay2 = 1;
        private float remainingdelay2 = delay2;
        private const float delay3 = 2;
        private float remainingdelay3 = delay3;

        public int ScreenWidth
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
        }

        public void LoadNextMap(ContentManager Content)
        {
            levelIndex++;
            if (levelIndex == 1)
            {
                Debug.WriteToFile("Trying to load Level " + levelIndex, true, false);
                map = Content.Load<TiledMap>("Level1");
                playerSpawningPosition = new Vector2(96, 96);

                map = Content.Load<TiledMap>("Level1");
                Debug.WriteToFile("Level " + levelIndex + " has been loaded", true, false);

            }
            else if (levelIndex == 2)
            {
                player.isVisible = true;
<<<<<<< HEAD
                gameState.isVisible = true;
                Debug.WriteToFile("Trying to load Level " + levelIndex, true, false);
=======
                Game1.theGame.gameStates.isVisible = true;
                playerSpawningPosition = new Vector2(576,512);
>>>>>>> 43beaeaf43a5c8fc50200267010658188d2a3913
                map = Content.Load<TiledMap>("Level2");
                Debug.WriteToFile("Level " + levelIndex + " has been loaded", true, false);
                Game1.theGame.gameStates.isVisible = true;
            }
            Game1.theGame.player.sPosition = playerSpawningPosition;
            foreach (TiledTileLayer layer in map.TileLayers)
            {
                if (layer.Name == "Collisions")
                {
                    collisionLayer = layer;
                }
            }
            
        }

        public void info(SpriteBatch spriteBatch, GameTime gameTime)
        {
                var timer2 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay2 -= timer2;
                if (remainingdelay2 <= 0)
                {
                    spriteBatch.DrawString(font, "time: 10:45pm", new Vector2(player.sPosition.X + 400,player.sPosition.Y + 275), Color.White);
                }
                var timer3 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                remainingdelay3 -= timer3;
                if (remainingdelay3 <= 0)
                {
                    spriteBatch.DrawString(font, "location: Bermuda Triangle", new Vector2(player.sPosition.X + 300, player.sPosition.Y + 300), Color.White);
                }
        }

        public static int PixelToTile(float pixelCoord)
        {
            return (int)Math.Floor(pixelCoord / tile);
        }

        public static int TileToPixel(int tileCoord)
        {
            return tile * tileCoord;
        }

        public int CellAtPixelCoord(Vector2 pixelCoords)
        {
            if (pixelCoords.X < 0 ||
           pixelCoords.X > map.WidthInPixels || pixelCoords.Y < 0)
                return 1;
            // let the player drop of the bottom of the screen (this means death)
            if (pixelCoords.Y > map.HeightInPixels)
                return 0;
            return CellAtTileCoord(
           PixelToTile(pixelCoords.X), PixelToTile(pixelCoords.Y));
        }

        public int CellAtTileCoord(int tx, int ty)
        {
            if (tx < 0 || tx >= map.Width || ty < 0)
                return 1;
            // let the player drop of the bottom of the screen (this means death)
            if (ty >= map.Height)
                return 0;
            TiledTile tile = collisionLayer.GetTile(tx, ty);
            return tile.Id;
        }

        public void Update(GameTime gameTime)
        {
            int tx = PixelToTile(player.sPosition.X);
            int ty = PixelToTile(player.sPosition.Y);
            bool nx = (player.sPosition.X) % tile != 0;

            bool ny = (player.sPosition.Y) % tile != 0;
            bool cell = CellAtTileCoord(tx, ty) != 0;
            bool cellright = CellAtTileCoord(tx + 1, ty) != 0;
            bool celldown = CellAtTileCoord(tx, ty + 1) != 0;
            bool celldiag = CellAtTileCoord(tx + 1, ty + 1) != 0;

            if (player.sDirection.Y > 0)
            {
                if ((celldown && !cell) || (celldiag && !cellright && nx))
                {
                    // clamp the y position to avoid falling into platform below
                    player.sPosition.Y = TileToPixel(ty) - 5;
                    player.sDirection.Y = 0; // stop downward velocity
                    ny = false; // - no longer overlaps the cells below
                }
            }
            else if (player.sDirection.Y < 0)
            {
                if ((cell && !celldown) || (cellright && !celldiag && nx))
                {
                    // clamp the y position to avoid jumping into platform above
                   player.sPosition.Y = TileToPixel(ty + 1) - 25;
                    player.sDirection.Y = 0; // stop upward velocity
                                         // player is no longer really in that cell, we clamped them
                                         // to the cell below
                    cell = celldown;
                    cellright = celldiag; // (ditto)
                    ny = false; // player no longer overlaps the cells below
                }
            }

            if (player.sDirection.X > 0)
            {
                if ((cellright && !cell) || (celldiag && !celldown && ny))
                {
                    // clamp the x position to avoid moving into the platform
                    // we just hit
                    player.sPosition.X = TileToPixel(tx);
                    player.sDirection.X = 0; // stop horizontal velocity
                }
            }
            else if (player.sDirection.X < 0)
            {
                if ((cell && !cellright) || (celldown && !celldiag && ny))
                {
                    // clamp the x position to avoid moving into the platform
                    // we just hit
                    player.sPosition.X = TileToPixel(tx + 1);
                    player.sDirection.X = 0; // stop horizontal velocity
                }
            }
        }
    }
}

            

 
    



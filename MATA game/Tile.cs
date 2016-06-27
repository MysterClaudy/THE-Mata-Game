using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MATA_game
{
    enum TileCollision
    {
        Passable = 0,
        Impassable = 1
    }
    struct Tile
    {
        public Texture2D texture;
        public TileCollision collision;

        public const int Width = 32;
        public const int Height = 32;

        public static readonly Vector2 size = new Vector2(Width, Height);

        public Tile(Texture2D Texture, TileCollision Collision)
        {
            texture = Texture;
            collision = Collision;

        }
    }
}

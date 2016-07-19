using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;

namespace Apocalyptic_Sunrise
{
    public class Level
    {
        public TiledMap map = null;

        public void LoadContent(ContentManager Content)
        {
            map = Content.Load<TiledMap>("Test2");
        }
    }
}

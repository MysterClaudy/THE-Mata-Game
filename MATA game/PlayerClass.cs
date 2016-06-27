<<<<<<< HEAD:MATA game/MATA game/PlayerClass.cs
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MATA_game
{
    class PlayerClass : AnimatedSprite
    {
        #region properties


        #endregion

        #region Collectors
        public PlayerClass(Vector2 position)
            : base(position)
        {
            framesPerSecond = 2;
             AddAnimation(3, 170, 1, "Left", 71, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 0, "LeftIdle", 71, 80, new Vector2(0, 0));
            AddAnimation(3, 48, 1, "Right", 32, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 1, "RightIdle", 32, 48, new Vector2(0, 0));
            AddAnimation(3, 48, 1, "Up", 32, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 1, "UpIdle", 32, 48, new Vector2(0, 0));
            AddAnimation(3, 48, 1, "Down", 32, 48, new Vector2(0, 0));
            AddAnimation(1, 48, 1, "DownIdle", 32, 48, new Vector2(0, 0));

            PlayAnimation("Idle");
        }

        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("xeonsheet");
        }


        
        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;
            m_position += m_velocity;
        }
        

        public void GetInput(GameTime gameTime)
        {
            m_velocity = new Vector2(0);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 0);
                PlayAnimation("Left");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 0);
                PlayAnimation("Right");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_velocity = new Vector2(0, -5);
                PlayAnimation("Up");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                m_velocity = new Vector2(0, 5);
                PlayAnimation("Down");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, -5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, -5);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 5);
            }
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                
            }
        }
        #endregion
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MATA_game
{
    class PlayerClass : MobileObject
    {
        #region properties


        #endregion

        #region Collectors
        public PlayerClass(Texture2D texture = null
            , Vector2 position = new Vector2()
            , Vector2 size = new Vector2()
            , float rotation = 0f
            , float scale = 1f,
            float speed = 3.0f)
            : base(texture
                  , position
                  , size
                  , rotation
                  , scale
                  , speed)
        {
            m_size = size;
        }



        public override void Update(GameTime gameTime)
        {
            m_position += m_velocity;

            /*if(m_position.X > maxLimit.X)
            {
                m_position.X = minLimit.X;
            }
            else if(m_position.X < minLimit.X)
            {
                m_position.X = maxLimit.X;
            }

            if(m_position.Y > maxLimit.Y)
            {
                m_position.Y = minLimit.Y;
            }
            else if(m_position.Y < minLimit.Y)
            {
                m_position.Y = maxLimit.Y;
            }*/

        }
        

        public void GetInput(GameTime gameTime)
        {
            m_velocity = new Vector2(0);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_velocity = new Vector2(0, -5);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                m_velocity = new Vector2(0, 5);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, -5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, -5);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-5, 5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(5, 5);
            }
        }
        #endregion
    }
}
>>>>>>> master:MATA game/PlayerClass.cs
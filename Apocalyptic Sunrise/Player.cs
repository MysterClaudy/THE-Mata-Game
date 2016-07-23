using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Apocalyptic_Sunrise
{
    public class Player : AnimatedSprite
    {
        bool attacking = false;
        float mySpeed = 200;
        public Level level;
        public AABB aabb;
        public GameStates gameStates;
        public bool isVisible = true;
        public ContentManager Content;
        public Texture2D elevatorTexture;
        public Vector2 elevatorPosition;
        public Texture2D table2Tex;
        public Vector2 table2Pos = new Vector2(128,448);
        public List<Enemy> deadEnemies = new List<Enemy>();
        public bool tableIsVisible = true;
        public Texture2D PressEText;
        public Texture2D shipTexture;
        public Vector2 shipPosition = new Vector2(928,64);
        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("playerSheet");
            elevatorTexture = content.Load<Texture2D>("Elevator");
            table2Tex = content.Load<Texture2D>("Table2");
            PressEText = content.Load<Texture2D>("PressE");
            shipTexture = content.Load<Texture2D>("ship");
            elevatorPosition = new Vector2(1760, 448);
        }

        public Level Level
        {
            get { return level; }
        }
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;
        public bool IsAlive
        {
            get { return isAlive; }
        }

        bool isAlive;
        public bool pressE = false;
        private bool attackingAllowed = false;
        private float previousBottom;
        public const float delay = 3;
        public float remainingdelay = delay;
        public Player(Vector2 position) : base(position)
        {
            FramesPerSecond = 10;

            //Adds all the players animations
            AddAnimation(12, 0, 0, "Down", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 0, 0, "IdleDown", 50, 50, new Vector2(0, 0));
            AddAnimation(12, 50, 0, "Up", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 50, 0, "IdleUp", 50, 50, new Vector2(0, 0));
            AddAnimation(8, 100, 0, "Left", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 100, 0, "IdleLeft", 50, 50, new Vector2(0, 0));
            AddAnimation(8, 100, 8, "Right", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 100, 8, "IdleRight", 50, 50, new Vector2(0, 0));
            AddAnimation(9, 150, 0, "AttackDown", 70, 80, new Vector2(0, 0));
            AddAnimation(9, 230, 0, "AttackUp", 70, 80, new Vector2(-13, -27));
            AddAnimation(9, 310, 0, "AttackLeft", 70, 70, new Vector2(-30, -5));
            AddAnimation(9, 380, 0, "AttackRight", 70, 70, new Vector2(+15, -5));
            //Plays our start animation
            PlayAnimation("IdleDown");
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;
            HandleInput(Keyboard.GetState());
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            sDirection *= mySpeed;
            sPosition += (sDirection * deltaTime);
            Collision();
            if(sPosition.X <= 0)
            {
                sPosition.X = 0;
            }
            if(sPosition.Y <= 0)
            {
                sPosition.Y = 0;
            }
            //if(sPostion.X >= 100)
            //{
            //    sPostion.X = 100;
            //}
            //if(sPostion.Y >= 100)
            //{
            //    sPostion.Y = 100;   
            //}
            if(level.levelIndex == 1)
            {
                Rectangle elevatorRect = new Rectangle((int)elevatorPosition.X, (int)elevatorPosition.Y, elevatorTexture.Width, elevatorTexture.Height);
                if (elevatorRect.Intersects(new Rectangle((int)sPosition.X, (int)sPosition.Y, 50, 50)))
                {
                    gameStates.isVisible = false;
                    for (int i = 0; i < 30; i++)
                    {
                        sPosition = elevatorPosition;
                        elevatorPosition -= new Vector2(0, 1);
                    }
                    Thread.Sleep(100);
                    Game1.theGame.isloadingLevel = true;
                }
            }
            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyState)
        {
            attacking = false;
            if (!attacking)
            {
                mySpeed = 100;
                FramesPerSecond = 10;
                if (keyState.IsKeyDown(Keys.W))
                {
                    //Move char Up
                    sDirection += new Vector2(0, -1);
                    PlayAnimation("Up");
                    currentDir = myDirection.up;

                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    //Move char Left
                    sDirection += new Vector2(-1, 0);
                    PlayAnimation("Left");
                    currentDir = myDirection.left;

                }
                if (keyState.IsKeyDown(Keys.S))
                {
                    //Move char Down
                    sDirection += new Vector2(0, 1);
                    PlayAnimation("Down");
                    currentDir = myDirection.down;

                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    //Move char Right
                    sDirection += new Vector2(1, 0);
                    PlayAnimation("Right");
                    currentDir = myDirection.right;

                }
                if (keyState.IsKeyDown(Keys.LeftShift))
                {
                    mySpeed = 200;
                    FramesPerSecond = 20;
                    if (keyState.IsKeyDown(Keys.W))
                    {
                        //Move char Up
                        sDirection += new Vector2(0, -1);
                        PlayAnimation("Up");
                        currentDir = myDirection.up;

                    }
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        //Move char Left
                        sDirection += new Vector2(-1, 0);
                        PlayAnimation("Left");
                        currentDir = myDirection.left;

                    }
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        //Move char Down
                        sDirection += new Vector2(0, 1);
                        PlayAnimation("Down");
                        currentDir = myDirection.down;

                    }
                    if (keyState.IsKeyDown(Keys.D))
                    {
                        //Move char Right
                        sDirection += new Vector2(1, 0);
                        PlayAnimation("Right");
                        currentDir = myDirection.right;

                    }
                }

                
                if (attackingAllowed)
                {
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        sPosition += new Vector2(0,0);
                        FramesPerSecond = 30;
                        if (currentAnimation.Contains("Down"))
                        {
                            PlayAnimation("AttackDown");
                            attacking = true;
                            currentDir = myDirection.down;
                        }
                        if (currentAnimation.Contains("Left"))
                        {
                            PlayAnimation("AttackLeft");
                            attacking = true;
                            currentDir = myDirection.left;
                        }
                        if (currentAnimation.Contains("Right"))
                        {
                            PlayAnimation("AttackRight");
                            attacking = true;
                            currentDir = myDirection.right;
                        }
                        if (currentAnimation.Contains("Up"))
                        {
                            PlayAnimation("AttackUp");
                            attacking = true;
                            currentDir = myDirection.up;
                        }
                    }
                    
                       
                }
                 if (!attacking)
                {
                    if (currentAnimation.Contains("Left"))
                    {
                        PlayAnimation("IdleLeft");
                    }
                    if (currentAnimation.Contains("Right"))
                    {
                        PlayAnimation("IdleRight");
                    }
                    if (currentAnimation.Contains("Up"))
                    {
                        PlayAnimation("IdleUp");
                    }
                    if (currentAnimation.Contains("Down"))
                    {
                        PlayAnimation("IdleDown");
                    }
                }
                currentDir = myDirection.none;


                currentDir = myDirection.none;
            }
        }

       public void Collision()
        {
            Rectangle playerRect = new Rectangle((int)sPosition.X, (int)sPosition.Y,50,50);
            if(tableIsVisible == true)
            {
                if (playerRect.Intersects(new Rectangle((int)table2Pos.X, (int)table2Pos.Y, table2Tex.Width, table2Tex.Height)))
                {
                    pressE = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                    {
                        attackingAllowed = true;
                        tableIsVisible = false;
                        pressE = false;
                    }
                }
                else
                {
                    pressE = false;
                }
            }

            if(attacking)
            {
                List<Enemy> DeadEnemies = new List<Enemy>();
                foreach  (Enemy enemy in gameStates.enemies)
                {
                    if(playerRect.Intersects(new Rectangle((int)enemy.m_position.X, (int)enemy.m_position.Y, gameStates.enemytexture.Width, gameStates.enemytexture.Height)))
                    {
                        DeadEnemies.Add(enemy);
                    }

                }

                foreach (Enemy enemy in DeadEnemies)
                {
                    gameStates.enemies.Remove(enemy);
                }
            }
            else
            {
                foreach (Enemy enemy in gameStates.enemies)
                {
                    if (playerRect.Intersects(new Rectangle((int)enemy.m_position.X, (int)enemy.m_position.Y, gameStates.enemytexture.Width, gameStates.enemytexture.Height)))
                    {
                        Game1.theGame.healthBar.currentHealth -= 1;
                    }
                }

            }

            if(playerRect.Intersects(new Rectangle((int)shipPosition.X, (int)shipPosition.Y, shipTexture.Width, shipTexture.Height)))
            {
                Game1.theGame.gameStates.gameState = GameStates.GameState.Win;
            }
            
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                attacking = false;
            }
        }
        

           
        
    }
}

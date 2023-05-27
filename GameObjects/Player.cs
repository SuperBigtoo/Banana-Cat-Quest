using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Banana_Catto_Quest.GameObjects
{
    internal class Player : GameObject
    {
        private const float Gravity = 0.0015f;
        private const float JumpForce = 0.2f;
        private float MoveSpeed = 0.08f;

        private List<Texture2D> playerTexture;

        private float hitTimer = 0.0f;
        private float animationTimer;
        private float animationInterval = 0.2f;
        private float speedMoving = 0.0f;
        private float speedJumping = 0.0f;
        private float timerAnimation;

        private float timerAttacking;
        public SoundEffectInstance crying, walking, jumping;

        public int playerDamage;
        public int knockbackLevel = 1;
        public float knockbackForce = 25f;

        private bool IsLeft;
        private bool IsRight;
        private bool IsJumping;
        private bool IsMoving;
        private bool IsAttacking;
        private bool IsAttacked;

        public Player(List<Texture2D> playerTexture) : base(playerTexture[0])
        {
            this.playerTexture = playerTexture;            
            color = Color.White;
            IsAttacking = false;
            IsAttacked = false;
            IsJumping = false;
            IsMoving = false;
            IsLeft = false;
            IsRight = true;

            // Set the initial position of the player
            Singleton.Instance.PlayerPosition = position;
            Singleton.Instance.PlayerRectangle = Rectangle;
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            timerAttacking += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
           

            // Handle animation
            animationTimer += timerAnimation;
            if (IsMoving)
            {
                if (IsJumping)
                {
                    if (speedJumping < 0.1f)
                    {
                        speedJumping += 0.0005f;
                    }
                }
                if (speedMoving < 1.0f)
                {
                    speedMoving += 0.005f;
                }
            }
            else
            {
                speedMoving = 0f;
            }

            // Apply gravity to the player 
            if (position.Y < Singleton.Instance.mainFloor)
            {
                velocity.Y += Gravity * speedJumping;
            }
            else if (position.Y >= Singleton.Instance.mainFloor)
            {
                IsJumping = false;
                position.Y = Singleton.Instance.mainFloor;
                velocity.Y = 0;
            }

            if (!Singleton.Instance.IsPlayerDead)
            {
                // Checking Banana Count
                switch (Singleton.Instance.bananaCount)
                {
                    case 2:
                        playerDamage = 15;
                        break;

                    case 4:
                        playerDamage = 20;
                        knockbackLevel = 2;
                        knockbackForce = 35f;
                        break;

                    case 6:
                        playerDamage = 25;
                        break;

                    case 8:
                        playerDamage = 30;
                        knockbackLevel = 3;
                        knockbackForce = 40f;
                        break;

                    case 10:
                        playerDamage = 35;
                        break;

                    case 12:
                        playerDamage = 40;
                        knockbackLevel = 4;
                        knockbackForce = 45f;
                        break;

                    case 14:
                        playerDamage = 45;
                        break;

                    case 16:
                        playerDamage = 50;
                        knockbackLevel = 5;
                        knockbackForce = 50f;
                        break;
                }

                if (IsAttacking)
                {
                    Animate(playerTexture[5]);

                    if (!IsAttacked)
                    {
                        // Attack Switch
                        CryingAttack();
                    }

                    if (timerAttacking >= 0.5f) // Attack Duration
                    {
                        IsAttacking = false;
                        IsAttacked = false;
                        Animate(playerTexture[0], playerTexture[1]);
                    }
                }

                if (Singleton.Instance.IsPlayerHit)
                {
                    hitTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    crying.Play();

                    if (hitTimer >= 0.0f)
                    {
                        Animate(playerTexture[5], playerTexture[6]);
                        MoveSpeed = 0.04f;
                    }

                    // Check if hit duration has been reached
                    if (hitTimer >= 0.5f)
                    {
                        hitTimer = 0.0f;
                        MoveSpeed = 0.08f;
                        Singleton.Instance.IsPlayerHit = false;
                        Animate(playerTexture[0], playerTexture[1]);
                    }
                }



                // Move the player based on input
                if (Singleton.Instance.keyboardCurrent.IsKeyDown(Keys.Space)
                        && Singleton.Instance.keyboardPreviose.IsKeyUp(Keys.Space) && !IsAttacking)
                {
                    // Crying
                    IsAttacking = true;
                    timerAttacking = 0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    // Move left
                    IsMoving = true;
                    IsLeft = true;
                    IsRight = false;
                    velocity.X = -MoveSpeed;
                    if (!IsJumping && !IsAttacking)
                    {
                        Animate(playerTexture[2], playerTexture[3]);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    // Move right
                    IsMoving = true;
                    IsLeft = false;
                    IsRight = true;
                    velocity.X = MoveSpeed; 
                    if (!IsJumping && !IsAttacking)
                    {
                        Animate(playerTexture[2], playerTexture[3]);
                    }
                }
                else if (!IsJumping && !Singleton.Instance.IsPlayerHit)
                {
                    //Idle
                    IsMoving = false;
                    velocity.X = 0;
                    if (!IsJumping && !IsAttacking)
                    {
                        Animate(playerTexture[0], playerTexture[1]);
                    }
                }

                // Jumping
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !IsJumping)
                {
                    jumping.Play();
                    IsMoving = true;
                    IsJumping = true;
                    velocity.Y = -JumpForce;
                }
                else if (IsJumping)
                {
                    Animate(playerTexture[4]);
                }
            }

            // Update the player's position
            if (!Singleton.Instance.IsGameStart)
            {
                if (position.X >= 0 && position.X <= 1280)
                {
                    position += velocity * speedMoving;
                    position += velocity * speedJumping;                 
                }
                if (position.X < 0)
                {
                    position.X = 0;
                }
                if (position.X > 1280)
                {
                    position.X = 1280;
                }
            }
            else
            {
                if (position.X >= 0 && position.X <= (1280 * 8))
                {
                    position += velocity * speedMoving;
                    position += velocity * speedJumping;

                    if (position.X >= 1110 + (1280 * 4) && Singleton.Instance.IsDoorLock)
                    {
                        position.X = 1110 + (1280 * 4);
                    }
                }
                if (position.X < 0)
                {
                    position.X = 0;
                }
                else if (position.X > (1280 * 8))
                {
                    position.X = 1280 * 8;
                }

                if (position.X >= Singleton.Instance.GameScreen.X / 2 && position.X <= 9600)
                {
                    Singleton.Instance.velocityUI.X += velocity.X * speedMoving;
                }
            }

            Singleton.Instance.PlayerPosition = position;
            //Descaled player bound
            Rectangle descaledBounds = Rectangle;
            descaledBounds.Inflate(-40, -150);
            Singleton.Instance.PlayerRectangle = descaledBounds;
        }

        private void Animate(params Texture2D[] textures)
        {
            if (animationTimer > animationInterval)
            {
                // Cycle through the animation textures
                Texture2D currentTexture = textures[(int)(animationTimer / animationInterval) % textures.Length];
                texture = currentTexture;

                animationTimer = 0f;
            }
        }

        private void CryingAttack()
        {
            //System.Diagnostics.Debug.WriteLine("Player Attack!");
            if (Singleton.Instance.enemiesList.Count != 0)
            {
                foreach (Enemy en in Singleton.Instance.enemiesList)
                {
                    if (Rectangle.Intersects(en.Rectangle))
                    {
                        if (!en.IsDead)
                        {
                            //System.Diagnostics.Debug.WriteLine("Hit! " + en.name + " HP : " + en.EnemyHP);
                            en.EnemyHP -= playerDamage;

                            Vector2 knockbackDirection = en.position - this.position;
                            knockbackDirection.Normalize();
                            en.velocity.X = knockbackDirection.X * knockbackForce;
                            en.position += en.velocity;

                            if (en.EnemyHP <= 0)
                            {
                                en.IsDead = true;
                                //break;
                            }
                        }
                    }
                }
            }

            if (Singleton.Instance.keySection6_Obj != null)
            {
                if (!Singleton.Instance.keySection6)
                {
                    if (Rectangle.Intersects(Singleton.Instance.keySection6_Obj.Rectangle))
                    {
                        Vector2 knockbackDirection = Singleton.Instance.keySection6_Obj.position - this.position;
                        knockbackDirection.Normalize();
                        Singleton.Instance.keySection6_Obj.velocity.X = knockbackDirection.X * knockbackForce;
                        Singleton.Instance.keySection6_Obj.position += Singleton.Instance.keySection6_Obj.velocity;
                    }
                }
            }
            
            crying.Play();
            
            IsAttacked = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player on the screen at its current position
            if (IsLeft) {
                spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
            else if (IsRight)
            {
                spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}

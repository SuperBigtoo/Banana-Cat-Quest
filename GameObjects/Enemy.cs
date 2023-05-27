using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Banana_Catto_Quest.GameObjects
{
    internal class Enemy : GameObject
    {
        private const float Gravity = 0.0015f;
        public float MoveSpeed;

        private List<Texture2D> enemyTexture;
        public List<Texture2D> enemyTexture_2 = new List<Texture2D>();
        public List<Texture2D> itemDropTexture = new List<Texture2D>();

        private float deadTimer = 0.0f;
        private float deadTimerTotal = 0.0f;
        private int fading = 255;
        private bool fadingOff = true;

        private float animationTimer;
        private float animationInterval = 0.2f;
        private float speedMoving = 0.0f;
        private float speedFalling = 0.0f;
        private float timerAnimation;

        private float timerAttacking;
        public SoundEffectInstance enemyAttack_SFX, enemyDead_SFX, itemdrop;

        public bool IsLeft;
        public bool IsRight;
        private bool IsFalling;
        private bool IsCollision;

        public float baseGavity;

        public int EnemyHP;
        public int EnemyDamage;
        public int EnemySection;

        public bool IsDead;
        public bool IsActive;
        private bool IsMoving;
        private bool IsAttacking;
        private bool IsAttacked;

        public Enemy(List<Texture2D> enemyTexture) : base(enemyTexture[0])
        {
            this.enemyTexture = enemyTexture;
            IsDead = false;
            IsActive = false;
            IsAttacking = false;
            IsAttacked = false;
            IsFalling = false;
            IsMoving = false;
            IsCollision = false;
            color = new Color(fading, fading, fading, fading);
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            timerAttacking += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            // Enemey fadding
            color = new Color(fading, fading, fading, fading);

            // Handle animation
            animationTimer += timerAnimation;
            if (IsMoving)
            {
                if (IsFalling)
                {
                    if (speedFalling < 0.1f)
                    {
                        speedFalling += 0.0005f;
                    }
                }
                if (speedMoving < 1.0f)
                {
                    speedMoving += 0.005f;
                }
            }
            else
            {
                speedMoving = 0;
            }

            // Apply gravity to the enemy 
            if (position.Y < baseGavity)
            {
                velocity.Y += Gravity * speedFalling;
            }
            else if (position.Y >= baseGavity)
            {
                IsFalling = false;
                position.Y = baseGavity;
                velocity.Y = 0;
            }

            if (Singleton.Instance.playerSectionEntered == EnemySection) 
            {
                IsActive = true;
            }

            if (IsActive)
            {
                if (!IsDead)
                {
                    //Attacking Animation
                    if (IsAttacking)
                    {
                        if (timerAttacking >= 0.0f)
                        {
                            AnimateAttacking(enemyTexture[4], enemyTexture[5]);

                            if (timerAttacking >= 0.5f)
                            {
                                if (!IsAttacked)
                                {
                                    Attack();
                                }

                                if (timerAttacking >= 0.75f) // Attack Duration
                                {
                                    IsAttacking = false;
                                    IsAttacked = false;
                                    IsCollision = false;
                                }
                            }
                        }
                    }

                    // Movements & Actions
                    if (Rectangle.Intersects(Singleton.Instance.PlayerRectangle) && !IsCollision)
                    {
                        IsCollision = true;
                        if (IsCollision && !IsAttacking)
                        {
                            // Crying
                            IsAttacking = true;
                            timerAttacking = 0f;
                        }
                    }
                    else if (position.X >= Singleton.Instance.PlayerPosition.X && !IsAttacking)
                    {
                        // Move left
                        IsMoving = true;
                        IsLeft = true;
                        IsRight = false;
                        velocity.X = -MoveSpeed;
                        if (!IsFalling && !IsAttacking)
                        {
                            Animate(enemyTexture[2], enemyTexture[3]);
                        }
                    }
                    else if (position.X <= Singleton.Instance.PlayerPosition.X && !IsAttacking)
                    {
                        // Move right
                        IsMoving = true;
                        IsLeft = false;
                        IsRight = true;
                        velocity.X = MoveSpeed;
                        if (!IsFalling && !IsAttacking)
                        {
                            Animate(enemyTexture[2], enemyTexture[3]);
                        }
                    }
                    else if (!IsFalling)
                    {
                        //Idle
                        IsMoving = false;
                        velocity.X = 0;
                        if (!IsFalling && !IsAttacking)
                        {
                            Animate(enemyTexture[0], enemyTexture[1]);
                        }
                    }

                    // Update the enemy's position
                    position += velocity * speedMoving;
                    position += velocity * speedFalling;
                }
                else if (name.Equals("Inc the Conqueror Prime"))
                {
                    //Phase 2 For Boss
                    Singleton.Instance.enemiesList.Add(new Enemy(enemyTexture_2)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 8,
                        EnemyHP = 2000,
                        EnemyDamage = 20,
                        MoveSpeed = 0.075f,
                        baseGavity = this.baseGavity + 30,
                        IsLeft = true,
                        IsRight = false,
                        IsActive = true,
                        position = new Vector2(position.X, this.baseGavity + 30),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemDropTexture)
                    {
                        collected = itemdrop,
                        position = new Vector2(position.X, Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemDropTexture)
                    {
                        collected = itemdrop,
                        position = new Vector2(position.X - 400, Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemDropTexture)
                    {
                        collected = itemdrop,
                        position = new Vector2(position.X + 350, Singleton.Instance.mainFloor + 50)
                    });

                    Singleton.Instance.enemiesList.Remove(this);
                }
                else
                {
                    deadTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    deadTimerTotal += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                    if (deadTimer >= 0.02f)
                    {
                        if (fadingOff)
                        {
                            fading -= 5;
                            if (fading <= 0)
                            {
                                fadingOff = false;
                            }
                        }
                        else
                        {
                            fading += 5;
                            if (fading >= 255)
                            {
                                fadingOff = true;
                            }
                        }

                        // Check if hit duration has been reached
                        if (deadTimerTotal >= 0.8f)
                        {
                            enemyDead_SFX.Play();
                            Singleton.Instance.enemiesList.Remove(this);
                            deadTimer = 0.0f;
                        }

                        deadTimer -= 0.02f;
                    }
                }
            }            
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

        private void AnimateAttacking(params Texture2D[] textures)
        {
            Texture2D currentTexture;
            if (timerAttacking > 0.0f && timerAttacking < 0.5f)
            {
                // Cycle through the animation textures
                currentTexture = textures[0];
                texture = currentTexture;
            }
            else if (timerAttacking > 0.5f)
            {
                currentTexture = textures[1];
                texture = currentTexture;
            }
        }

        private void Attack()
        {
            enemyAttack_SFX.Play();
            if (Rectangle.Intersects(Singleton.Instance.PlayerRectangle)) 
            {
                Singleton.Instance.IsPlayerHit = true;
                Singleton.Instance.PlayerHP -= EnemyDamage;
                //System.Diagnostics.Debug.WriteLine("Enemy Attack!" + Singleton.Instance.PlayerHP);
            }
            IsAttacked = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player on the screen at its current position
            if (IsLeft)
            {
                spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
            else if (IsRight)
            {
                spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}

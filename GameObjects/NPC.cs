using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Banana_Catto_Quest.GameObjects
{
    internal class NPC : GameObject
    {
        private const float Gravity = 0.0015f;
        private const float MoveSpeed = 0.025f;

        private List<Texture2D> npcTexture;

        private float animationTimer;
        private float animationInterval = 0.2f;
        private float speedMoving = 0.0f;
        private float speedFalling = 0.0f;
        private float timerAnimation;

        private bool IsMoving;
        public bool IsLeft;
        public bool IsRight;
        private bool IsFalling;

        public NPC(List<Texture2D> npcTexture) : base(npcTexture[0])
        {
            this.npcTexture = npcTexture;
            IsFalling = false;
            IsMoving = false;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

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

            // Apply gravity
            if (position.Y < Singleton.Instance.mainFloor)
            {
                velocity.Y += Gravity * speedFalling;
            }
            else if (position.Y >= Singleton.Instance.mainFloor)
            {
                IsFalling = false;
                position.Y = Singleton.Instance.mainFloor;
                velocity.Y = 0;
            }

            if (!IsFalling)
            {
                //Idle
                IsMoving = false;
                velocity.X = 0;
                if (!IsFalling)
                {
                    Animate(npcTexture[0], npcTexture[1]);
                }
            }

            // Update the NPC's position (if moving)
            position += velocity * speedMoving;
            position += velocity * speedFalling;
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

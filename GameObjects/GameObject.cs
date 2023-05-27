using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Banana_Catto_Quest.GameObjects
{
    internal class GameObject
    {
        protected Texture2D texture;
        public string name;

        public Vector2 position;
        public Vector2 scale;
        public Vector2 velocity;
        public float rotation;

        public Color color;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        public GameObject(Texture2D texture)
        {
            this.texture = texture;
            position = Vector2.Zero;
            scale = Vector2.One;
            rotation = 0f;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime, bool playerEntered) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}

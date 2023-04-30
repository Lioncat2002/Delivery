using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delivery.src
{
    public abstract class Entity
    {
        public enum currentAnimation
        {
            Idle,
            Run,
            Jumping
        }
        public Vector2 position;
        public Rectangle hitbox;

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}

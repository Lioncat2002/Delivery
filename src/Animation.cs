using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delivery.src
{
    public class Animation
    {
        Texture2D spritesheet;
        int frames;
        int rows = 0;
        int columns = 0;

        float timeSinceLastFrame = 0;
        float width;
        float height;
        public Animation(Texture2D spritesheet, float width = 32, float height = 32)
        {
            this.spritesheet = spritesheet;
            frames = (int)(spritesheet.Width / width);
            this.width = width;
            this.height = height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, float milisecondsperframes = 500, SpriteEffects effect = SpriteEffects.None)
        {
            if (columns < frames)
            {
                var rect = new Rectangle((int)width * columns, rows, (int)width, (int)width);
                spriteBatch.Draw(spritesheet, position, rect, Color.White, 0f, new Vector2(), 1f, effect, 1);
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeSinceLastFrame > milisecondsperframes)
                {
                    timeSinceLastFrame -= milisecondsperframes;
                    columns++;
                    if (columns == frames)
                    {
                        columns = 0;
                    }
                }

            }

        }
    }
}

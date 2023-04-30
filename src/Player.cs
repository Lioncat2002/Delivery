using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Delivery.src
{
    public class Player : Entity
    {

        public Vector2 velocity;
        public Rectangle playerFallRect;
        public SpriteEffects effects;

        public float playerSpeed = 1;
        public float fallSpeed = 5;
        public float jumpSpeed = -10;
        public float startY;

        public bool isFalling = true;
        public bool isJumping;
        public bool isShooting;

        public Animation[] playerAnimation;
        public currentAnimation playerAnimationController;
        public Player(Vector2 position, Texture2D idleSprite, Texture2D runSprite,Texture2D jumpSprite)
        {
            playerAnimation = new Animation[3];

            this.position = position;
            velocity = new Vector2();
            effects = SpriteEffects.None;

            playerAnimation[0] = new Animation(idleSprite);
            playerAnimation[1] = new Animation(runSprite);
            playerAnimation[2] = new Animation(jumpSprite);
            hitbox = new Rectangle((int)position.X, (int)position.Y,8, 6);
            playerFallRect = new Rectangle((int)position.X + 3, (int)position.Y + 8, 6, (int)fallSpeed);
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();


            playerAnimationController = currentAnimation.Idle;
            position = velocity;

            isShooting = keyboard.IsKeyDown(Keys.Enter);

            startY = position.Y;
            Move(keyboard);
            Jump(keyboard);

            if (isFalling)
            {
                velocity.Y += fallSpeed;
                playerAnimationController = currentAnimation.Falling;
            }
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            playerFallRect.X = (int)position.X;
            playerFallRect.Y = (int)(velocity.Y + 10);
        }
        private void Move(KeyboardState keyboard)
        {

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
                playerAnimationController = currentAnimation.Run;
                effects = SpriteEffects.FlipHorizontally;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
                playerAnimationController = currentAnimation.Run;
                effects = SpriteEffects.None;
            }
        }
        private void Jump(KeyboardState keyboard)
        {
            if (isJumping)
            {
                velocity.Y += jumpSpeed;//Making it go up
                jumpSpeed += 1;//Some math (explained later)
                Move(keyboard);
                playerAnimationController = currentAnimation.Jumping;

                if (velocity.Y >= startY)
                //If it's farther than ground
                {
                    velocity.Y = startY;//Then set it on
                    isJumping = false;

                }
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Space) && !isFalling)
                {
                    isJumping = true;
                    isFalling = false;
                    jumpSpeed = -14;//Give it upward thrust
                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            switch (playerAnimationController)
            {
                case currentAnimation.Idle:
                    playerAnimation[0].Draw(spriteBatch, position, gameTime, 500, effects);
                    break;
                case currentAnimation.Run:
                    playerAnimation[1].Draw(spriteBatch, position, gameTime, 100, effects);
                    break;
                case currentAnimation.Jumping:
                    playerAnimation[2].Draw(spriteBatch, position, gameTime, 100, effects);

                    break;
                
            }

        }
    }
}

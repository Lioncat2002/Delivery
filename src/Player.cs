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
        public float fallSpeed = 1;
        public float jumpSpeed = -2;
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
            velocity = position;
            effects = SpriteEffects.None;

            playerAnimation[0] = new Animation(idleSprite,8,8);
            playerAnimation[1] = new Animation(runSprite,8,8);
            playerAnimation[2] = new Animation(jumpSprite,8,8);
            hitbox = new Rectangle((int)position.X, (int)position.Y,8, 5);
            playerFallRect = new Rectangle((int)position.X, (int)position.Y, 6, 6);
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            

            playerAnimationController = currentAnimation.Idle;
            position = velocity;

            startY = position.Y;
            Move(keyboard);
            Jump(keyboard);
            if (isFalling)
            {
                velocity.Y += fallSpeed;
                playerAnimationController = currentAnimation.Jumping;
            }
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            playerFallRect.X = (int)position.X;
            playerFallRect.Y = (int)velocity.Y+2;
        }
        private void Move(KeyboardState keyboard)
        {

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
                playerAnimationController = currentAnimation.Run;
                effects = SpriteEffects.None;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
                playerAnimationController = currentAnimation.Run;
                effects = SpriteEffects.FlipHorizontally;
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
                    jumpSpeed = -5;//Give it upward thrust
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System.Collections.Generic;
using TiledSharp;

namespace Delivery.src.levels
{
    public class Level1 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        #region TileMap
        private TmxMap map;
        private TilemapManager tilemapManager;
        private Texture2D tileset;
        private List<Rectangle> collisionRects;
        private Rectangle startRect;
        private Rectangle endRect;
        #endregion

        #region Player
        private Player player;
        
        #endregion
        public Level1(Game game) : base(game) { }
        public override void LoadContent()
        {
            #region Tilemap
            map = new TmxMap("Content\\levels\\map1.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            int tilesetTileWidth = tileset.Width / tileWidth;

            tilemapManager = new TilemapManager(map, tileset, tilesetTileWidth, tileWidth, tileHeight, GraphicsDevice, Game._spriteBatch);
            #endregion

            #region Collision
            collisionRects = new List<Rectangle>();

            foreach (var o in map.ObjectGroups["Collisions"].Objects)
            {
                if (o.Name == "")
                {
                    collisionRects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
                }
                if (o.Name == "start")
                {
                    startRect = new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);
                }
                if (o.Name == "end")
                {
                    endRect = new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);

                }
            }

            #endregion

            player = new Player(
               new Vector2(startRect.X, startRect.Y),
               Content.Load<Texture2D>("player_idle"),
               Content.Load<Texture2D>("player_run"),
               Content.Load<Texture2D>("player_jump")
            );
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            #region Player Collisions
            var initPos = player.position;
            player.Update();
            //y axis
            
            foreach (var rect in collisionRects)
            {
                if (!player.isJumping)
                    player.isFalling = true;
                if (rect.Intersects(player.playerFallRect))
                {
                    player.isFalling = false;
                    break;
                }
            }
            
            
            //x axis
            foreach (var rect in collisionRects)
            {
                if (rect.Intersects(player.hitbox))
                {
                    player.position = initPos;
                    player.velocity = initPos;
                    break;
                }
            }
            #endregion
        }
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Colors.SkyBlue);
            Game._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            tilemapManager.Draw();
            Game._spriteBatch.End();
        }
    }
}

using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System;
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
        private Rectangle killRect;
        #endregion

        private RenderTarget2D renderTarget;

        #region Player
        private Player player;
        #endregion

        #region UI
        private bool hasWon=false;
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
                else if (o.Name == "start")
                {
                    startRect = new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);
                }
                else if (o.Name == "end")
                {
                    endRect = new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);

                }
                else if(o.Name == "kill")
                {
                    killRect= new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);
                }
            }

            #endregion
            Game.player.position = new Vector2(startRect.X, startRect.Y);
            renderTarget = new RenderTarget2D(GraphicsDevice, 1024, 850);
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            GuiHelper.UpdateSetup(gameTime);
            Game._ui.UpdateStart(gameTime);
            
            #region Player Collisions
            var initPos = Game.player.position;
            Game.player.Update();
            //y axis
            
            foreach (var rect in collisionRects)
            {
                if (!Game.player.isJumping)
                    Game.player.isFalling = true;
                if (rect.Intersects(Game.player.playerFallRect))
                {
                    Game.player.isFalling = false;
                    
                    break;
                }
            }
            
            
            //x axis
            foreach (var rect in collisionRects)
            {
                if (rect.Intersects(Game.player.hitbox))
                {
                    Game.player.position = initPos;
                    Game.player.velocity = initPos;
                    Game.player.isMovingLeft = !Game.player.isMovingLeft;
                    break;
                }
            }
            #endregion

            #region WinCondition
            
            if (endRect.Intersects(Game.player.hitbox))
            {
                hasWon = true;
                Game.player.jumpedFirst = false;
            }
            #endregion

            #region LoseCondition
            if(killRect.Intersects(Game.player.hitbox))
            {
                Console.WriteLine("You died");
            }
            #endregion

            #region UI
            if (hasWon)
            {
                Game.gameColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
                MenuPanel.Push();
                if (Button.Put("Next Level", 30, Color.AliceBlue).Clicked)
                {
                    Game.currentLevel = Utils.Level.Level2;
                    Game.hasLevelChanged = false;
                    Game.gameColor = Color.White;
                }
                MenuPanel.Pop();
            }
            #endregion
            Game._ui.UpdateEnd(gameTime);
            GuiHelper.UpdateCleanup();
        }
        private void DrawLevel(GameTime gameTime)
        {
            Game.GraphicsDevice.SetRenderTarget(renderTarget);
            Game.GraphicsDevice.Clear(Utils.SkyBlue);
            Game._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            tilemapManager.Draw();
            //Game._spriteBatch.Draw(playerIdle, new Vector2(30, 30), Color.White);
            Game.player.Draw(Game._spriteBatch, gameTime);

            Game._spriteBatch.End();
            Game.GraphicsDevice.SetRenderTarget(null);
        }
        public override void Draw(GameTime gameTime)
        {
            DrawLevel(gameTime);
            Game._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            Game._spriteBatch.Draw(renderTarget, new Vector2(0, 0), null, Game.gameColor, 0f, new Vector2(), 8f, SpriteEffects.None, 0);
            Game._spriteBatch.End();
            Game._ui.Draw(gameTime);
        }
    }
}

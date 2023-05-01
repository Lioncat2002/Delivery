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
    public class Level2 : GameScreen
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
        private IMGUI _ui;
        private bool hasWon = false;
        #endregion
        public Level2(Game game) : base(game) { }
        public override void LoadContent()
        {
            #region UI
            FontSystem fontSystem = new FontSystem();
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dogicapixel.ttf")); GuiHelper.Setup(Game, fontSystem);
            _ui = new IMGUI();
            #endregion
            #region Tilemap
            map = new TmxMap("Content\\levels\\map2.tmx");
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
                else if (o.Name == "kill")
                {
                    killRect = new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);
                }
            }

            #endregion

            player = new Player(
               new Vector2(startRect.X, startRect.Y),
               Content.Load<Texture2D>("player_idle"),
               Content.Load<Texture2D>("player_run"),
               Content.Load<Texture2D>("player_jump")
            );

            renderTarget = new RenderTarget2D(GraphicsDevice, 1024, 850);
            Game.gameColor = Color.White;
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            GuiHelper.UpdateSetup(gameTime);
            _ui.UpdateStart(gameTime);

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
                    player.isMovingLeft = !player.isMovingLeft;
                    break;
                }
            }
            #endregion

            #region WinCondition

            if (endRect.Intersects(player.hitbox))
            {
                hasWon = true;
                player.jumpedFirst = false;
            }
            #endregion

            #region LoseCondition
            if (killRect.Intersects(player.hitbox))
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
                    Game.gameColor=Color.White;
                }
                MenuPanel.Pop();
            }
            #endregion
            _ui.UpdateEnd(gameTime);
            GuiHelper.UpdateCleanup();
        }
        private void DrawLevel(GameTime gameTime)
        {
            Game.GraphicsDevice.SetRenderTarget(renderTarget);
            Game.GraphicsDevice.Clear(Utils.SkyBlue);
            Game._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            tilemapManager.Draw();
            //Game._spriteBatch.Draw(playerIdle, new Vector2(30, 30), Color.White);
            player.Draw(Game._spriteBatch, gameTime);

            Game._spriteBatch.End();
            Game.GraphicsDevice.SetRenderTarget(null);
        }
        public override void Draw(GameTime gameTime)
        {
            DrawLevel(gameTime);
            Game._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            Game._spriteBatch.Draw(renderTarget, new Vector2(0, 0), null, Game.gameColor, 0f, new Vector2(), 8f, SpriteEffects.None, 0);
            Game._spriteBatch.End();
            _ui.Draw(gameTime);
        }
    }
}

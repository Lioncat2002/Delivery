using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using TiledSharp;

namespace Delivery.src.levels
{
    public class Level2 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private TmxMap map;
        private TilemapManager tilemapManager;
        private Texture2D tileset;
        public Level2(Game game) : base(game) { }

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
            base.LoadContent();
        }



        public override void Update(GameTime gameTime)
        {

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
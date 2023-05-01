using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;

namespace Delivery.src
{
    public class TilemapManager
    {

        TmxMap map;
        Texture2D tileset;
        RenderTarget2D renderTarget;

        SpriteBatch spriteBatch;

        int tilesetTilesWide;
        int tileWidth;
        int tileHeight;

        public TilemapManager(TmxMap _map, Texture2D _tileset, int _tilesetTilesWide, int _tileWidth, int _tileHeight, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)

        {

            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            this.spriteBatch = spriteBatch;

            renderTarget = new RenderTarget2D(graphicsDevice, 128, 128);
            DrawTilemap(graphicsDevice);
        }

        private void DrawTilemap(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Utils.SkyBlue);
            spriteBatch.Begin();
            for (var i = 0; i < map.TileLayers.Count; i++)
            {
                for (var j = 0; j < map.TileLayers[i].Tiles.Count; j++)
                {
                    int gid = map.TileLayers[i].Tiles[j].Gid;
                    if (gid == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                        float x = (j % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                        Rectangle tilesetRec = new Rectangle((tileWidth) * column, (tileHeight) * row, tileWidth, tileHeight);
                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                    }
                }
            }
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

        }

        public void Draw()
        {
            spriteBatch.Draw(renderTarget, new Vector2(0, 0), null, Color.White, 0f, new Vector2(), 1f, SpriteEffects.None, 0);

        }

    }
}

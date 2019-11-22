using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tempora.Engine
{
    //Enums that represent layers on the tilemap
    public enum MapLayer
    {
        Background = 0,
        Collision = 1,
        Lights = 2,
        Entities = 3
    }

    public class Map
    {
        //Integer multiplier for all map transformations
        public int MapScale;

        //Width of the map in tiles
        public int Width { get; set; }

        //Height of the map in tiles
        public int Height { get; set; }

        //Are we working with an infinite map here?
        public bool Infinite { get; set; }
        
        //All the layer data from the decoded json string
        public List<Dictionary<string, object>> Layers { get; set; }

        //All the decoded layer data from the layers list
        public Dictionary<MapLayer, uint[]> LayerData { get; set; }

        //Width of a tile in pixels
        public int TileWidth { get; set; }

        //Height of the tile in pixels
        public int TileHeight { get; set; }

        //The atlast the represents this map
        public Texture2D Atlas;

        //A list of all the rects to convert indexs to rectangles on the atlast
        private Rectangle[] AtlasRects;

        //Generate a bunch of rects that map each tile to an index
        public void GenerateAtasIndexes()
        {
            int _Width = Atlas.Width / TileWidth;
            int _Height = Atlas.Height / TileHeight;

            AtlasRects = new Rectangle[_Width * _Height];
            int i = 0;
            for(int y = 0; y < _Height; y++)
            {
                for (int x = 0; x < _Width; x++)
                {
                    AtlasRects[i] = new Rectangle(new Point(x * TileWidth, y * TileHeight), new Point(TileWidth, TileHeight));

                    i++;
                }
            }
        }

        //Converts all the points in the JArray to a uint[]
        public void DecondeLayerData()
        {
            for(int i = 0; i < Layers.Count; i++)
            {
                LayerData[(MapLayer)i] = ((Newtonsoft.Json.Linq.JArray)(Layers[i]["data"])).ToObject<uint[]>();
            }
        }

        //Render that specific layer
        public void RenderLayer(SpriteBatch spriteBatch, MapLayer layer)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    //The flat index of the tile
                    int offset = (y * Width) + x;

                    //The ID of the tile we want to draw
                    uint tileID = LayerData[layer][offset];

                    //Check if last bit is 1 == flipped
                    bool flipped = tileID >> 31 == 1;

                    //Turn last bit to 0
                    tileID &= 0xFFFFFF;

                    if (tileID == 0)
                        continue;

                    //Offset ID to remove 0 index
                    tileID -= 1;

                    if(!flipped)
                        spriteBatch.Draw(Atlas, new Rectangle(new Point(x * TileWidth * MapScale, y * TileHeight * MapScale), 
                                                new Point(TileWidth * MapScale, TileHeight * MapScale)), 
                                                AtlasRects[tileID], Color.White);
                    else
                        spriteBatch.Draw(Atlas, new Rectangle(new Point(x * TileWidth * MapScale, y * TileHeight * MapScale),
                                                new Point(TileWidth * MapScale, TileHeight * MapScale)),
                                                AtlasRects[tileID], Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }

    }
}

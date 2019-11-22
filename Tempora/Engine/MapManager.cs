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
    /// <summary>
    /// THIS CLASS NEEDS TO BE REMADE
    /// </summary>
    public class MapManager
    {
        //Generates a map object, tries to load the data into it then returns it (Will render at scale);
        public static Map LoadMap(string fileName, int scale = 1)
        {
            Map m = new Map();

            m.MapScale = scale;

            string fileData = File.ReadAllText("Content/maps/" + fileName + ".map");
            m.Atlas = GameManager.ContentManager.Load<Texture2D>("maps/" + fileName);

            //Initialize map
            JsonConvert.PopulateObject(fileData, m);

            //Generate atlas rects
            m.GenerateAtasIndexes();

            //Set up layer data
            m.LayerData = new Dictionary<MapLayer, uint[]>();

            //Decode layers into arrays
            m.DecondeLayerData();

            return m;
        }
    }
}

using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Nodes;
using ComboFigther;
using ComboFigther.Characters;
using ComboFigther.Decor;

namespace ComboFigther.Scene
{
    class Scene
    {
        int _level;
        string _name;
        List<Slime> _slimes;
        List<Obstacle> _obstacle;
        int _nbAvaibleSoldier;

        public string Name { get => _name; set => _name = value; }
        public int Level { get => _level; set => _level = value; }
        internal List<Slime> Slimes { get => _slimes; set => _slimes = value; }
        internal List<Obstacle> Obstacles { get => _obstacle; set => _obstacle = value; }
        public Vector2 PlayerPosition;
        public int NbAvaibleSoldier { get => _nbAvaibleSoldier; set => _nbAvaibleSoldier = value; }

        public Scene(string name)
        {
            Name = name;
            string path = Path.Combine(SceneManager.MapPath, name + ".json");
            Slimes = new List<Slime>();
            Obstacles = new List<Obstacle>();
            CreateScene(path);
        }

        void CreateScene(string path)
        {
            string jsonString = File.ReadAllText(path);

            JsonDocument doc = JsonDocument.Parse(jsonString);

            // Access the root element of the JSON
            JsonElement root = doc.RootElement;

            // To access a value from a JsonElement
            Name = root.GetProperty("name").GetString();
            Level = root.GetProperty("level").GetInt32();
            NbAvaibleSoldier = root.GetProperty("nbSoldier").GetInt32();
            //get map data
            JsonElement dataArray = root.GetProperty("data");

            List<int> dataList = new List<int>();

            foreach (JsonElement element in dataArray.EnumerateArray())
            {
                dataList.Add(element.GetInt32());
            }

            int[] mapData = dataList.ToArray();

            int width = root.GetProperty("width").GetInt32();
            int height = root.GetProperty("height").GetInt32();
            int tileHeigth = root.GetProperty("tileheight").GetInt32();
            int tileWidth = root.GetProperty("tilewidth").GetInt32();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int element = mapData[i * width + j];

                    switch (element)
                    {
                        case 19:
                            Slime slime = new Slime(new Vector2(j * tileWidth, i * tileHeigth),Game1.SlimeSprite);
                            Slimes.Add(slime);
                            break;

                        case 1:
                            Obstacle obstacle = new Obstacle(Game1.RockSprite, new Vector2(j * tileWidth, i * tileHeigth), new Microsoft.Xna.Framework.Vector2(32,32));
                            Obstacles.Add(obstacle);
                            break;

                        case 20:
                            PlayerPosition = new Vector2(j * tileWidth, i * tileHeigth);
                            break;

                        default:
                            break;
                    }
                }
            }


        }
    }
}

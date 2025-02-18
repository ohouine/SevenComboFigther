using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Utility;

namespace ComboFigther.Scene
{
    class SceneManager
    {
        List<Scene> _scenes;
        public static string MapPath = "../../../Content/Map/";
        public SceneManager()
        {
            _scenes = new List<Scene>();
            if (Directory.Exists(MapPath))
            {
                foreach (string file in Directory.GetFiles(MapPath, "*.json"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    _scenes.Add(new Scene(fileName));
                }
            }
        }

        public void LoadScene(string name)
        {
            foreach (Scene scene in _scenes)
            {
                if (scene.Name == name)
                {
                    LoadScene(scene);
                    return;
                }
            }
        }

        public void LoadScene(int level)
        {
            foreach (Scene scene in _scenes)
            {
                if (scene.Level == level)
                {
                    LoadScene(scene);
                    return;
                }
            }
        }

        void LoadScene(Scene scene)
        {
            Manager.Slimes = scene.Slimes;
            Manager.Enemys.AddRange(scene.Slimes);
            Manager.Characters.AddRange(scene.Slimes);
            Manager.Obstacles = scene.Obstacles;
            Manager.Player.Position = scene.PlayerPosition;
        }

    }
}

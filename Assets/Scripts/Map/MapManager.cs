using System.Linq;
using UnityEngine;

namespace TSoft.Map
{
    public class MapManager : MonoBehaviour
    {
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Start()
        {
            GenerateNewMap();
            /*if (GameSave.Instance.IsMapExist())
            {
                var map = GameSave.Instance.MapSaved;
                if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
                {
                    // payer has already reached the boss, generate a new map
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;
                    // player has not reached the boss yet, load the current map
                    view.ShowMap(map);
                }
            }
            else
            {
                GenerateNewMap();
            }*/
        }

        public void GenerateNewMap()
        {
            var config = view.GetRandomConfig();
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            view.ShowMap(map);
        }

        public void SaveMap()
        {
            if (CurrentMap == null) 
                return;
            
            GameSave.Instance.SaveMap(CurrentMap);
        }

        private void OnApplicationQuit()
        {
            SaveMap();
        }
    }
}


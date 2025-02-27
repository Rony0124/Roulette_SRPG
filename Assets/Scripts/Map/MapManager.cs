using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TSoft.Map
{
    public class MapManager : MonoBehaviour
    {
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Start()
        {
            if (GameSave.Instance.IsMapExist())
            {
                var mapJson = GameSave.Instance.MapSaved;
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
             
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
            }
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
            
            string mapJson = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            
            GameSave.Instance.SaveMap(mapJson);
        }

        private void OnApplicationQuit()
        {
            SaveMap();
        }
    }
}


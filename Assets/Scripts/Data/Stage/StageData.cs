using System;
using UnityEngine;

namespace TSoft.Data.Stage
{
    [Serializable]
    public class StageData
    {
        public DataRegistryIdSO[] monsterIds;
        public DataRegistryIdSO bossId;
        public Sprite background;
    }
}

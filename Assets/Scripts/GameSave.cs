using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TSoft.Core;
using TSoft.Data.Registry;
using TSoft.Data.Skill;
using TSoft.InGame;
using UnityEngine;

namespace TSoft
{
    public class GameSave : Singleton<GameSave>
    {
        private ES3Settings easySaveSettings;
        
        //gold
        public Action<int> onGoldChanged;
        
        private int gold;
        public int Gold => gold;
        
        private static readonly int MaxGold = 999999999;
        private static readonly int MinGold = 0;
        
        //items
        private List<Guid> possessItemIds;
        
        public HashSet<Guid> possessItemIdsSet;
        
        private static readonly string GoldKey = "gold";
        private static readonly string ItemIdKey = "ItemId";
        
        //equippedItem
        private Dictionary<int, Guid> skillEquippedDictionary;
        private Dictionary<int, Guid> artifactEquippedSet;
        
        public Dictionary<int, Guid> SkillEquippedDictionary => skillEquippedDictionary;
        public Dictionary<int, Guid> ArtifactEquippedSet => artifactEquippedSet;
        
        //map
        private Map.Map mapSaved;
        public Map.Map MapSaved => mapSaved;
        
        private static readonly string MapKey = "map";
        
        
        private void Awake()
        {
            easySaveSettings = new ES3Settings(ES3.EncryptionType.AES, "IAmCheater");
            easySaveSettings.compressionType = ES3.CompressionType.Gzip;
            easySaveSettings.path = "GameSave/GameSave.bin";
        
            LoadFromSaveFile(); 
        }
        
        #region Gold

        public bool HasEnoughGold(int value) => gold >= value;
        
        public void AddGold(int addValue)
        {
            SetGold(gold + addValue);
        }

        private void SetGold(int newValue)
        {
            gold = Math.Clamp(newValue, MinGold, MaxGold);
            onGoldChanged?.Invoke(gold);
            
            SaveRaw(GoldKey, gold);
        }

        private void ResetGold()
        {
            gold = 0;
            SetGold(0);
        }

        #endregion

        #region Item

        public void AddPossessItem(Guid rid)
        {
            if (possessItemIdsSet.Contains(rid))
                return;

            possessItemIdsSet.Add(rid);
            possessItemIds = possessItemIdsSet.ToList();

            SaveItemsRaw();
        }
        
        private void SaveItemsRaw()
        {
            SaveRaw(ItemIdKey, possessItemIds);
        }
        
        public bool HasItemsId(Guid itemGuid)
        {
            return possessItemIdsSet.Contains(itemGuid);
        }

        public void RemoveItem(Guid rid)
        {
            if (!possessItemIdsSet.Contains(rid))
                return;
            
            possessItemIdsSet.Remove(rid);
            possessItemIds = possessItemIdsSet.ToList();
            
            SaveItemsRaw();
        }

        private void RemoveAllItemsIds()
        {
            possessItemIds.Clear();
            
            SaveItemsRaw();
        }

        #endregion

        #region EquippedItem

        public void SaveEquippedSkill(CardPatternType pattern, Guid skillId)
        {
            if (skillEquippedDictionary.ContainsKey((int)pattern))
            {
                skillEquippedDictionary[(int)pattern] = skillId;
            }
            else
            {
                if (!skillEquippedDictionary.TryAdd((int)pattern, skillId))
                {
                    Debug.Log("Can not Save Equipped Skill");
                }
            }
        }

        #endregion
        
        #region Map

        public void SaveMap(Map.Map map)
        {
            mapSaved = map;
            SaveRaw(MapKey, mapSaved);
        }

        public void ResetMap()
        {
            mapSaved = null;
            SaveMap(mapSaved);
        }

        public bool IsMapExist()
        {
            return mapSaved != null;
        }
        
        #endregion
        
        private void LoadFromSaveFile() 
        {
            gold = LoadRaw(GoldKey, gold);
            possessItemIds = LoadRaw(ItemIdKey, possessItemIds);
            mapSaved = LoadRaw(MapKey, mapSaved);

            possessItemIdsSet = !possessItemIds.IsNullOrEmpty() ? new HashSet<Guid>(possessItemIds) : new HashSet<Guid>();
            
            Debug.Log("[GameSave] Load Finished"); 
        }

        public void ClearSaveFile()
        {
            ResetGold();
            RemoveAllItemsIds();
        }
        
        [IngameDebugConsole.ConsoleMethod("gamesave.clearsave", "세이브 파일 클리어")]
        public static void ConsoleCmd_ClearSaveFile()
        {
            Instance.ClearSaveFile();
        }
        
        static void SaveRaw<T>(string key, T value)
        {
            ES3.Save(key, value, Instance.easySaveSettings);
        }
        
        static T LoadRaw<T>(string key, T defaultValue)
        {
            return ES3.Load(key, defaultValue, Instance.easySaveSettings);
        }
        
        static string LoadRaw<T>(string key,  string defaultValue)
        {
            return ES3.LoadString(key, defaultValue, Instance.easySaveSettings);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TSoft.Core;
using UnityEngine;

namespace TSoft
{
    public class GameSave : Singleton<GameSave>
    {
        private ES3Settings easySaveSettings;
        
        public Action<int, int> OnGoldChanged;
        
        //gold
        private int gold;
        
        private static readonly int MaxGold = 999999999;
        private static readonly int MinGold = 0;
        
        
        //items
        private List<Guid> possessItemIds;
        
        public HashSet<Guid> possessItemIdsSet;
        
        private static readonly string GoldKey = "gold";
        private static readonly string ItemIdKey = "ItemId";
        
        private void Awake()
        {
            easySaveSettings = new ES3Settings(ES3.EncryptionType.AES, "IAmCheater");
            easySaveSettings.compressionType = ES3.CompressionType.Gzip;
            easySaveSettings.path = "GameSave/GameSave.bin";
        
            LoadFromSaveFile(); 
        }
        
        #region Gold
        
        public void AddGold(int addValue)
        {
            SetGold(gold + addValue);
        }

        private void SetGold(int newValue)
        {
            gold = Math.Clamp(newValue, MinGold, MaxGold);
            SaveRaw(GoldKey, gold);
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

        #endregion
        
        private void LoadFromSaveFile() 
        {
            gold = LoadRaw(GoldKey, gold);
            possessItemIds = LoadRaw(ItemIdKey, possessItemIds);

            possessItemIdsSet = !possessItemIds.IsNullOrEmpty() ? new HashSet<Guid>(possessItemIds) : new HashSet<Guid>();
            
            Debug.Log("[GameSave] Load Finished"); 
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

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TSoft
{
    public static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void MainBeforeScene()
        {
            var gameContextPrefabOp = Addressables.LoadAssetAsync<GameObject>("GameContext");
            gameContextPrefabOp.WaitForCompletion();
            
            Debug.Log($"[Main.MainBeforeScene] GameContext Load");
            
            var gameContextPrefab = gameContextPrefabOp.Result;
            var gameContext = Object.Instantiate(gameContextPrefab);
            Object.DontDestroyOnLoad(gameContext);
            
            var gameSavePrefabOp = Addressables.LoadAssetAsync<GameObject>("GameSave");
            gameSavePrefabOp.WaitForCompletion();
            
            Debug.Log($"[Main.MainBeforeScene] GameSave Load");
            
            var gameSavePrefab = gameSavePrefabOp.Result;
            var gameSave = Object.Instantiate(gameSavePrefab);
            Object.DontDestroyOnLoad(gameSave);
        }
    }
}

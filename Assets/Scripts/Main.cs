using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TSoft
{
    public static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void MainBeforeScene()
        {
            //GameContext
            var gameContextPrefabOp = Addressables.LoadAssetAsync<GameObject>("GameContext");
            gameContextPrefabOp.WaitForCompletion();
            
            Debug.Log($"[Main.MainBeforeScene] GameContext Load");
            
            var gameContextPrefab = gameContextPrefabOp.Result;
            var gameContext = Object.Instantiate(gameContextPrefab);
            Object.DontDestroyOnLoad(gameContext);
            
            //GameSave
            var gameSavePrefabOp = Addressables.LoadAssetAsync<GameObject>("GameSave");
            gameSavePrefabOp.WaitForCompletion();
            
            Debug.Log($"[Main.MainBeforeScene] GameSave Load");
            
            var gameSavePrefab = gameSavePrefabOp.Result;
            var gameSave = Object.Instantiate(gameSavePrefab);
            Object.DontDestroyOnLoad(gameSave);
            
            //InGameDebugConsole
            var consolePrefabOp = Addressables.LoadAssetAsync<GameObject>("IngameDebugConsole");
            consolePrefabOp.WaitForCompletion();
            
            Debug.Log($"[Main.MainBeforeScene] InGameDebugConsole Load");
            
            var consolePrefab = consolePrefabOp.Result;
            var console = Object.Instantiate(consolePrefab);
            Object.DontDestroyOnLoad(console);
        }
    }
}

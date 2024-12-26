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
            var gameContextPrefab = gameContextPrefabOp.Result;
            
            var gameContext = Object.Instantiate(gameContextPrefab);
            Object.DontDestroyOnLoad(gameContext);
        }
    }
}

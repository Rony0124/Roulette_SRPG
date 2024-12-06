using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TSoft
{
    public class LoadFromScripts : MonoBehaviour
    {
        public List<AssetReference> reference;
        
        // Start the load operation on start
        void Start()
        {
            foreach (var re in reference)
            {
                AsyncOperationHandle handle = re.LoadAssetAsync<GameObject>();
                handle.Completed += Handle_Completed;
            }
           
        }

        // Instantiate the loaded prefab on complete
        private void Handle_Completed(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
               // Instantiate(reference.Asset, transform);
            }
            else
            {
                Debug.LogError("AssetReference failed to load.");
            }
        }

        // Release asset when parent object is destroyed
        private void OnDestroy()
        {
            foreach (var re in reference)
            {
                re.ReleaseAsset();
            }
           
        }
    }
}

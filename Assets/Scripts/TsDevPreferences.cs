using TSoft.Data;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
    public class TsDevPreferences : EditorWindow
    {
        public static TsDevPreferences Instance;
        
        int toolbarInt = 0;
        string[] toolbarStrings = {"InGame", "StageMap", "Lobby" };
        
        public static DataRegistryIdSO MonsterId;
        
        private static bool prefsLoaded = false;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void MainBeforeScene()
        {
            LoadPrefs();
        }
        
        [MenuItem("Window/DevPreferences")]
        public static void ShowWindow()
        {
            Instance = GetWindow<TsDevPreferences>("DevPreferences");
        }

        private static void LoadPrefs()
        {
            string overrideStagePath = EditorPrefs.GetString("monsterIdPath", "");

            if (overrideStagePath.Length > 0)
                MonsterId = AssetDatabase.LoadAssetAtPath<DataRegistryIdSO>(overrideStagePath);
        }

        private static void SavePrefs()
        {
            string monsterIdPath = AssetDatabase.GetAssetPath(MonsterId);
            EditorPrefs.SetString("monsterIdPath", monsterIdPath);
        }

        private void OnGUI()
        {
            if (!prefsLoaded)
            {
                LoadPrefs();
                prefsLoaded = true;
            }
            
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
            GUILayout.Space(10);
        
            switch(toolbarInt)
            {
                case 0:
                    OnGUI_InGame();
                    break;
                case 1:
                    OnGUI_Stage();
                    break;
                case 2:
                    OnGUI_Lobby();
                    break;
            }
            
            if (GUI.changed)
            {
                SavePrefs();
            }
        }

        private void OnGUI_InGame()
        {
            GUILayout.Label("Monster", EditorStyles.boldLabel);
            MonsterId = EditorGUILayout.ObjectField(new GUIContent("Monster Id", "스테이지를 아래 값으로 고정합니다."), MonsterId, typeof(DataRegistryIdSO), false) as DataRegistryIdSO;
        }
        
        private void OnGUI_Stage()
        {
            GUILayout.Label("Stage", EditorStyles.boldLabel);
        }

        private void OnGUI_Lobby()
        {
            GUILayout.Label("Lobby", EditorStyles.boldLabel);
        }
    }
#endif


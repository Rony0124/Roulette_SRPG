using TSoft.Data.Monster;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
    namespace TSoft.Utils
    {
        public class TsDevPreferences : EditorWindow
        {
            public static TsDevPreferences Instance;
        
            int toolbarInt = 0;
            string[] toolbarStrings = {"InGame", "StageMap", "Lobby" };
        
            public static MonsterDataSO Monster;
            public static bool overrideMonster;
        
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
                    Monster = AssetDatabase.LoadAssetAtPath<MonsterDataSO>(overrideStagePath);
                
                overrideMonster = EditorPrefs.GetBool("overrideMonster", overrideMonster);
            }

            private static void SavePrefs()
            {
                string monsterIdPath = AssetDatabase.GetAssetPath(Monster);
                EditorPrefs.SetString("monsterIdPath", monsterIdPath);
                EditorPrefs.SetBool("overrideMonster", overrideMonster);
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
                Monster = EditorGUILayout.ObjectField(new GUIContent("Monster Id", "몬스터 id 값 설정"), Monster, typeof(MonsterDataSO), false) as MonsterDataSO;
                overrideMonster = EditorGUILayout.Toggle(new GUIContent("Override Monster", "테스트 몬스터로 바꿀지 설정"), overrideMonster);
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
    }
#endif


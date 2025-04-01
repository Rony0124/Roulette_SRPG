using TSoft.Data.Monster;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
    namespace HF.Utils
    {
        public class HfDevPreferences : EditorWindow
        {
            public static HfDevPreferences Instance;
        
            int toolbarInt = 0;
            string[] toolbarStrings = {"InGame", "StageMap", "Lobby" };
        
            public static MonsterDataSO Monster;
            public static bool overrideMonster;
            public static bool Ai;
        
            private static bool prefsLoaded = false;
        
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            public static void MainBeforeScene()
            {
                LoadPrefs();
            }
        
            [MenuItem("Window/DevPreferences")]
            public static void ShowWindow()
            {
                Instance = GetWindow<HfDevPreferences>("DevPreferences");
            }

            private static void LoadPrefs()
            {
                string overrideStagePath = EditorPrefs.GetString("monsterIdPath", "");

                if (overrideStagePath.Length > 0)
                    Monster = AssetDatabase.LoadAssetAtPath<MonsterDataSO>(overrideStagePath);
                
                overrideMonster = EditorPrefs.GetBool("overrideMonster", overrideMonster);
                Ai = EditorPrefs.GetBool("Ai", Ai);
            }

            private static void SavePrefs()
            {
                string monsterIdPath = AssetDatabase.GetAssetPath(Monster);
                EditorPrefs.SetString("monsterIdPath", monsterIdPath);
                
                EditorPrefs.SetBool("overrideMonster", overrideMonster);
                EditorPrefs.SetBool("Ai", Ai);
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
                Ai = EditorGUILayout.Toggle(new GUIContent("Ai", "Ai mode"), Ai);
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


using UnityEditor;
using UnityEngine;
using EvieEngine.Audio;
using EvieEngine.Messages;
using EvieEngine.States;
using EvieEngine.Triggers;

namespace EvieEngine.Editor
{
    public class FastElementCreate : MonoBehaviour
    {
        [MenuItem("GameObject/Evie Engine/Глобальный объект EvieEngine", false, 10)]
        static void CreateEvieEngine()
        {
            GameObject go = new GameObject("EvieEngine");
            go.AddComponent<EvieEngine.EvieEngineGlobalObject>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/Evie Engine/Менеджеры/Message Manager", false, 10)]
        static void CreateMessageManager()
        {
            GameObject go = new GameObject("MessageManager");
            go.AddComponent<MessageManager>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/Evie Engine/Менеджеры/State Manager", false, 10)]
        static void CreateStateManager()
        {
            GameObject go = new GameObject("StateManager");
            go.AddComponent<StateManager>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/Evie Engine/Менеджеры/Trigger Manager", false, 10)]
        static void CreateTriggerManager()
        {
            GameObject go = new GameObject("TriggerManager");
            go.AddComponent<TriggerManager>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/Evie Engine/Менеджеры/Audio Manager", false, 10)]
        static void CreateAudioManager()
        {
            GameObject go = new GameObject("AudioManager");
            go.AddComponent<AudioManager>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/Evie Engine/Игровые системы/First Person Controller (Готовый к использованию)", false, 10)]
        static void CreateFPC()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/EvieEngine/Prefabs/Player.prefab");

            if (prefab == null)
            {
                Debug.LogError("Префаб не найден");
                return;
            }

            GameObject instance =
                (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            Undo.RegisterCreatedObjectUndo(instance, "Player");
            Selection.activeGameObject = instance;
        }

        [MenuItem("GameObject/Evie Engine/Игровые системы/HUD (Готовый к использованию)", false, 10)]
        static void CreateFPCHUD()
        {
            var selected = Selection.activeGameObject;
            if (selected == null || selected.GetComponent<Canvas>() == null)
            {
                EditorUtility.DisplayDialog(
                    "Canvas не выбран",
                    "Выбери объект с компонентом Canvas в Hierarchy",
                    "OK");
                return;
            }

            var canvas = selected.transform;

            var aim = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/EvieEngine/Prefabs/HUD/AIM.prefab");
            var gunHud = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/EvieEngine/Prefabs/HUD/GUN HUD.prefab");
            var statsHud = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/EvieEngine/Prefabs/HUD/STATS HUD.prefab");

            if (aim == null || gunHud == null || statsHud == null)
            {
                Debug.LogError("Один или несколько HUD-префабов не найдены");
                return;
            }

            CreateChild(aim, canvas, "AIM HUD");
            CreateChild(gunHud, canvas, "GUN HUD");
            CreateChild(statsHud, canvas, "STATS HUD");
        }

        static void CreateChild(GameObject prefab, Transform parent, string undoName)
        {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            Undo.RegisterCreatedObjectUndo(instance, undoName);

            var rect = instance.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
            }
        }

        [MenuItem("GameObject/Evie Engine/Игровые системы/HUD (Готовый к использованию)", true)]
        static bool ValidateCreateFPCHUD()
        {
            return Selection.activeGameObject != null &&
                   Selection.activeGameObject.GetComponent<Canvas>() != null;
        }
    }
}
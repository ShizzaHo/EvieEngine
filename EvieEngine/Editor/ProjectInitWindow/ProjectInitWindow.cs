using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EvieEngine.Editor
{
    public class ProjectInitWindow : EditorWindow
    {
        [MenuItem("Evie Engine/Инициализатор")]
        static void InitializeProject()
        {
            ProjectInitWindow window = GetWindow<ProjectInitWindow>();
            window.titleContent = new GUIContent("Инициализатор");
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                rootVisualElement.Q<VisualElement>("warning").visible = false;
            }
            else
            {
                rootVisualElement.Q<VisualElement>("warning").visible = true;
            }
        }

        void CreateGUI()
        {
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EvieEngine/Editor/ProjectInitWindow/init.uxml");
            var clone = visualTree.Instantiate();
            rootVisualElement.Add(clone);
        }
    }
}
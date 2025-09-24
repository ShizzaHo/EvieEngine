using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class EventTriggerEditor : EditorWindow
{
    [MenuItem("Evie Engine/Tools/Event Trigger")]
    static void HelloWorld()
    {
        EventTriggerEditor window = GetWindow<EventTriggerEditor>();
        window.titleContent = new GUIContent("Event Trigger");
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
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EvieEngine/Editor/EventTrigger/EventTriggerUI.uxml");
        var clone = visualTree.Instantiate();
        rootVisualElement.Add(clone);

        UpdateCategory();
        ClearEvents();
        
        rootVisualElement.Q<Button>("update").clicked += () =>
        {
            UpdateCategory();
            ClearEvents();
        };
    }

    void UpdateCategory()
    {
        var categories = rootVisualElement.Q<VisualElement>("categories");
        
        categories.Clear();
        
        EventTriggerBind[] allTriggers = FindObjectsOfType<EventTriggerBind>();
        foreach (var triggerBind in allTriggers)
        {
            Button button = new Button(() =>
            {
                LoadEvents(triggerBind.category);
            });
                
            button.name = triggerBind.category;
            button.text = triggerBind.category;
            categories.Add(button);
        }
    }
    
    void ClearEvents()
    {
        var events = rootVisualElement.Q<VisualElement>("events");
        
        events.Clear();
    }

    void LoadEvents(string category)
    {
        var events = rootVisualElement.Q<VisualElement>("events");
        ClearEvents();
        
        EventTriggerBind[] allTriggers = FindObjectsOfType<EventTriggerBind>();
        foreach (var triggerBind in allTriggers)
        {
            if (triggerBind.category == category)
            {
                foreach (var e in triggerBind.eventTriggers)
                {
                    Button button = new Button(() =>
                    {
                        
                    });
                
                    button.name = e.name;
                    button.text = e.name;
                    events.Add(button);
                }
            }
        }
    }

}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events; // ← обязательно для List

public class EventTriggerBind : MonoBehaviour
{
    public string category;

    public List<EventTrigger> eventTriggers = new List<EventTrigger>();
}

[System.Serializable]
public class EventTrigger
{
    public string name;
    public UnityEvent onTrigger;
}
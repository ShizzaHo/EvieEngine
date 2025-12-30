using EvieEngine.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvieDemo_Triggers : MonoBehaviour
{
    public Text text;
    public GameObject obj;
    private Vector3 baseScale;

    public GameObject particle;
    
    void Start()
    {
        TriggerManager.Instance.AddTrigger("oneTrigger", false);
        TriggerManager.Instance.AddTrigger("twoTrigger", false);
        TriggerManager.Instance.AddTrigger("threeTrigger", false);
        
        baseScale = obj.transform.localScale;
    }
    
    void Update()
    {
        UIStateUpdate();

        ChangeCube();
    }

    void UIStateUpdate()
    {
        text.text = $"Состояние всех триггеров:\n" +
                    $"oneTrigger = {TriggerManager.Instance.GetTriggerState("oneTrigger").ToString()}\n" +
                    $"twoTrigger = {TriggerManager.Instance.GetTriggerState("twoTrigger").ToString()}\n" +
                    $"threeTrigger = {TriggerManager.Instance.GetTriggerState("threeTrigger").ToString()}";
    }

    public void changeTrigger(string triggerName)
    {
        TriggerManager.Instance.SetTriggerState(triggerName, !TriggerManager.Instance.GetTriggerState(triggerName));
    }

    public void ChangeCube()
    {
        if (TriggerManager.Instance.GetTriggerState("oneTrigger"))
        {
            obj.transform.Rotate(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        }
        
        if (TriggerManager.Instance.GetTriggerState("twoTrigger"))
        {
            float scaleFactor = 1f + Mathf.Sin(Time.time * 2) * 2;
            obj.transform.localScale = baseScale * scaleFactor;
        }
        
        particle.SetActive(TriggerManager.Instance.GetTriggerState("threeTrigger"));
    }
}

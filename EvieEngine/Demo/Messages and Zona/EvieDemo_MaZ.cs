using System;
using EvieEngine.Messages;
using UnityEngine;

public class EvieDemo_MaZ : MonoBehaviour
{
    public GameObject part;
    
    private void Start()
    {
        MessageManager.Instance.Subscribe("inside", BoxInside);
        MessageManager.Instance.Subscribe("outside", BoxOutside);
    }

    private void OnDestroy()
    {
        MessageManager.Instance.Unsubscribe("inside", BoxInside);
        MessageManager.Instance.Unsubscribe("outside", BoxOutside);
    }

    public void BoxInside()
    {
        part.SetActive(true);
    }
    
    public void BoxOutside()
    {
        part.SetActive(false);
    }
}

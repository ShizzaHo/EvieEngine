using System;
using EvieEngine.AIM;
using UnityEngine;
using UnityEngine.UI;

public class EvieDemo_AIMFeedback : AimFeedback
{
    public Image aim; 
    
    private void Awake()
    {
        aim.enabled = false;
    }

    public override void OnFeedbackEnter()
    {
        aim.enabled = true;
    }
    
    public override void OnFeedbackExit()
    {
        aim.enabled = false;
    }
}

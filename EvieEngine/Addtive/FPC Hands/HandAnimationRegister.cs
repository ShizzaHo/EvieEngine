using System;
using System.Collections;
using EvieEngine.FPC;
using UnityEngine;
using UnityEngine.Events;

public class HandAnimationRegister : MonoBehaviour
{
    public Animator animator;

    [Header("Triggers")]
    public string idleTrigger = "idle";
    public string walkTrigger = "walk";
    public string runTrigger = "run";
    
    public string showTrigger = "show";
    public string hideTrigger = "hide";
    public float showDuration = 1;
    public float hideDuration = 1;

    private string currentTrigger;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        string newTrigger;

        if (!isMoving)
        {
            newTrigger = idleTrigger;
        }
        else if (isMoving && FPCController.Instance.isRunning)
        {
            newTrigger = runTrigger;
        }
        else
        {
            newTrigger = walkTrigger;
        }

        if (newTrigger != currentTrigger)
        {
            animator.ResetTrigger(idleTrigger);
            animator.ResetTrigger(walkTrigger);
            animator.ResetTrigger(runTrigger);

            animator.SetTrigger(newTrigger);
            currentTrigger = newTrigger;
        }
    }
    
    public void PlayAnimationWithCallback(string triggerName, Action onComplete, float waitTime)
    {
        StartCoroutine(PlayAnimationCoroutine(triggerName, onComplete, waitTime));
    }
    
    private IEnumerator PlayAnimationCoroutine(string triggerName, Action onComplete, float waitTime)
    {
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(waitTime);

        onComplete?.Invoke();
    }
}
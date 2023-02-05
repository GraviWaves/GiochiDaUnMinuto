using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void SetFloat(string triggerName, float value)
    {
        animator.SetFloat(triggerName, value);
    }
}

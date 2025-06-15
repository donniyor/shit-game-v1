using UnityEngine;

public class KneelState : State
{
    public AnimationClip anim;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        if (!input.grounded || input.yInput >= 0)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
    }
}
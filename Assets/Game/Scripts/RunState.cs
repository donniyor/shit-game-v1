using UnityEngine;

public class RunState : State
{
    public AnimationClip anim;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = Helpers.Map(input.maxXSpeed, 0, 1, 0, 1.6f, true);

        if (!input.grounded)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
    }
}
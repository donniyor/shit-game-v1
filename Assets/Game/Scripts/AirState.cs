using UnityEngine;

public class AirState : State
{
    public AnimationClip anim;
    public float jumpSpeed;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        Debug.Log($"[AirState]->grounded={input.grounded} | player={input.name}");
        float time = Helpers.Map(body.linearVelocityY, jumpSpeed, -jumpSpeed, 0, 1, true);
        animator.Play("Jump", 0, time);
        animator.speed = 0;

        if (input.grounded)
        {
            Debug.Log("[AirState]->do() Is Complete");
            isComplete = true;
        }
        Debug.Log("[AirState]->do() End");
    }

    public override void Exit()
    {
    }
}
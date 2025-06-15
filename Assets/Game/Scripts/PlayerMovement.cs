using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AirState airState;
    public IdleState idleState;
    public RunState runState;
    public KneelState kneelState;

    private State state;

    public Animator animator;

    public bool grounded { get; private set; }
    public float xInput { get; private set; }
    public float yInput { get; private set; }
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public float maxXSpeed;
    [Range(0f, 1f)]
    public float groundDecay;
    public float acceleration;

    void Start()
    {
        idleState.Setup(body, animator, this);
        runState.Setup(body, animator, this);
        airState.Setup(body, animator, this);
        kneelState.Setup(body, animator, this);

        state = idleState;
    }

    void Update()
    {
        CheckInput();
        HandleJumpInput();

        SelectState();

        state.Do();
    }

    private void SelectState()
    {
        State oldState = state;

        if (grounded)
        {
            if (yInput < 0)
            {
                state = kneelState;
            }
            else if (xInput == 0)
            {
                state = idleState;
            }
            else
            {
                state = runState;
            }
        }
        else
        {
            state = airState;
        }

        if (oldState != state || oldState.isComplete)
        {
            oldState.Exit();
            state.Inisialise();
            state.Enter();
        }
    }

    public void FixedUpdate()
    {
        CheckGrounded();
        ApplyFriction();
        HandleXMovement();
    }

    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    private void CheckGrounded()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    private void HandleXMovement()
    {
        if (Mathf.Abs(xInput) < 0.01f)
        {
            return;
        }

        float increment = xInput * acceleration;
        float newSpeed = Mathf.Clamp(body.linearVelocityX + increment, -maxXSpeed, maxXSpeed);
        body.linearVelocityX = newSpeed;

        FaceInput();
    }

    private void FaceInput()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.linearVelocityY = airState.jumpSpeed;
        }
    }

    private void ApplyFriction()
    {
        if (grounded && 0 == xInput && body.linearVelocityY <= 0)
        {
            body.linearVelocityX *= groundDecay;
            body.linearVelocityY *= groundDecay;
        }
    }
}

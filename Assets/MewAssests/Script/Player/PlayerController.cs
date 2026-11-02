using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Controller
{
    private Rigidbody rb;
    private InputAction moveAction;
    public Movement move;
    public PushInteract push;
    void Awake()
    {
        
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (move.MoveDirection.sqrMagnitude > 0.01f && !push.IsHold)
        {
            Vector3 movementDirection = new Vector3(move.MoveDirection.x, 0, move.MoveDirection.z);

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, move.RotationSpeed * move.RotationSmoothSpeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        if (move.IsWalking)
        {
            move.MovePower = move.WalkSpeed * (push.IsHold? 0.5f : (move.IsRunning? move.RunMultipiler : 1));

            Vector3 velocity = rb.linearVelocity;
            velocity.x = move.MovePower * move.MoveDirection.x;
            velocity.z = move.MovePower * move.MoveDirection.z;
            
            rb.linearVelocity = velocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            move.MoveDirection = new Vector3(0, rb.linearVelocity.y, 0);
        }

        if (!move.IsJumping && !move.IsGrounded && move.CanJump && !push.IsHold)
        {
            move.CanJump = false;
            move.IsJumping = true;
            rb.AddForce(move.JumpPower * Vector3.up, ForceMode.Impulse);
        }
    }
    public void Move()
    {
        move.MoveDirection = moveAction.ReadValue<Vector3>();
    }
}

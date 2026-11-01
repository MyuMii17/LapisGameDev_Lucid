using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Controller
{
    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 currentRotatiion;
    private RaycastHit hit;
    public Movement move;
    public PushInteract push;
    public ClimbInteract climb;
    void Awake()
    {
        climb.ClimbLayer = LayerMask.GetMask("Edge");
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (move.MoveDirection.sqrMagnitude > 0.01f && !push.IsHold && !climb.IsHold)
        {
            Vector3 movementDirection = new Vector3(move.MoveDirection.x, 0, move.MoveDirection.z);

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, move.RotationSpeed * move.RotationSmoothSpeed * Time.deltaTime);
        }
        
    }
    void FixedUpdate()
    {
        // Vector3 position = transform.position;
        // position.y += climb.RayHeight;

        // Debug.DrawRay(position , move.MoveDirection * climb.RayRange , Color.red);
        // if (Physics.Raycast(position, move.MoveDirection, out RaycastHit hit, climb.RayRange, climb.ClimbLayer) && climb.IsHold)
        // {
        //     Vector3 wallDirection = -hit.normal;
        //     wallDirection.y = 0f;

        //     transform.rotation = Quaternion.LookRotation(wallDirection);
        //     climb.WallDirection = wallDirection;
        // }
        if (move.IsWalking)
        {
            move.MovePower = move.WalkSpeed * (push.IsHold || climb.IsHold? 0.5f : (move.IsRunning? move.RunMultipiler : 1));

            Vector3 velocity = rb.linearVelocity;
            velocity.x = move.MovePower * move.MoveDirection.x;
            velocity.z = move.MovePower * move.MoveDirection.z;
            
            rb.linearVelocity = velocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        if (!move.IsJumping && !move.IsGrounded && move.CanJump && !push.IsHold)
        {
            move.CanJump = false;
            move.IsJumping = true;
            rb.linearVelocity = Vector3.zero;
            move.TotalJumpPower =  move.JumpPower * (move.IsWalljump? move.WallJumpMultiplier : 1);
            rb.AddForce(move.TotalJumpPower * Vector3.up, ForceMode.Impulse);
        }
    }
    public void Move()
    {
        move.MoveDirection = moveAction.ReadValue<Vector3>();

        if(!climb.IsHold) return;

        if(climb.WallDirection.x > climb.WallDirection.z)
        {
            move.MoveDirection.x = 0f;
        }
        else if(climb.WallDirection.z > climb.WallDirection.x)
        {
            move.MoveDirection.z = 0f;
        }
    }

    public void ResetVelocity()
    { 
        rb.linearVelocity = Vector3.zero;
    }
}

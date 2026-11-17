using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Controller
{
    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 currentRotation;
    private CapsuleCollider collider;
    [SerializeField]private AnimationClip walkClip;
    private Animator animator;
    private RaycastHit hit;
    public Movement move;
    public PushInteract push;
    public ClimbInteract climb;
    public PickInteract pick;
    public ObjectInteract interact;
    private Coroutine slideCoroutine;
    public float slideInterval;
    public float slideTime;
    void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        move.CanMove = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        move.CurruntCrouchSizeY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Animation();

        if (move.MoveDirection.sqrMagnitude > 0.01f && !push.IsHold && !climb.IsHold && !move.IsSliding)
        {
            Vector3 movementDirection = new Vector3(move.MoveDirection.x, 0, move.MoveDirection.z);

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, move.RotationSpeed * move.RotationSmoothSpeed * Time.deltaTime);
        }

        if (push.IsHold)
        {
            collider.radius = 1;
        }
        else if (move.IsCrouching)
        {
            if (move.IsWalking)
            {
                collider.height = 2.25f;
                collider.radius = 1.2f;
                collider.center = new Vector3(collider.center.x, 1.2f, collider.center.z);
            }
            else
            {
                collider.height = 3;
                collider.radius = 1;
                collider.center = new Vector3(collider.center.x, 1.5f, collider.center.z);
            }
        }
        else
        {
            collider.radius = 0.5f;
            collider.height = 4.05f;
            collider.center = new Vector3(collider.center.x, 2, collider.center.z);
        }

        if (move.IsSliding)
        {
            if(slideCoroutine != null) return;
            
            slideCoroutine = StartCoroutine(OnSliding());
        }
    }
    void FixedUpdate()
    {
        if(move.IsSliding && !move.OnSlide)
        {
            move.OnSlide = true;
            move.MovePower = move.WalkSpeed * move.SlideMultiplier;

            Vector3 velocity = rb.linearVelocity;
            velocity.x = move.MovePower * move.MoveDirection.x;
            velocity.z = move.MovePower * move.MoveDirection.z;
            
            rb.linearVelocity = velocity;
        }
        if (move.IsWalking && !move.IsSliding)
        {   
            move.MovePower = move.WalkSpeed * (push.IsHold || climb.IsHold || (move.IsCrouching && !move.IsSliding)? 0.5f : 
            (move.IsRunning? move.RunMultiplier : 1));

            Vector3 velocity = rb.linearVelocity;

            if (climb.IsHold && climb.IsUp)
            {
                velocity.x = 0;
                velocity.z = 0;
                if(move.MoveDirection.z != 0)
                {
                    velocity.y = move.MovePower * move.MoveDirection.z;
                }

                rb.linearVelocity = velocity;
            }
            else
            {
                velocity.x = move.MovePower * move.MoveDirection.x;
                velocity.z = move.MovePower * move.MoveDirection.z;
                
                rb.linearVelocity = velocity;
            }
        }
        else if(!move.IsWalking && !move.IsSliding)
        {
            if (climb.IsUp)
            {
                rb.linearVelocity = Vector3.zero;
            }

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

        if (push.IsHold)
        {
            if(push.PushDirection.x != 0)
            {
                move.MoveDirection.z = 0;
                if (move.MoveDirection.x != 0)
                {
                    move.IsWalking = true;
                }
                return;
            }
            else if (push.PushDirection.z != 0)
            {
                move.MoveDirection.x = 0;
                if (move.MoveDirection.z != 0)
                {
                    move.IsWalking = true;
                }
                return;
            }
        }

        if (climb.IsHold && !climb.IsUp)
        {

            if(climb.WallDirection.x != 0)
            {
                move.MoveDirection.x = 0f;
                if (move.MoveDirection.z != 0)
                {
                    move.IsWalking = true;
                }
                return;
            }
            else if(climb.WallDirection.z != 0)
            {
                move.MoveDirection.z = 0f;
                if (move.MoveDirection.x != 0)
                {
                    move.IsWalking = true;
                }
                return;
            }
        }
        else if (climb.IsHold && climb.IsUp)
        {
            move.MoveDirection.x = 0f;
            if (move.MoveDirection.z != 0)
            {
                move.IsWalking = true;
            }
        }

        move.IsWalking = true;
    }
    IEnumerator OnSliding()
    {
        move.CanSlide = false;
        
        yield return new WaitForSeconds(slideTime);
        move.IsSliding = false;
        move.OnSlide = false;

        yield return new WaitForSeconds(slideInterval);
        move.CanSlide = true;
        slideCoroutine = null;
    }

    public void ResetVelocity()
    { 
        rb.linearVelocity = Vector3.zero;
    }
    public void Animation()
    {
        animator.SetBool("IsMoving", move.IsWalking? true : false);
        animator.SetBool("IsWalking", move.IsWalking && !move.IsJumping && !push.IsHold && !climb.IsHold? true : false);
        animator.SetBool("IsRunning",  move.IsRunning && !move.IsJumping && !push.IsHold && !climb.IsHold? true : false);
        animator.SetBool("IsJumping",  move.IsJumping && !push.IsHold || !move.IsGrounded? true : false);
        animator.SetBool("IsPush",  push.IsHold? true : false);
        animator.SetBool("IsClimbing",  climb.IsHold? true : false);
        animator.SetBool("IsCrouch",  move.IsCrouching? true : false);
        animator.SetBool("IsSliding",  move.IsSliding? true : false);
    }

}

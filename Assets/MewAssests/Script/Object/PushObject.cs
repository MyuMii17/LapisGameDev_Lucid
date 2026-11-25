using System.Data.Common;
using UnityEngine;

public class PushObject : InteractionObject
{
    protected PlayerPush playerPush;
    protected Rigidbody rb;
    protected Vector3 startTranform;
    public PullObject pull;
    private Animator animator;
    public Animator targetAnimator;
    public bool isTrigger;
    public bool hasTriggerAnim;
    public string animationTriggerName;
    public float maxMass;
    public float minMass;
    public bool isReset;
    public bool isGrounded;
    public bool canPush;
    public bool hasClimb;
    public bool isInvert;
    public bool isAtLimit;
    private ClimbObject climbObject;
    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        maxMass = rb.mass;
        startTranform = transform.position;
        if (hasTriggerAnim)
        {
            animator = GetComponentInParent<Animator>();
        }
    }
    protected override void Start()
    {
        base.Start();
        
        if (hasClimb)
        {
            climbObject = gameObject.transform.parent.GetComponentInChildren<ClimbObject>();
            if(climbObject != null)
            {
                climbObject.canClimb = false;
            }
        }
    }

    void Update()
    {
        if(playerPush == null || !canPush || !pull.CanPullOnly || pull.IsEndPull) return;

        if(playerPush.isInteract == true && isGrounded && pull.CanPullOnly && playerController.push.PullTime < pull.PullInterval)
        {
            playerController.move.CanMove = false;
            playerController.push.IsPull = true;
            playerController.push.PullInterval = pull.PullInterval;
        }
        else if(!isGrounded || playerPush.isInteract == false || playerController.push.PullTime >= pull.PullInterval)
        {
            if(playerController.push.PullTime >= pull.PullInterval)
            {
                pull.IsEndPull = true;
                //runAnimation
                if (hasTriggerAnim)
                {
                    if (isTrigger)
                    {
                        animator.SetTrigger(animationTriggerName);
                        if(targetAnimator != null) targetAnimator.SetTrigger(animationTriggerName);
                    }
                    else
                    {
                        animator.SetBool(animationTriggerName, true);
                        if(targetAnimator != null) targetAnimator.SetBool(animationTriggerName, true);
                    }
                }
                if(cutscene.HasCutscene)
                {
                    cutsceneManager.PlayerCutScene(cutscene.CutsceneNumber);
                }

                if (pull.CanResetPull)
                {
                    pull.IsEndPull = false;
                }
            }
            playerController.move.MoveDirection = playerController.push.PushDirection;
            playerController.push.PullTime = 0;
            
            getInteract = false;
            playerPush.isInteract = false;
            playerController.push.IsHold = false;
            playerController.move.CanMove = true;
            playerController.push.IsPull = false; 

            playerPush = null;
            playerController = null;
        }
    }
    void FixedUpdate()
    {
        if(playerPush == null || !canPush || pull.CanPullOnly) return;

        if(playerPush.isInteract == true && isGrounded)
        {
            if (isAtLimit)
            {
                playerController.push.IsAtLimit = true;
                if((!isInvert && playerController.move.MoveDirection != playerController.push.PushDirection) || (isInvert && playerController.move.MoveDirection == playerController.push.PushDirection))
                {
                    rb.linearVelocity = Vector3.zero;
                    return;
                }
            }
            // rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            Vector3 direction = playerController.move.MoveDirection;
            direction.y = 0;
            rb.linearVelocity = playerController.move.IsWalking? (direction * (playerController.move.MovePower + 0.25f)) : Vector3.zero;
            if (hasClimb)
            {
                if(transform.position != startTranform)
                {
                    climbObject.canClimb = true;
                }
                else
                {
                    climbObject.canClimb = false;
                }
            }
        }
        else if(!isGrounded || (playerPush.isInteract == false && !isReset))
        {
            if(transform.position == startTranform)
            {
                climbObject.canClimb = false;
            }

            // rb.constraints = RigidbodyConstraints.FreezeRotation;
            playerController.move.MoveDirection = playerController.push.PushDirection;
            playerPush.isInteract = false;
            playerController.push.IsHold = false;
            playerController.move.CanMove = true;
            playerController.push.IsAtLimit = false;
            playerController.push.IsInvert = false;

            rb.linearVelocity = Vector3.zero;
            rb.mass = maxMass;
            isReset = true;

            playerPush = null;
            playerController = null;
        }
    }
    public override void GetInteract(GameObject interactor)
    {
        if(!isGrounded) return;

        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerPush = interactor.GetComponentInChildren<PlayerPush>();
        getInteract = playerController.push.IsHold;

        playerPush.isInteract = getInteract;
        playerController.push.IsInvert = isInvert;
        isReset = false;

        if (isInvert)
        {
            Vector3 current = playerController.push.PushDirection;
            current = new Vector3(current.z, current.y, current.x);
            playerController.push.PushDirection = current;
        }

        if(getInteract == false || pull.CanPullOnly) return;
        rb.mass = minMass;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PushInvWall"))
        {
            isAtLimit = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PushInvWall"))
        {
            isAtLimit = false;
            playerController.push.IsAtLimit = false;
        }
    }
}

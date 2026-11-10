using System.Data.Common;
using UnityEditor.Build.Player;
using UnityEngine;

public class PushObject : InteractionObject
{
    protected PlayerPush playerPush;
    protected Rigidbody rb;
    public Vector3 startTranform;
    public float maxMass;
    public float minMass;
    public bool isReset;
    public bool isGrounded;
    public bool canPush;
    public bool hasClimb;
    private ClimbObject climbObject;
    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        maxMass = rb.mass;
        startTranform = transform.position;
        canPush = true;
    }
    void Start()
    {
        if (hasClimb)
        {
            climbObject = gameObject.transform.parent.GetComponentInChildren<ClimbObject>();
            if(climbObject != null)
            {
                climbObject.canClimb = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(playerPush == null || !canPush) return;

        if(playerPush.isInteract == true && isGrounded)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            Vector3 direction = playerController.move.MoveDirection;
            direction.y = 0;
            rb.linearVelocity = playerController.move.IsWalking? (direction * (playerController.move.MovePower + 0.25f)) : Vector3.zero;
            if(transform.position != startTranform)
            {
                climbObject.canClimb = true;
            }
            else
            {
                climbObject.canClimb = false;
            }
        }
        else if(!isGrounded || (playerPush.isInteract == false && !isReset))
        {
            if(transform.position == startTranform)
            {
                climbObject.canClimb = false;
            }

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            playerPush.isInteract = false;
            playerController.push.IsHold = false;
            playerController.move.CanMove = true;

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
        isReset = false;

        if(getInteract == false) return;

        rb.mass = minMass;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PushInvWall"))
        {
            canPush = false;
            playerController.move.CanMove = false;
        }
    }
}

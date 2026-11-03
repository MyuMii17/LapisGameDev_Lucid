using UnityEditor.Build.Player;
using UnityEngine;

public class PushObject : InteractionObject
{
    protected PlayerPush playerPush;
    protected Rigidbody rb;
    protected float maxMass;
    protected float minMass;
    public bool isReset;
    public bool isGrounded;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxMass = rb.mass;
    }

    void FixedUpdate()
    {
        if(playerPush == null) return;

        if(playerPush.isInteract == true && isGrounded)
        {
            rb.linearVelocity = playerController.move.IsWalking? (playerController.move.MoveDirection * (playerController.move.MovePower + 0.1f)) : Vector3.zero;
        }
        else if(!isGrounded || (playerPush.isInteract == false && !isReset))
        {
            playerPush.isInteract = false;
            playerController.push.IsHold = false;

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
}

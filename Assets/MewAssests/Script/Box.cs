using UnityEditor.Build.Player;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    private PlayerController playerController;
    private PlayerPushAndPull player;
    private Rigidbody rb;
    public float maxMass;
    public float minMass;
    public bool getInteract;
    public bool isReset;
    public bool isGrounded;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxMass = rb.mass;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player == null || playerController == null) return;

        if(player.isInteract == true && isGrounded)
        {
            rb.linearVelocity = playerController.move.IsWalking? (playerController.move.MoveDirection * (playerController.move.MovePower + 0.1f)) : Vector3.zero;
        }
        else if(!isGrounded || (player.isInteract == false && !isReset))
        {
            player.isInteract = false;
            playerController.push.IsHold = false;

            rb.linearVelocity = Vector3.zero;
            rb.mass = maxMass;
            isReset = true;

            player = null;
            playerController = null;
        }
    }
    public void OnInteract(GameObject interactor)
    {
        if(!isGrounded) return;

        playerController = interactor.GetComponentInParent<PlayerController>();
        getInteract = playerController.push.IsHold;

        player = interactor.GetComponentInParent<PlayerPushAndPull>();
        player.isInteract = getInteract;
        isReset = false;

        if(getInteract == false) return;

        rb.mass = minMass;
    }
}

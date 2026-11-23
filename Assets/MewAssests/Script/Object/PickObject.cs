using UnityEngine;

public class PickObject : InteractionObject
{
    private GameObject main;
    protected PlayerPick playerPick;
    protected Rigidbody rb;
    private BoxCollider boxCollider;
    public float thorwPower;
    public bool isThrow;
    public bool canThrow;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        main = gameObject.transform.parent.gameObject;
        boxCollider = main.GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        if(playerPick == null) return;

        if(playerPick.isInteract == true)
        {
            main.transform.position = playerPick.handPosition.position;
            rb.useGravity = false;
            boxCollider.isTrigger = true;
        }
        else if(playerPick.isInteract == false)
        {
            rb.useGravity = true;
            boxCollider.isTrigger = false;

            if(canThrow && !isThrow)
            {
                Vector3 direction = playerController.move.MoveDirection;
                direction.y += 2;

                rb.AddForce(thorwPower * direction, ForceMode.Impulse);
                isThrow = true;
            }
        }
    }
    public override void GetInteract(GameObject interactor)
    {
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerPick = interactor.GetComponentInChildren<PlayerPick>();
        getInteract = playerController.pick.IsHold;

        playerPick.isInteract = getInteract;

        if (playerController.pick.IsHold && canThrow)
        {
            isThrow = false;
        }
    }
}

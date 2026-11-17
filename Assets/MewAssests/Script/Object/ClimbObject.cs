using UnityEngine;

public class ClimbObject : InteractionObject
{
    private PlayerClimb playerClimb;
    private Rigidbody playerRb;
    public float rangeY;
    public bool isUp;
    public bool isReadyUp;
    public bool canClimb;
    void Awake()
    {
        canClimb = true;
    }

    void Update()
    {
        if(playerClimb == null || !canClimb) return;

        if(playerClimb.isInteract == true && !playerController.move.IsJumping)
        {
            if (!isUp)
            {
                player.transform.position = new Vector3(player.transform.position.x, transform.position.y - rangeY, player.transform.position.z);
            }
            else if(isUp && !isReadyUp)
            {
                isReadyUp = true;
                player.transform.position = new Vector3(player.transform.position.x, transform.position.y - rangeY, player.transform.position.z);
            }
        }
        else if(playerClimb.isInteract == false || playerController.move.IsJumping)
        {
            playerClimb.isInteract = false;
            playerController.climb.IsHold = false;
            playerController.climb.IsUp = false;
            playerRb.useGravity = true;

            isReadyUp = false;
            player = null;
            playerClimb = null;
            playerController = null;
            playerRb = null;
        }
    }
    public override void GetInteract(GameObject interactor)
    {
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerClimb = interactor.GetComponentInChildren<PlayerClimb>();
        playerRb = interactor.GetComponent<Rigidbody>();
        playerRb.useGravity = false;

        getInteract = playerController.climb.IsHold;
        playerClimb.isInteract = getInteract;
        playerController.climb.IsUp = isUp;
    }
}

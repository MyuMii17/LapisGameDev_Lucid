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
                if(transform.position.y - rangeY < player.transform.position.y)
                {
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                    return;
                }
                else
                {
                    player.transform.position = new Vector3(player.transform.position.x, transform.position.y - rangeY, player.transform.position.z);
                    return;
                }
            }
        }
        else if(playerClimb.isInteract == false || playerController.move.IsJumping)
        {
            playerController.move.MoveDirection = playerController.climb.WallDirection;
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

        getInteract = playerController.climb.IsHold;
        playerClimb.isInteract = getInteract;
        playerRb.useGravity = !getInteract;
        playerController.climb.IsUp = isUp;
    }
}

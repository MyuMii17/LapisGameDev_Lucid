using UnityEngine;

public class ClimbObject : InteractionObject
{
    private PlayerClimb playerClimb;

    void Update()
    {
        if(playerClimb == null) return;

        if(playerClimb.isInteract == true && !playerController.move.IsJumping)
        {
            player.transform.position = new Vector3(player.transform.position.x, transform.position.y + (transform.position.y / 2), player.transform.position.z);
        }
        else if(playerClimb.isInteract == false || playerController.move.IsJumping)
        {
            playerClimb.isInteract = false;
            playerController.climb.IsHold = false;

            player = null;
            playerClimb = null;
            playerController = null;
        }
    }
    public override void GetInteract(GameObject interactor)
    {
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerClimb = interactor.GetComponentInChildren<PlayerClimb>();

        getInteract = playerController.climb.IsHold;
        playerClimb.isInteract = getInteract;
    }
}

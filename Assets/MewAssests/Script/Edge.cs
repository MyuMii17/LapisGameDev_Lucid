using UnityEngine;

public class Edge : MonoBehaviour, IInteractable
{
    private GameObject player;
    private PlayerController playerController;
    private PlayerClimb playerClimb;
    public bool getInteract;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerClimb == null || playerController == null) return;

        if(playerClimb.isInteract == true && !playerController.move.IsJumping)
        {
            player.transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
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
    public void OnInteract(GameObject interactor)
    {
        playerController = interactor.GetComponentInParent<PlayerController>();
        getInteract = playerController.climb.IsHold;

        playerClimb = interactor.GetComponentInParent<PlayerClimb>();
        playerClimb.isInteract = getInteract;
        
        player = interactor.transform.parent.gameObject;
    }
}

using System.Collections;
using UnityEngine;

public class ClimbObject : InteractionObject
{
    public GameObject parent;
    private Animator animator;
    private PlayerClimb playerClimb;
    private Rigidbody playerRb;
    public bool isSetPosition;
    public float rangeX;
    public float rangeY;
    public float rangeZ;
    public bool isUp;
    public bool isReadyUp;
    public bool canClimb;
    public bool hasAnimation;
    public bool isStartAnimation;
    public string animationName;
    void Awake()
    {
        canClimb = true;
        if (hasAnimation)
        {
            animator = parent.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if(playerClimb == null || !canClimb) return;

        if(playerClimb.isInteract == true && !playerController.move.IsJumping)
        {
            if (hasAnimation)
            {
                player.transform.position = new Vector3(transform.position.x - rangeX, transform.position.y - rangeY, transform.position.z - rangeZ);

                if (!isStartAnimation)
                {
                    isStartAnimation = true;
                    StartCoroutine(OnPlayAnimation());
                }
                return;
            }
            if (!isUp)
            {
                if (!isSetPosition)
                {
                    isSetPosition = true;
                    player.transform.position = new Vector3(transform.position.x - rangeX, transform.position.y - rangeY, transform.position.z - rangeZ);
                }

                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
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
            isSetPosition = false;
            player = null;
            playerClimb = null;
            playerController = null;
            playerRb = null;
        }
    }
    IEnumerator OnPlayAnimation()
    {
        animator.SetTrigger("Start");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        animator.enabled = false;

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

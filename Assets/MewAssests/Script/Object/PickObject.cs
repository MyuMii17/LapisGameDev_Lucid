using UnityEditor.Build.Player;
using UnityEngine;

public class PickObject : InteractionObject
{
    protected PlayerPick playerPick;
    protected Rigidbody rb;
    public bool canThrow;
    public bool isGrounded;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(playerPick == null) return;

        
    }
    public override void GetInteract(GameObject interactor)
    {
        if(!isGrounded) return;

        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerPick = interactor.GetComponentInChildren<PlayerPick>();
        getInteract = playerController.push.IsHold;

        playerPick.isInteract = getInteract;
    }
}

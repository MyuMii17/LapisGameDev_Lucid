using UnityEngine;

public abstract class InteractionObject : MonoBehaviour, IInteractable
{
    protected GameObject player;
    protected PlayerController playerController;
    protected UiManager uIManager;
    protected CutsceneManager cutsceneManager;
    protected InventoryManager inventoryManager;
    protected int playerLayer;
    public CutsceneSetting cutscene;
    public bool getInteract;
    protected virtual void Start()
    {
        uIManager = UiManager.GetInstance();
        inventoryManager = InventoryManager.GetInstance();
        playerController = PlayerController.GetInstance();
        cutsceneManager = CutsceneManager.GetInstance();

        playerLayer = LayerMask.GetMask("Player");
    }
    public virtual void Condition()
    {
        
    }
    public abstract void GetInteract(GameObject interactor);
}

using UnityEngine;

public class Item : InteractObject
{
    public string warningDialog;
    public ItemCondition condition;
    public bool isWarning;
    public bool canWarning;
    public override void Condition()
    {
        if (condition.HasCondition)
        {
            if (condition.CanIteract)
            {
                OnGet();
            }
            else
            {
                if(isWarning || !canWarning) return;
                isWarning = true;
                // uIManager.OnDialogue(warningDialog);
                return;
            }
        }
        else
        {
            OnGet();
        }
    }
    void Update()
    {
        InventoryManager inv = Physics2D.OverlapCircle(gameObject.transform.position, condition.CircleRadius, playerLayer)?.GetComponent<InventoryManager>();
        if (condition.CanDetect)
        {
            if (inv != null && condition.IsAdd == false)
            {
                condition.IsAdd = true;
                inv.GetItem(this, gameObject, condition.Count, condition.MaxStack);
            }
        }
    }
    public void OnGet()
    {
        condition.IsAdd = true;
        inventoryManager.GetItem(this, gameObject, condition.Count, condition.MaxStack);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.pink;
        Gizmos.DrawWireSphere(transform.position, condition.CircleRadius);
    }
    public override void GetInteract(GameObject interactor)
    {
        base.GetInteract(interactor);
    }
}

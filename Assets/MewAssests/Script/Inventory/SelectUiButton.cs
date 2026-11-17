using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectUiButton : MonoBehaviour, IPointerClickHandler
{
    public int index;
    public GameObject selectUi;
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = InventoryManager.GetInstance();
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }

    }

    public void LeftClick()
    {
        inventoryManager.DropItem(index);
        selectUi.SetActive(false);
    }
}
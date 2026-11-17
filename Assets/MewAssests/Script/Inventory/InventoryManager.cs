using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSlots> itemSlots = new List<ItemSlots>();
    public List<Image> uis = new List<Image>();
    public Sprite noneSprite;
    public GameObject selectUi;
    private RectTransform selectPos;
    public bool isSelect;
    public int slotSize;
    private int count;
    private int overCount;
    private PlayerController playerController;
    private UiManager uIManager;
    private InputAction attackAction;
    private static InventoryManager Instance = null;
    public static InventoryManager GetInstance()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        // DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        selectPos = selectUi.GetComponent<RectTransform>();

        playerController = PlayerController.GetInstance();
        uIManager = UiManager.GetInstance();

        attackAction = InputSystem.actions.FindAction("Attack");
        
    }
    void Update()
    {
        
    }
    public void GetItem(Item target, GameObject targetObject, int count, int max)
    {
        Item targetItem = (Item)target;
        if(itemSlots.Count > slotSize) return;

        if(itemSlots.Count <= slotSize)
        {
            foreach(ItemSlots slot in itemSlots)
            {
                overCount = 0;
                int currentCount = count;
                if(slot.self == targetObject)
                {
                    if(slot.count == slot.maxStack) return;
                    slot.count += count;
                    if(slot.count > slot.maxStack)
                    {
                        overCount = slot.count - slot.maxStack;
                        slot.count = slot.maxStack;
                        currentCount = count - overCount;
                        targetItem.condition.Count = overCount;
                        UpdateInv();
                        // uIManager.OnGetItem(slot.item.objectName, currentCount);
                    }
                    if(overCount == 0)
                    {
                        Destroy(targetItem.gameObject);
                        UpdateInv();
                        // uIManager.OnGetItem(slot.item.objectName, count);
                        return;
                    }
                    return;
                }
            }
            overCount = 0;
            if(itemSlots.Count >= slotSize) return;
            ItemSlots itemSlot = new ItemSlots();
            itemSlot.self = targetObject;
            itemSlot.count = count;
            itemSlot.maxStack = max;
            // uIManager.OnGetItem(itemSlot.item.objectName, count);
            if(itemSlot.count > itemSlot.maxStack)
            {
                overCount = itemSlot.count - itemSlot.maxStack;
                itemSlot.count = itemSlot.maxStack;
                targetItem.condition.Count = overCount;
            }
            if(overCount == 0)
            {
                Destroy(targetItem.gameObject);
            }
            itemSlots.Add(itemSlot);
            UpdateInv();
            return;
        }

    }

    public void RemoveItem(int index)
    {
        if(index > itemSlots.Count - 1) return;
        DropFunction(index);
    }

    public void DropItem(int index)
    {
        if(index > itemSlots.Count - 1) return;
        GameObject prefeb = itemSlots[index].self;

        float posX = playerController.transform.position.x + (playerController.move.MoveDirection.x * 1.1f);
        float posY = playerController.transform.position.y + (playerController.move.MoveDirection.y * 1.1f);
        GameObject item = Instantiate(prefeb, new Vector2(posX, posY), Quaternion.identity);
        Item i = item.GetComponent<Item>();
        i.condition.Count = itemSlots[index].count;
        i.condition.MaxStack = itemSlots[index].maxStack;
        i.condition.IsAdd = false;

        DropFunction(index);
    }

    void DropFunction(int index)
    {
        if(itemSlots.Count > 1 && index != itemSlots.Count - 1)
        {
            itemSlots[index] = itemSlots[index + 1];
            itemSlots[index + 1] = null;
            UpdateInv();
            itemSlots.RemoveAll(i => i == null);
            UpdateInv();
        }
        else
        {
            itemSlots[index] = null;
            UpdateInv();
            itemSlots.RemoveAll(i => i == null);
        }
    }

    public void UpdateInv()
    {
        int index = 0;
        count = itemSlots.Count;
        if(itemSlots != null)
        {
            foreach(Image i in uis)
            {
                TMP_Text text = i.transform.GetChild(0).GetComponent<TMP_Text>();
                Image image = i.transform.GetChild(1).GetComponent<Image>();
                GameObject countText = i.transform.GetChild(0).gameObject;
                GameObject itemImage = i.transform.GetChild(1).gameObject;
                if(itemSlots[index] != null)
                {
                    countText.SetActive(true);
                    itemImage.SetActive(true);
                    text.text = itemSlots[index].count.ToString();
                    image.sprite = itemSlots[index].sprite;
                }
                else
                {
                    countText.SetActive(false);
                    itemImage.SetActive(false);
                    text.text = "0";
                    image.sprite = noneSprite;
                }

                if(index == count - 1) return;
                index++;
            }
        }
        itemSlots.RemoveAll(i => i== null);
    }
}

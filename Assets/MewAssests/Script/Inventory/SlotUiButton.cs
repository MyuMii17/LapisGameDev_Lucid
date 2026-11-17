using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SlotUiButton : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    public GameObject selectUi;
    private RectTransform selectPos;
    public SelectUiButton selectSetting;
    public bool isSelect;

    void Start()
    {
        selectPos = selectUi.GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftCLick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClik();
        }
    }

    public void LeftCLick()
    {
        selectUi.SetActive(false);
    }
    public void RightClik()
    {
        selectUi.SetActive(true);

        Vector2 screenPos = Vector2.zero;
        screenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.red, 5f);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        selectPos.position = screenPos;

        selectSetting.index = slotIndex;
    }
}
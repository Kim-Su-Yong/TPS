using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour ,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;
    private Transform inventroyTr;
    public static GameObject draggingItem = null;
    [SerializeField]
    private Transform itemListTr;
    [SerializeField]
    private CanvasGroup canvasGroup;
    void Start()
    {
        itemTr = GetComponent<Transform>();
        inventroyTr = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    //드래그 이벤트 
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {                   //드래그를 시작했을때 부모가 상위 인벤토리 오브젝트로 한다.
        this.transform.SetParent(inventroyTr);
        //드래그가 시작되면 드래그 되는 아이템 정보를 저장함ㄴ
        draggingItem = this.gameObject;
        //드래그가 시작되면 이벤트를 받지 않는다.
        canvasGroup.blocksRaycasts = false;
    }
    //드래그가 종료했을때 한 번 호출되는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        //드래그가 종료되면 드래그 아이템을 null 변경
        canvasGroup.blocksRaycasts = true;
        if(itemTr.parent == inventroyTr)
        {
            itemTr.SetParent(itemListTr.transform);
            GameManager.gameManager.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
    }
}

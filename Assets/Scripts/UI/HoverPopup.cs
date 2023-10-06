using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textbox;
    // Start is called before the first frame update
    void Start()
    {
        textbox.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textbox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textbox.SetActive(false);
    }
}

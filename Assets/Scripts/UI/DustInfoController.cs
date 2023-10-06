using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustInfoController : MonoBehaviour
{
    public GameObject character;

    void Start()
    {
        Debug.Log("��ʼ");
        HidePanel();
    }

    void Update()
    {
        // ����Ƿ����˽�ɫ
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("���");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == character)
                {
                    ShowPanel();
                    Debug.Log("��⵽����");
                }
                else
                {
                    HidePanel();
                }
            }
            else
            {
                HidePanel();
            }
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}

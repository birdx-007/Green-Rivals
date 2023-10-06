using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustInfoController : MonoBehaviour
{
    public GameObject character;

    void Start()
    {
        Debug.Log("开始");
        HidePanel();
    }

    void Update()
    {
        // 检测是否点击了角色
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("鼠标");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == character)
                {
                    ShowPanel();
                    Debug.Log("检测到物体");
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

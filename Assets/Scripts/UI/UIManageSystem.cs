using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class UIManageSystem : MonoBehaviour
{
    public float UIAnimationTime = 0.25f;
    public GameObject tutorial;
    private void Awake()
    {
        if(LevelDatabase.currentLevel == 0)
        {
            OpenMenu(tutorial);
        }
    }
    public void OpenMenu(GameObject menuObject)
    {
        var scale = menuObject.transform.localScale;
        menuObject.transform.localScale = Vector3.zero;
        menuObject.SetActive(true);
        menuObject.transform.DOScale(scale,UIAnimationTime);
    }
    public async void CloseMenu(GameObject menuObject)
    {
        var scale = menuObject.transform.localScale;
        await menuObject.transform.DOScale(0, UIAnimationTime).AsyncWaitForCompletion();
        menuObject.SetActive(false);
        menuObject.transform.localScale = scale;
    }
}

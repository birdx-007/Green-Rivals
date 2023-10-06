using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManageSystem : MonoBehaviour
{
    public GameObject book;
    public GameObject end;
    public GameObject roundStartTip;

    public void OpenMenu(GameObject menuObject)
    {
        menuObject.SetActive(true);
    }
    public void CloseMenu(GameObject menuObject)
    {
        menuObject.SetActive(false);
    }
    public void OpenTheEnd()
    {
        end.SetActive(true);
    }

    public void CloseTheEnd()
    {
        end.SetActive(false);
    }
    public void OpenTheBook()
    {
        book.SetActive(true);
    }
    public void ExitLastStep()
    {
        book.SetActive(false);
    }
    public void OpenTip()
    {
        roundStartTip.SetActive(true);

    }
    public void CloseTip()
    {
        roundStartTip.SetActive(false);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    public void Quit()
    {
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
    }

}

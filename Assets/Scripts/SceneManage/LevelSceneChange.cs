using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSceneChange : MonoBehaviour
{

    public void Exit()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    public void Next()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}

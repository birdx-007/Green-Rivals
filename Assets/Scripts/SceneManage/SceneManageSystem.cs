using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManageSystem : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
    public void Buttonattack()
    {
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Exit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
       #else
        Application.Quit();
       #endif
    }
}

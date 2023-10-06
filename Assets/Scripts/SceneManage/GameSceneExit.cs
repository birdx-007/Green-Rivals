using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSceneExit : MonoBehaviour
{
    public void Buttonattack()
    {
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
    }

}

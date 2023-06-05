using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System;



public class Newstart : MonoBehaviour
{
    public static void GoStartScene()
    {
        SceneManager.LoadScene(9);
    }
    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void Exit()
    {
        Application.Quit();
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class outgame2 : MonoBehaviour
{
    public void OnLoginButtonClick()
    {
        SceneManager.LoadScene(7);
    }
    public void LostButtonClick()
    {
        SceneManager.LoadScene(5);
    }

}


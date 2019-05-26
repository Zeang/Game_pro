using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class outgame : MonoBehaviour
{
    public void OnLoginButtonClick()
    {
        SceneManager.LoadScene(2);
    }

}


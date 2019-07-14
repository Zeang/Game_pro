using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class room : MonoBehaviour
{
    public void OnLoginButtonClick()
    {
        SceneManager.LoadScene(8);
    }

}

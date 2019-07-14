using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_exit : MonoBehaviour
{
 public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying=false;
    }
}

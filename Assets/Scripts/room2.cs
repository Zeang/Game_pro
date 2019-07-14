using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class room2 : MonoBehaviour
{
    public GameObject obj;
    public int cha_id;
    public void Start(){
        DontDestroyOnLoad(obj);
    }
    
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(9);
    }
    public void ChangeCharactor(){
        IEnumerable<Toggle> toggleGroup = GameObject.Find("Canvas/ToggleGroup").GetComponent<ToggleGroup>().ActiveToggles();
        foreach(Toggle t in toggleGroup){
             if(t.isOn){
                  switch(t.name){
                       case "cha1":
					        cha_id=0;
					        break;
                       case "cha2":
					        cha_id=1;
					        break;
                       case "cha3":
					        cha_id=2;
					        break;
				  }
				  break;
				  Debug.Log(cha_id);
			 }
        }
	}

}

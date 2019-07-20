using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tip : MonoBehaviour
{
    public static Vector3 vec3, pos;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void PointerDown()
    {
        vec3 = Input.mousePosition;//获取当前鼠标的位置
        pos = transform.GetComponent<RectTransform>().position;//获取自己所在的位置
    }

    public void Drag()
    {
        Vector3 off = Input.mousePosition - vec3;
        //此处Input.mousePosition指鼠标拖拽结束的新位置
        //减去刚才在按下时的位置，刚好就是鼠标拖拽的偏移量
        vec3 = Input.mousePosition;//刷新下鼠标拖拽结束的新位置，用于下次拖拽的计算
        pos = pos + off;//原来image所在的位置自然是要被偏移
        transform.GetComponent<RectTransform>().position = pos;//直接将自己刷新到新坐标
    }

    public void onShow()
    {
        gameObject.SetActive(true);
    }

    public void onOK()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(7);
    }

    public void onClose()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

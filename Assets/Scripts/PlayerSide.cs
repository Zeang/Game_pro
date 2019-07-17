using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSide : MonoBehaviour
{
    public int side = 0;
    public Color color1 = Color.white;
    public Color color2 = Color.red;
    private GameObject holo = null;
    // Start is called before the first frame update
    void Start()
    {
        holo = transform.Find("holo").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        holo.GetComponent<SpriteRenderer>().color = (side == 0 ? color1 : color2);
    }
}

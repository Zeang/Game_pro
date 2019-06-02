using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotater : MonoBehaviour {
    float shakeAmount = 0.1f;
    Vector3 first_pos;
    int count = 0;
	// Use this for initialization
	void Start () {
        first_pos = this.transform.localPosition;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (count % 10 == 0)
        {
            float newy = first_pos.y + Random.insideUnitSphere.y * shakeAmount;
            Vector3 pos=first_pos;
            pos.y = newy;
     
            transform.localPosition = pos;
        }
        count++;
	}
}

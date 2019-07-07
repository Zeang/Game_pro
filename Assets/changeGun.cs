using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGun : MonoBehaviour
{
    public Mesh[] otherMesh;
    private MeshFilter mf;
    private MeshRenderer mr;
    public Material []mt;
    int count=0;
    // Start is called before the first frame update
    void Start()
    {
        mf = this.GetComponent<MeshFilter>();
        mr = this.GetComponent<MeshRenderer>();

    }

    public int getWeapon()
    {
        return count;
    }




    // Update is called once per frame
    void Update()

    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            count = (count + 1) % 3;
            
        }
        if (count == 0)
        {
            mf.mesh = otherMesh[0];
            mr.material = mt[0];

        }
        else if (count == 1)
        {
            mf.mesh = otherMesh[1];
            mr.material = mt[1];
        }
        else if (count == 2)
        {
            mf.mesh = otherMesh[2];
            mr.material = mt[2];
        }

    }
}

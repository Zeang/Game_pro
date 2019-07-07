using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMesh : MonoBehaviour
{
    public Mesh []otherMesh;
    private SkinnedMeshRenderer smeshrender;
    int count = 0;
    Material[] m;
    
    // Start is called before the first frame update
    void Start()
    {
        smeshrender = this.GetComponent<SkinnedMeshRenderer>();
        m = smeshrender.sharedMaterials;
    }
    
       

        
    public int getCharacter()
    {
        return count;
    }

    // Update is called once per frame
    void Update()

    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            count=(count+1)%3;
        }
        if (count == 0)
        {
            smeshrender.sharedMesh = otherMesh[0];
            smeshrender.material = m[0];
        }
        else if (count == 1)
        {
            smeshrender.sharedMesh = otherMesh[1];
            smeshrender.material = m[1];
        }
        else if (count == 2)
        {
            smeshrender.sharedMesh = otherMesh[2];
            smeshrender.material = m[2];
        }

    }
}

using System.Collections;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    //[SerializeField] Material mat;
    [SerializeField] float timeAlive;
    [SerializeField] float spawnRate;
    [SerializeField] Material mat;
    MeshFilter[] meshFliters;

    public bool generate;
    float spawnT;

    private void Update()
    {
        if (generate && spawnT >= spawnRate)
        {
            spawnT = 0;
            SpawnTrail();
        }
        else if (generate)
        {
            spawnT += Time.deltaTime;
        }
    }

    void SpawnTrail()
    {
        if (meshFliters == null)
        {
            meshFliters = GetComponentsInChildren<MeshFilter>();
        }

        for (int i = 0; i < meshFliters.Length; i++)
        {
            GameObject gObj = new GameObject();
            gObj.transform.SetPositionAndRotation(meshFliters[i].transform.position, meshFliters[i].transform.rotation);
            gObj.transform.localScale = meshFliters[i].transform.lossyScale;


            MeshRenderer mr =  gObj.AddComponent<MeshRenderer>();            
            MeshFilter mf =  gObj.AddComponent<MeshFilter>();

            mf.mesh = meshFliters[i].mesh;

            Material[] mats = new Material[mr.materials.Length];

            for (int f = 0; f < mats.Length; f++)
            {
                mats[f] = mat;
            }

            mr.materials = mats;

            Destroy(gObj, timeAlive);
        }
    }
 
}

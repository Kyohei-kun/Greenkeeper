using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;

public class ObjData
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }

    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        //this.scale = scale;
        this.scale = new Vector3(0.13f,0.09f,0.09f);
        //this.rot = rot;
        this.rot = Quaternion.Euler(new Vector3(Random.Range(-10f, 10f), Random.Range(0f,180f), Random.Range(85f, 95f)));
    }
}

public class test_gpuspawn : MonoBehaviour
{
    // Start is called before the first frame update
    public int instances;
    public Vector3 maxPos;
    public Mesh objMesh;
    public Material objMat;

    private List<List<ObjData>> batches = new List<List<ObjData>>();

    void Start()
    {

        int batchIndexNum = 0;
        List<ObjData> currentBatch = new List<ObjData>();
        for(int i = 0; i< instances; i++)
        {
            AddObj(currentBatch, i);
            batchIndexNum++;
            if(batchIndexNum >= 1000)
            {
                batches.Add(currentBatch);
                currentBatch = BuildNewBatch();
                batchIndexNum = 0;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        RenderBatches();
    }

    private void AddObj(List<ObjData> currentBatch, int i)
    {
        Vector3 position = new Vector3(Random.Range(-maxPos.x, maxPos.x), 0, Random.Range(-maxPos.z, maxPos.z));
        currentBatch.Add(new ObjData(position, new Vector3(2, 2, 2), Quaternion.identity));
    }

    private List<ObjData> BuildNewBatch()
    {
        return new List<ObjData>();
    }

    private void RenderBatches()
    {
        foreach (var batch in batches)
        {
            RenderParams rp = new RenderParams(objMat);
            Graphics.RenderMeshInstanced(rp, objMesh, 0, batch.Select((a) => a.matrix).ToList());
        }
    }
}

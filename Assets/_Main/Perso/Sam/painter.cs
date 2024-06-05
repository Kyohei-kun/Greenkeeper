using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class painter : MonoBehaviour
{

    [SerializeField] Shader drawShader;

    [SerializeField] Transform playerTr;

    RenderTexture paintMap;
    Material currentMaterial, paintMaterial;
    RaycastHit hit;

    [SerializeField]
    [Range(1, 500)]
    float size;
    [SerializeField]
    [Range(0, 5)]
    float strength;
    //test
    // Start is called before the first frame update
    void Start()
    {
        paintMaterial = new Material(drawShader);
        paintMaterial.SetVector("_Color", Color.white);

        currentMaterial = GetComponent<MeshRenderer>().material;

        paintMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        currentMaterial.SetTexture("_PaintMap", paintMap);
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(playerTr.position, Vector3.down, out hit))
        {
            Debug.DrawRay(playerTr.position, Vector3.down);
            paintMaterial.SetVector("_Coordinates", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
            Debug.Log(hit.collider);
            Debug.Log("tex.x = " + hit.textureCoord.x + ", tex.y = " +  hit.textureCoord.y);
            paintMaterial.SetFloat("_Strength", strength);
            paintMaterial.SetFloat("_Size", size);
            RenderTexture temp = RenderTexture.GetTemporary(paintMap.width, paintMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(paintMap, temp);
            Graphics.Blit(temp, paintMap, paintMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}

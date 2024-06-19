using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class paintCircle : MonoBehaviour
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
    float tempSize = 1f;
    [SerializeField]
    float rate;

    [SerializeField]
    VisualEffect fx;

    bool painting;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            painting = true;
            fx.enabled = true;
        }

        if(painting && tempSize < size)
        {
            if (Physics.Raycast(playerTr.position, Vector3.down, out hit))
            {
                fx.SetFloat("Radius", tempSize/3f);
                paintMaterial.SetVector("_Coordinates", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                paintMaterial.SetFloat("_Strength", strength);
                tempSize += Time.deltaTime * rate;
                paintMaterial.SetFloat("_Size", tempSize);
                RenderTexture temp = RenderTexture.GetTemporary(paintMap.width, paintMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(paintMap, temp);
                Graphics.Blit(temp, paintMap, paintMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
        if(tempSize >= size)
        {
            fx.SetBool("ongoing", false);
        }

    }

    void Paint()
    {
        if (Physics.Raycast(playerTr.position, Vector3.down, out hit))
        {
            paintMaterial.SetVector("_Coordinates", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
            paintMaterial.SetFloat("_Strength", strength);
            paintMaterial.SetFloat("_Size", size);
            RenderTexture temp = RenderTexture.GetTemporary(paintMap.width, paintMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(paintMap, temp);
            Graphics.Blit(temp, paintMap, paintMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyController : MonoBehaviour
{
    public Renderer renderer;
    MaterialPropertyBlock block;

    public bool setBaseColor = true;
    public Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        if(renderer == null)
            renderer = GetComponent<Renderer>();

        block = new MaterialPropertyBlock();
        renderer.SetPropertyBlock(block);
    }

    private void OnDestroy()
    {
        block.Clear();
        block = null; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (setBaseColor)
        {
            block.SetColor("_BaseColor", baseColor); 
        }
        renderer.SetPropertyBlock(block);
    }
}

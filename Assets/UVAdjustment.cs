using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UVAdjustment : MonoBehaviour
{
    private Material originalMaterial; // Assign the original material in the Inspector
    public Vector2 tiling = new Vector2(1, 1); // Adjust tiling values as needed
    public Texture texture;
    private Material instanceMaterial;
    public Color color = Color.white;

#if UNITY_EDITOR
    void OnValidate()
    {
        UpdateMaterial();
    }
#endif

    void Start()
    {
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        if (originalMaterial != null)
        {
            // Create an instance of the original material
            instanceMaterial = new Material(originalMaterial);

            // Apply the tiling values to the instance material
            instanceMaterial.mainTextureScale = tiling;
            instanceMaterial.color = color;
            instanceMaterial.mainTexture = texture;

            // Assign the instance material to the Renderer of this GameObject
            Renderer renderer = GetComponent<Renderer>();
            renderer.sharedMaterial = instanceMaterial;

        }
        else
        {
            Debug.Log("Original material is now assigned!");
            originalMaterial = GetComponent<MeshRenderer>().material;
            texture = originalMaterial.mainTexture;
        }
    }
}

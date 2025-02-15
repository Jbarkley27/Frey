using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HitFlash : MonoBehaviour 
{
    public List<Renderer> renderers = new List<Renderer>();
    public List<Material> originalMaterials = new List<Material>();
    public Coroutine flashCoroutine;

    private void Start() {

        // Get all renderers in children and within those children recursively
        GetRenderers(transform);
        GetOriginalMaterials();
    }

    public void Flash(Material color, float duration = .3f)
    {
        if (flashCoroutine != null)
        {
            Debug.Log("Stopping coroutine");
            StopCoroutine(flashCoroutine);
            ResetMaterial();
        }

        Debug.Log("Starting coroutine");
        flashCoroutine = StartCoroutine(FlashCoroutine(color, duration));
    }

    private IEnumerator FlashCoroutine(Material color, float duration)
    {
        // Set the color of the material to the color passed in
        foreach (Renderer renderer in renderers)
        {
            Debug.Log("Switch from " + renderer.material + " to " + renderer.material.color);
            renderer.material = color;
        }

        yield return new WaitForSeconds(duration);

        // Reset the color of the material to the original color
        ResetMaterial();
    }

    public void ResetMaterial()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    
    private void GetRenderers(Transform parent)
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    private void GetOriginalMaterials()
    {
        foreach (Renderer renderer in renderers)
        {
            originalMaterials.Add(renderer.materials[0]);
        }
    }
}
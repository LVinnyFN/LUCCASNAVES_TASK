using System.Collections.Generic;
using UnityEngine;

public class DebugInteractable : MonoBehaviour, IInteractable
{
    public Material material;
    public MeshRenderer mesh;

    public Color hoverColor;
    public Color interactColor;
    private Color defaultColor;

    private void Awake()
    {
        material = Material.Instantiate(this.material);
        List<Material> materials = new List<Material>();
        materials.Add(material);
        mesh.SetMaterials(materials);
        defaultColor = material.GetColor("_Color");
    }

    public void OnHover()
    {
        material.SetColor("_Color", hoverColor);
        Debug.Log("Hover");
    }

    public void OnInteract()
    {
        material.SetColor("_Color", interactColor);
        Debug.Log("Interact");
    }

    public void OnUnhover()
    {
        material.SetColor("_Color", defaultColor);
        Debug.Log("Unhover");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsObstacle => _isObstacle;
    
    [SerializeField]
    private Color _allowingColor;
    [SerializeField]
    private Color _forbiddingColor;
    [SerializeField] 
    private bool _isObstacle;
    
    private List<Material> _materials = new();

    private void Awake()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in renderers)
        {
            _materials.Add(meshRenderer.material);
        }
    }

    public void SetColor(bool isAvailable)
    {
        SetColor(isAvailable ? _allowingColor : _forbiddingColor);
    }

    public void ResetColor()
    {
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        foreach (var material in _materials)
        {
            material.color = color;
        }
    }
}
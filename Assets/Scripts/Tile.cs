using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IHighlightable
{
    public int GCost = int.MaxValue;  // стоимость пути от старта до этого узла
    public int HCost;                 // эвристическая оценка расстояния до цели
    public int FCost => GCost + HCost;

    public Tile Parent; // ссылка на родительский узел (для восстановления пути)
    public Vector2Int Index { get; private set; }

    public bool IsObstacle => _isObstacle;
    
    [SerializeField]
    private Color _allowingColor;
    [SerializeField]
    private Color _forbiddingColor;
    [SerializeField]
    private Color _highlightingColor;
    [SerializeField] 
    private MeshRenderer _tileTopRenderer;
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

    public void SetIndex(Vector2Int index)
    {
        Index = index;
    }

    public void HighlightFinalPoint()
    {
        _tileTopRenderer.material.color = _highlightingColor;
    }

    public void HighlightOnPath()
    {
        _tileTopRenderer.material.color = _allowingColor;
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

    public void ResetPathData()
    {
        if (!_isObstacle)
        {
            GCost = int.MaxValue;
            HCost = 0;
            Parent = null;
        }
    }
}
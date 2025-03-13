using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    private Camera _camera;
    private Tile _lastHighlightedTile;
    private List<Tile> _path;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void ProcessMouseHighlight()
    {
        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            hitInfo.transform.TryGetComponent<Tile>(out var currentHighlightedTile);

            var isNewObject = currentHighlightedTile != _lastHighlightedTile;
            // Если это новый объект (не ранее подсвеченный)
            if (isNewObject)
            {
                if (_lastHighlightedTile != null)
                {
                    // Убираем подсветку ранее подсвеченному тайлу
                    _lastHighlightedTile.ResetColor();
                }

                // Если объект, на который указывает мышь, есть (это подсвечиваемый тайл)
                if (currentHighlightedTile != null && !currentHighlightedTile.IsObstacle)
                {
                    // Подсвечиваем его
                    currentHighlightedTile.HighlightFinalPoint();
                }

                // Сохраняем текущий подсвечиваемый объект в поле
                _lastHighlightedTile = currentHighlightedTile;
            }
        }
    }

    public void HighlightPath(List<Tile> path)
    {
        _path = path;
        
        foreach (var tile in path)
        {
            tile.HighlightOnPath();
        }
    }

    public void ResetPathHighlighting()
    {
        foreach (var tile in _path)
        {
            tile.ResetColor();
        }
    }
}
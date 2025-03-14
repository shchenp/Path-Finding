using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    private Camera _camera;
    private IHighlightable _lastHighlightedTile;
    private List<IHighlightable> _path;

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
            hitInfo.transform.TryGetComponent<IHighlightable>(out var currentHighlightedTile);

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

    public void HighlightPath(IEnumerable<IHighlightable> path)
    {
        _path = path.ToList();
        
        foreach (var tile in _path)
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
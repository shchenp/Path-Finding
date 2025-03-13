using UnityEngine;

public class MovementSelector : MonoBehaviour
{
    private bool _isGameStarted;
    private Camera _camera;
    private Tile _lastHighlightedTile;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void SetGameState(bool isStarted)
    {
        _isGameStarted = isStarted;
    }

    private void Update()
    {
        if (!_isGameStarted)
        {
            return;
        }

        ProcessMouseHighlight();
    }

    private void ProcessMouseHighlight()
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
}
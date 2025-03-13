using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] 
    private Map _map;
    [SerializeField] 
    private MapIndexProvider _mapIndexProvider;

    private Camera _camera;
    private Tile _currentTile;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void StartPlacingTile(Tile tilePrefab)
    {
        ResetBuilder();
        
        _currentTile = Instantiate(tilePrefab, _map.transform, true);
    }

    private void Update()
    {
        if (!_currentTile)
        {
            return;
        }
        
        UpdateTilePosition();
        ProcessPlacementInput();
    }

    private void UpdateTilePosition()
    {
        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            // Получаем индекс и позицию тайла по позиции курсора
            var tileIndex = _mapIndexProvider.GetIndex(hitInfo.point);
            var tilePosition = _mapIndexProvider.GetTilePosition(tileIndex);
            _currentTile.transform.localPosition = tilePosition;

            // Проверяем, доступно ли место для постройки тайла
            var isAvailable = _map.IsCellAvailable(tileIndex);
            // Задаем тайлу соответствующий цвет
            _currentTile.SetColor(isAvailable);
            _currentTile.SetIndex(tileIndex);
        }
    }

    private void ProcessPlacementInput()
    {
        var isAvailable = _map.IsCellAvailable(_currentTile.Index);
        
        if (isAvailable && Input.GetMouseButtonDown(0))
        {
            SetTile(_currentTile.Index);
        }
    }

    private void SetTile(Vector2Int tileIndex)
    {
        _map.SetTile(tileIndex, _currentTile);
        _currentTile.ResetColor();
        _currentTile = null;
    }


    public void ResetBuilder()
    {
        if (_currentTile != null)
        {
            Destroy(_currentTile.gameObject);
        }
    }
}
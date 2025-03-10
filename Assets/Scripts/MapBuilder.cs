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

    public void StartPlacingTile(GameObject tilePrefab)
    {
        ResetBuilder();
        
        var tileObject = Instantiate(tilePrefab);
        
        _currentTile = tileObject.GetComponent<Tile>();
        _currentTile.transform.SetParent(_map.transform);
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hitInfo) && _currentTile != null) 
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
            
            // Если место недоступно для постройки - выходим из метода
            if (!isAvailable)
            {
                return;
            }
            
            // Если нажата ЛКМ - устанавливаем тайл 
            if (Input.GetMouseButtonDown(0))
            {
                _map.SetTile(tileIndex, _currentTile);
                _currentTile.ResetColor();
                _currentTile = null;
            }
        }
    }

    public void ResetBuilder()
    {
        if (_currentTile != null)
        {
            Destroy(_currentTile.gameObject);
        }
    }
}
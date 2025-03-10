using UnityEngine;

public class Map : MonoBehaviour
{
    public Tile[,] Tiles => _tiles;
    public Vector2Int Size => _size;

    [SerializeField] 
    private Vector2Int _size;
    
    private Tile[,] _tiles;
    
    private void Awake()
    {
        _tiles = new Tile[Size.x, Size.y];
    }

    public bool IsCellAvailable(Vector2Int index)
    {
        // Если индекс за пределами сетки - возвращаем false
        if (IsOutOfGrid(index))
        {
            return false;
        }

        // Возвращаем значение, свободна ли клетка в пределах сетки
        var isFree = _tiles[index.x, index.y] == null;
        return isFree;
    }

    public void SetTile(Vector2Int index, Tile tile)
    {
        _tiles[index.x, index.y] = tile;
    }

    private bool IsOutOfGrid(Vector2Int index)
    {
        return index.x < 0 || index.y < 0 ||
               index.x >= _tiles.GetLength(0) || index.y >= _tiles.GetLength(1);
    }
}
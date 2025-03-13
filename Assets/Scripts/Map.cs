using UnityEngine;

public class Map : MonoBehaviour
{
    public Tile[,] Tiles { get; private set; }

    public Vector2Int Size => _size;

    [SerializeField] 
    private Vector2Int _size;

    private void Awake()
    {
        Tiles = new Tile[Size.x, Size.y];
    }

    public bool IsCellAvailable(Vector2Int index)
    {
        // Если индекс за пределами сетки - возвращаем false
        if (IsOutOfGrid(index))
        {
            return false;
        }

        // Возвращаем значение, свободна ли клетка в пределах сетки
        var isFree = Tiles[index.x, index.y] == null;
        return isFree;
    }

    public void SetTile(Vector2Int index, Tile tile)
    {
        Tiles[index.x, index.y] = tile;
    }

    public bool IsOutOfGrid(Vector2Int index)
    {
        return index.x < 0 || index.y < 0 ||
               index.x >= Tiles.GetLength(0) || index.y >= Tiles.GetLength(1);
    }
}
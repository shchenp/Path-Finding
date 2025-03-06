using System;
using System.Collections.Generic;

public class PathFinder
{
    private int _width;
    private int _height;
    private Tile[,] _tiles;

    public PathFinder(Map map)
    {
        _tiles = map.Tiles;
        _height = _tiles.GetLength(0);
        _width = _tiles.GetLength(1);
    }


    /// <summary>
    /// Находит кратчайший путь между тайлами.
    /// Если путь найден, возвращает список узлов, иначе — null.
    /// </summary>
    public List<Tile> FindPath(Tile fromTile, Tile toTile)
    {
        var openList = new List<Tile>();
        var closedSet = new HashSet<Tile>();

        // Инициализация стартового узла
        fromTile.GCost = 0;
        fromTile.HCost = GetDistance(fromTile, toTile);
        openList.Add(fromTile);

        while (openList.Count > 0)
        {
            // Выбираем узел с минимальным fCost (при равных — с меньшим hCost)
            var currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentTile.FCost ||
                   (openList[i].FCost == currentTile.FCost && openList[i].HCost < currentTile.HCost))
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closedSet.Add(currentTile);

            // Если достигли цели, восстанавливаем и возвращаем путь
            if (currentTile == toTile)
            {
                return RetracePath(fromTile, toTile);
            }

            foreach (var neighbor in GetNeighbors(currentTile))
            {
                if (neighbor.IsObstacle || closedSet.Contains(neighbor))
                {
                    continue;
                }

                var tentativeGCost = currentTile.GCost + 1; // стоимость перемещения между соседними узлами = 1

                if (tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = GetDistance(neighbor, toTile);
                    neighbor.Parent = currentTile;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        // Путь не найден
        ResetTiles();
        return null;
    }

    /// <summary>
    /// Восстанавливает путь от конечного узла до стартового с помощью ссылки parent.
    /// </summary>
    private List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        var path = new List<Tile>();
        var current = endTile;
        while (current != startTile)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Add(current);
        path.Reverse();
        
        ResetTiles();
        
        return path;
    }

    /// <summary>
    /// Вычисляет манхэттенское расстояние между двумя узлами.
    /// Разрешены только горизонтальные и вертикальные перемещения.
    /// </summary>
    private int GetDistance(Tile a, Tile b)
    {
        return (int)(Math.Abs(a.transform.position.x - b.transform.position.x)
                     + Math.Abs(a.transform.position.y - b.transform.position.y));
    }

    /// <summary>
    /// Получает соседей узла: только по горизонтали и вертикали.
    /// </summary>
    private List<Tile> GetNeighbors(Tile node)
    {
        var neighbors = new List<Tile>();

        // Определяем возможные направления: вверх, вниз, вправо, влево
        var directions = new int[,]
        {
            { 0, 1 },  // вверх
            { 0, -1 }, // вниз
            { 1, 0 },  // вправо
            { -1, 0 }  // влево
        };
        
        for (var i = 0; i < directions.GetLength(0); i++)
        {
            var newX = node.Index.x + directions[i, 0];
            var newY = node.Index.y + directions[i, 1];
            
            if (newX >= 0 && newX < _width && newY >= 0 && newY < _height)
            {
                if (_tiles[newX, newY] != null)
                {
                    neighbors.Add(_tiles[newX, newY]);
                }
            }
        }

        return neighbors;
    }

    private void ResetTiles()
    {
        foreach (var tile in _tiles)
        {
            if (tile != null)
            {
                tile.ResetPathData();
            }
        }
    }
}
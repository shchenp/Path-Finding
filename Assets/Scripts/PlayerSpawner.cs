using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Map _map;
    [SerializeField] private float _heightSpawn;

    public void Spawn()
    {
        var tiles = _map.Tiles;
        foreach (var tile in tiles)
        {
            if (tile != null && !tile.IsObstacle)
            {
                var spawnPosition = tile.transform.position;
                spawnPosition.y = _heightSpawn;
                Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
                
                return;
            }
        }
        
        Debug.Log("There are no one free tile");
    }
}

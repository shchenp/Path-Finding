using UnityEngine;
using UnityEngine.AI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Map _map;

    public PlayerController Spawn()
    {
        var tiles = _map.Tiles;
        foreach (var tile in tiles)
        {
            if (tile != null && !tile.IsObstacle)
            {
                var spawnPosition = tile.transform.position;
                if (NavMesh.SamplePosition(spawnPosition, out var hit, 2.0f, NavMesh.AllAreas)) 
                {
                    var player = Instantiate(_playerPrefab, hit.position, Quaternion.identity);
                    player.Initialize(tile);
                    
                    return player;
                }
            }
        }
        
        Debug.Log("There are no one free tile");
        return null;
    }
}
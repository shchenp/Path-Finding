using UnityEngine;
using UnityEngine.AI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Map _map;

    public void Spawn()
    {
        var tiles = _map.Tiles;
        foreach (var tile in tiles)
        {
            if (tile != null && !tile.IsObstacle)
            {
                var spawnPosition = tile.transform.position;
                if (NavMesh.SamplePosition(spawnPosition, out var hit, 2.0f, NavMesh.AllAreas)) 
                {
                    Instantiate(_playerPrefab, hit.position, Quaternion.identity);
                }
                else 
                {
                    Debug.LogError("Не удалось найти подходящую точку на NavMesh!");
                }
                
                return;
            }
        }
        
        Debug.Log("There are no one free tile");
    }
}

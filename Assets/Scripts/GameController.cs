using JetBrains.Annotations;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private HighlightController _highlightController;
    [SerializeField]
    private PlayerSpawner _playerSpawner;
    [SerializeField]
    private Map _map;

    private Camera _camera;
    private PlayerController _player;
    private PathFinder _pathFinder;
    private bool _canSelectTile;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_canSelectTile)
        {
            return;
        }
        
        _highlightController.ProcessMouseHighlight();

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        TryFindPath();
    }

    private void TryFindPath()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            if (hit.collider.TryGetComponent(out Tile endTile))
            {
                var path = _pathFinder.FindPath(_player.PlayerPositionTile, endTile);

                if (path != null)
                {
                    _highlightController.HighlightPath(path);
                    _canSelectTile = false;
                    StartCoroutine(_player.Move(path));
                }
            }
        }
    }

    [UsedImplicitly]
    public void SpawnPlayer()
    {
        _player = _playerSpawner.Spawn();

        if (_player)
        {
            _pathFinder = new PathFinder(_map);
            _canSelectTile = true;
            _player.StopMoving += OnStopMoving;
        }
    }

    private void OnStopMoving()
    {
        _highlightController.ResetPathHighlighting();
        _canSelectTile = true;
    }

    private void OnDestroy()
    {
        if (_player)
        {
            _player.StopMoving -= OnStopMoving;
        }
    }
}
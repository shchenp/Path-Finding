using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private NavMeshAgent _navMeshAgent;
    
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private bool _isMoving;
    private PathFinder _pathFinder;
    private Animator _animator;
    private Camera _camera;
    private Tile _currentTile;

    public void Initialize(Tile currentTile, Map map)
    {
        _currentTile = currentTile;
        _pathFinder = new PathFinder(map);
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isMoving)
        {
            return;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            List<Tile> path = null;
            
            if (hit.collider.TryGetComponent(out Tile endTile))
            {
                path = _pathFinder.FindPath(_currentTile, endTile);
            }

            if (path != null)
            {
                StartCoroutine(Move(path));
            }
        }
    }

    private IEnumerator Move(List<Tile> path)
    {
        OnMoved(true);
        
        for (var i = 1; i < path.Count; i++)
        {
            var position = path[i].transform.position;
            
            position.y = transform.position.y;
            _navMeshAgent.SetDestination(position);
            
            while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > 0)
            {
                foreach (var tile in path)
                {
                    tile.HighlightOnPath();
                }
                
                yield return null;
            }
        }

        _currentTile = path.Last();
        OnMoved(false);
        
        foreach (var tile in path)
        {
            tile.ResetColor();
        }
    }

    private void OnMoved(bool isMoved)
    {
        _isMoving = isMoved;
        _animator.SetBool(IsMoving, isMoved);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Action StopMoving;
    public Tile PlayerPositionTile { get; private set; }

    [SerializeField] 
    private NavMeshAgent _navMeshAgent;
    
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private Animator _animator;

    public void Initialize(Tile currentTile)
    {
        PlayerPositionTile = currentTile;
        _animator = GetComponent<Animator>();
    }

    public IEnumerator Move(List<Tile> path)
    {
        SetMovingState(true);
        
        for (var i = 1; i < path.Count; i++)
        {
            var position = path[i].transform.position;
            
            position.y = transform.position.y;
            _navMeshAgent.SetDestination(position);
            
            while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > 0)
            {
                yield return null;
            }
        }

        PlayerPositionTile = path.Last();
        SetMovingState(false);
        StopMoving?.Invoke();
    }

    private void SetMovingState(bool isMoved)
    {
        _animator.SetBool(IsMoving, isMoved);
    }
}
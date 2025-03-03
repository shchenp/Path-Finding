using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private PathFinder _pathFinder;
    
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private Animator _animator;
    private Camera _camera;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    // По умолчанию у персонажа проигрывается анимация покоя.
    // Для того, чтобы запустить анимацию ходьбы - передавайте в параметр аниматора IsMoving значение true:
    // _animator.SetBool(IsMoving, true);
    // Для того, чтобы запустить анимацию покоя - передавайте в параметр аниматора IsMoving значение false:
    // _animator.SetBool(IsMoving, false);

    private void Update()
    {
        if (_navMeshAgent.remainingDistance <= 0)
        {
            _animator.SetBool(IsMoving, false);
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            _pathFinder.FindPath(_navMeshAgent, hit.point);
            
            //_animator.SetBool(IsMoving, true);
            //_navMeshAgent.SetDestination(hit.point);
        }
    }
}
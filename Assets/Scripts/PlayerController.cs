using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // По умолчанию у персонажа проигрывается анимация покоя.
    // Для того, чтобы запустить анимацию ходьбы - передавайте в параметр аниматора IsMoving значение true:
    // _animator.SetBool(IsMoving, true);
    // Для того, чтобы запустить анимацию покоя - передавайте в параметр аниматора IsMoving значение false:
    // _animator.SetBool(IsMoving, false);
}
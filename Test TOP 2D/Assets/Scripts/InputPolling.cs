using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputPolling : MonoBehaviour
{

    private PlayerInput _input;
    private bool _isCinematic = false;

    public struct InputStruct
    {
        public Vector2 move;
        public Vector2 fixedMove;
        
    };

    public static event Action OnAttackEvent;
    public static event Action<bool> OnHotbarScrollEvent;

    public static InputStruct inputStruct;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = GetComponent<PlayerInput>();

        if (_input == null)
        {
            Debug.LogError("PlayerInput component not found on this GameObject.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isCinematic)
            return;

        inputStruct.move = _input.actions["Move"].ReadValue<Vector2>();
        inputStruct.fixedMove = _input.actions["Move"].ReadValue<Vector2>();
        
    }

    void OnAttack()
    {
        OnAttackEvent?.Invoke();
        // Debug.Log("Attack performed");
    }

    void OnNext()
    {
        OnHotbarScrollEvent?.Invoke(true);
        // Debug.Log("Next hotbar slot");
    }

    void OnPrevious()
    {
        OnHotbarScrollEvent?.Invoke(false);
        // Debug.Log("Previous hotbar slot");
    }
}

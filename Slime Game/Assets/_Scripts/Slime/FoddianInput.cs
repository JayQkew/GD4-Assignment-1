using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoddianInput : MonoBehaviour
{
    private InputAction aimAction;
    [SerializeField] private Vector2 mousePos;

    private void Awake()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(aimAction.ReadValue<Vector2>());
    }
}

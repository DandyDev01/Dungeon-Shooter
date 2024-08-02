using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed;

	private Rigidbody2D _rigidbody;
    private PlayerControls _inputControls;
	private Vector2 _moveVector = Vector2.zero;
	private float _speedModifier = 1f;

	private void Awake()
	{
		_inputControls = new PlayerControls();
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		_inputControls.Enable();
		_inputControls.Player.Movement.performed += OnMovementPerformed;
		_inputControls.Player.Movement.canceled += OnMovementCanceled;
	}

	private void OnDisable()
	{
		_inputControls.Disable();
		_inputControls.Player.Movement.performed -= OnMovementPerformed;
		_inputControls.Player.Movement.canceled -= OnMovementCanceled;
	}

	private void FixedUpdate()
	{
		_rigidbody.velocity = _moveVector * Time.fixedDeltaTime * _speed * _speedModifier;
	}

	private void OnMovementPerformed(InputAction.CallbackContext value)
	{
		_moveVector = value.ReadValue<Vector2>();
	}

	private void OnMovementCanceled(InputAction.CallbackContext value)
	{
		_moveVector = Vector2.zero;
	}
}

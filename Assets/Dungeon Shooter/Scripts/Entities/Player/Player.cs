using Codice.Client.Common.GameUI;
using DungeonShooter.Player.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonShooter.Player
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private float _speed = 6f;
		[SerializeField] private float _dodgeForce = 6f;

		private List<PlayerEffect> _effects;
		private PlayerControls _inputControls;
		private PlayerStateHolder _stateHolder;
		private Animator _animator;
		private Vector2 _moveVector = Vector2.zero;
		private bool _dodgeInput;
		private bool _attackInput;
		private float _speedModifier = 1f;

		public PlayerStateBase CurrentState { get; set; }
		public PlayerStateHolder StateHolder => _stateHolder;
		public Vector2 MoveVector => _moveVector;
		public bool DodgeInput => _dodgeInput;
		public bool AttackInput => _attackInput;	
		public float SpeedModifier => _speedModifier;
		public float Speed => _speed;
		public float DodgeForce => _dodgeForce;

		private void Awake()
		{
			_inputControls = new PlayerControls();
			_stateHolder = new PlayerStateHolder(this);
			_animator = GetComponentInChildren<Animator>();

			_effects = new List<PlayerEffect>();

			CurrentState = _stateHolder.IdleState;
			CurrentState.Enter();
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

		private void Update()
		{
			_dodgeInput = _inputControls.Player.Dodge.WasPressedThisFrame();
			_attackInput = _inputControls.Player.Attack.ReadValue<float>() == 0 ? false : true;
			CurrentState.Run();

			UpdateEffects();
		}

		private void UpdateEffects()
		{
			PlayerEffect effectToRemove = null;
			foreach (PlayerEffect effect in _effects)
			{
				effect.Tick(Time.deltaTime);
				if (effect.HasCompleted)
					effectToRemove = effect;
			}

			if (effectToRemove != null)
				RemoveEffect(effectToRemove);
		}

		private void OnMovementPerformed(InputAction.CallbackContext value)
		{
			_moveVector = value.ReadValue<Vector2>();
		}

		private void OnMovementCanceled(InputAction.CallbackContext value)
		{
			_moveVector = Vector2.zero;
		}

		public void PlayAnimation(string animationName)
		{
			_animator.Play(animationName);
		}

		public void AddEffect(PlayerEffect effect)
		{
			if (_effects.Contains(effect))
				return;

			_effects.Add(effect);
			effect.Start(this);
		}

		public void RemoveEffect(PlayerEffect effect)
		{
			if (_effects.Contains(effect))
				_effects.Remove(effect);
		}
	}
}
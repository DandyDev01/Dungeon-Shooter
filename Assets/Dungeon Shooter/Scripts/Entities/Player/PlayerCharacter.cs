using DungeonShooter.Player.Effects;
using Guns2D;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonShooter.Player
{
	public class PlayerCharacter : MonoBehaviour
	{
		[SerializeField] private PlayerSpecial _specialAbility;
		[SerializeField] private float _speed = 6f;
		[SerializeField] private float _dodgeForce = 6f;

		private Camera _camera;
		private List<PlayerEffect> _effects;
		private SpriteRenderer _spriteRenderer;
		private PlayerControls _inputControls;
		private PlayerStateHolder _stateHolder;
		private Gun2D _currentGun;
		private Transform _gunPivotPoint;
		private Animator _animator;
		private Health _health;
		private InteractableBase _closestInteractable;
		private Vector2 _moveVector = Vector2.zero;
		private Vector3 _mousePosition = Vector3.zero;
		private bool _bossRoomKey;
		private bool _dodgeInput;
		private bool _attackInput;
		private float _speedModifier = 1f;
		private float _interationRange = 1f;

		public Health Health => _health;
		public PlayerStateBase CurrentState { get; set; }
		public PlayerStateHolder StateHolder => _stateHolder;
		public Vector2 MoveVector => _moveVector;
		public bool DodgeInput => _dodgeInput;
		public bool AttackInput => _attackInput;
		public bool BossRoomKey => _bossRoomKey;
		public float SpeedModifier { get => _speedModifier; set => _speedModifier = value; } 
		public float Speed => _speed;
		public float DodgeForce => _dodgeForce;

		private void Awake()
		{
			_inputControls = new PlayerControls();
			_stateHolder = new PlayerStateHolder(this);
			_effects = new List<PlayerEffect>();
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			_animator = GetComponentInChildren<Animator>();
			_currentGun = GetComponentInChildren<Gun2D>();
			_health = new Health(5);
			_camera = Camera.main;
			_gunPivotPoint = _currentGun.transform.parent;

			_currentGun.Init();
			
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
			
			HandleGun();

			CurrentState.Run();

			UpdateEffects();

			CheckForInteractables();

			SpriteFlip();

			if (_inputControls.Player.Interact.WasPressedThisFrame() && _closestInteractable != null)
				_closestInteractable.Interact(this);

			if (_inputControls.Player.SpecialMove.WasPerformedThisFrame())
				_specialAbility.Activate();
		}

		/// <summary>
		/// Flips the player sprite to orient with the direction they are moving on the x-axis
		/// </summary>
		private void SpriteFlip()
		{
			if (_moveVector.x > 0)
				_spriteRenderer.flipX = true;
			else if (_moveVector.x < 0)
				_spriteRenderer.flipX = false;
		}

		/// <summary>
		/// Checks for interactables around the player.
		/// </summary>
		private void CheckForInteractables()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interationRange)
				.OrderBy(x => Vector2.Distance(x.transform.position, transform.position))
				.Where(x => x.GetComponent<InteractableBase>() != null).ToArray();

			if (colliders.Length == 0)
			{
				if (_closestInteractable != null)
					_closestInteractable.transform.localScale = Vector2.one;
				
				_closestInteractable = null;
				return;
			}
			
			var closest = colliders.First().GetComponent<InteractableBase>();

			_closestInteractable = closest;

			if (_closestInteractable.tag != "Door")
				_closestInteractable.transform.localScale = Vector2.one * 2;
		}

		/// <summary>
		/// Set the guns rotation and handle its state.
		/// </summary>
		private void HandleGun()
		{
			_mousePosition = _inputControls.Player.Mouse.ReadValue<Vector2>();
			_mousePosition = _camera.ScreenToWorldPoint(_mousePosition);
			
			Vector3 direction = _mousePosition - _gunPivotPoint.transform.position;
			direction = direction.normalized;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			_gunPivotPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

			if (_attackInput)
				_currentGun.Fire();
			else
				_currentGun.EndFire();

			_currentGun.RunActiveState(Time.deltaTime);
		}

		/// <summary>
		/// Apply active efffects to the player.
		/// </summary>
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

		/// <summary>
		/// Plays a specified animation.
		/// </summary>
		/// <param name="animationName">Name of the animation to play.</param>
		public void PlayAnimation(string animationName)
		{
			_animator.Play(animationName);
		}

		/// <summary>
		/// Adds an effect to the player.
		/// </summary>
		/// <param name="effect">Effect that will be added to the player.</param>
		public void AddEffect(PlayerEffect effect)
		{
			if (_effects.Contains(effect) || _effects.Contains(x => x.EffectType == effect.EffectType))
				return;

			_effects.Add(effect);
			effect.Start(this);
		}

		/// <summary>
		/// Removes an effect from the player.
		/// </summary>
		/// <param name="effect">Effect to remove from the player.</param>
		public void RemoveEffect(PlayerEffect effect)
		{
			if (_effects.Contains(effect))
				_effects.Remove(effect);
		}

		/// <summary>
		/// Switch out the gun the player is using.
		/// </summary>
		/// <param name="newGun">The new gun that the player will be using.</param>
		public void SwitchGun(Gun2D newGun)
		{
			newGun.transform.parent = _currentGun.transform.parent;

			Destroy(_currentGun.gameObject);

			_currentGun = newGun;
			_currentGun.transform.position = Vector3.zero;
			_currentGun.Init();
		}

		/// <summary>
		/// Sets the value of bossKey
		/// </summary>
		/// <param name="key">Wheather or not the player has a boss key.</param>
		public void PickupBossRoomKey(bool key)
		{
			_bossRoomKey = key;
		}
	}
}
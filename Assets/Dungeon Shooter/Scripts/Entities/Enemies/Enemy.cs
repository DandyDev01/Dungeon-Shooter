using Grid;
using System;
using System.Collections;
using UnityEngine;

namespace DungeonShooter
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private float _attackDistance = 1f;
		[SerializeField] private Transform _playerTransform;

		[Header("States")]
		[SerializeField] private EnemyBaseState _moveState;
		[SerializeField] private EnemyBaseState _attackState;
		[SerializeField] private EnemyBaseState _spawnState;
		[SerializeField] private EnemyBaseState _stunState;
		[SerializeField] private EnemyBaseState _deadState;

		private Health _health;
		private Animator _animator;
		private float _speedModifier = 1;

		public EnemyBaseState MoveState => _moveState;
		public EnemyBaseState AttackState => _attackState;
		public EnemyBaseState SpawnState => _spawnState;
		public EnemyBaseState StunState => _stunState;
		public EnemyBaseState DeadState => _deadState;
		public Health Health => _health;
		public EnemyBaseState CurrentState { get; set; }
		public float AttackDistance => _attackDistance;
		public float SpeedModifier { get => _speedModifier; set => _speedModifier = value; }	

		private void Awake()
		{
			_health = new Health(5);
			_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
			_animator = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			CurrentState = _spawnState;
			CurrentState.Enter();
		}

		private void Update()
		{
			CurrentState.Run();
		}

		/// <summary>
		/// Plays a specified animation.
		/// </summary>
		/// <param name="name">Name of the animation to play.</param>
		public void PlayAnimation(string name)
		{
			_animator.Play(name);
		}

		/// <summary>
		/// Get the position of the enemies target.
		/// </summary>
		/// <returns>Position of the target.</returns>
		public Vector2 GetTargetPosition()
		{
			return _playerTransform.position;
		}

		/// <summary>
		/// Moves the enemy around the world to their target.
		/// </summary>
		public void Move()
		{
			transform.position = Vector2.MoveTowards(transform.position, GetTargetPosition(), _speed * _speedModifier * Time.deltaTime);
		}
	}
}
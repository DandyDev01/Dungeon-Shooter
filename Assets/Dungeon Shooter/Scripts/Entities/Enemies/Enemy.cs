using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
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

		private Health _health;
		private Animator _animator;
		private float _speedModifier = 1;

		public EnemyBaseState MoveState => _moveState;
		public EnemyBaseState AttackState => _attackState;
		public EnemyBaseState SpawnState => _spawnState;
		public Health Health => _health;
		public EnemyBaseState CurrentState { get; set; }
		public float AttackDistance => _attackDistance;

		private void Awake()
		{
			_health = new Health(5);
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

		public void PlayAnimation(string name)
		{
			_animator.Play(name);
		}

		public Vector2 GetTargetPosition()
		{
			return _playerTransform.position;
		}

		public void Move()
		{
			transform.position = Vector2.MoveTowards(transform.position, GetTargetPosition(), _speed * _speedModifier * Time.deltaTime);
		}
	}
}
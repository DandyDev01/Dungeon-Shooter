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
		[SerializeField] private Transform _playerTransform;
		[SerializeField] private SampleGridXY _grid;
		[SerializeField] private GameObject _marker;

		[Header("States")]
		[SerializeField] private EnemyBaseState _moveState;
		[SerializeField] private EnemyBaseState _attackState;
		[SerializeField] private EnemyBaseState _spawnState;

		private Rigidbody2D _rigidbody;
		private PathBuilder _pathBuilder;
		private Health _health;
		private Animator _animator;
		private float _speedModifier = 1;

		public EnemyBaseState MoveState => _moveState;
		public EnemyBaseState AttackState => _attackState;
		public EnemyBaseState SpawnState => _spawnState;
		public Health Health => _health;
		public PathBuilder PathBuilder => _pathBuilder;
		public EnemyBaseState CurrentState { get; set; }

		private void Awake()
		{
			_health = new Health(5);
			_rigidbody = GetComponent<Rigidbody2D>();
			_animator = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			_pathBuilder = new PathBuilder(_grid.Grid, _marker);

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

		internal Vector2 GetTargetPosition()
		{
			return _playerTransform.position;
		}

		internal void Move(Vector2 currentPathTarget)
		{
			Vector2 direction = currentPathTarget - (Vector2)transform.position;

			_rigidbody.velocity = direction.normalized * _speed * _speedModifier * Time.deltaTime;
		}
	}

}
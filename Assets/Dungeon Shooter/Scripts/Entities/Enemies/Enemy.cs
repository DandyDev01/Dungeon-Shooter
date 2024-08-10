using Grid;
using System;
using System.Collections;
using UnityEngine;

namespace DungeonShooter
{
	public class Enemy : EnemyBase
	{
		[Header("States")]
		[SerializeField] private EnemyBaseState _moveState;
		[SerializeField] private EnemyBaseState _attackState;
		[SerializeField] private EnemyBaseState _spawnState;
		[SerializeField] private EnemyBaseState _stunState;
		[SerializeField] private EnemyBaseState _deadState;

		public EnemyBaseState MoveState => _moveState;
		public EnemyBaseState AttackState => _attackState;
		public EnemyBaseState SpawnState => _spawnState;
		public EnemyBaseState StunState => _stunState;
		public EnemyBaseState DeadState => _deadState;
		public EnemyBaseState CurrentState { get; set; }

		private void Start()
		{
			CurrentState = _spawnState;
			CurrentState.Enter();
		}

		private void Update()
		{
			CurrentState.Run();
		}
	}
}
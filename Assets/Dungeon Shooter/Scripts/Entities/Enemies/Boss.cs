using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class Boss : EnemyBase
	{
		[SerializeField] private BossStateBase _stageOneState;
		[SerializeField] private BossStateBase _stageTwoState;

		protected BossStateBase _moveState;
		protected BossStateBase _spawnState;
		protected BossStateBase _deadState;

		public BossStateBase MoveState => _moveState;
		public BossStateBase SpawnState => _spawnState;
		public BossStateBase DeadState => _deadState;

		public BossStateBase CurrentRootState { get; set; }

		private void Start()
		{
			_moveState = GetComponentInChildren<BossMoveState>();
			_spawnState = GetComponentInChildren<BossSpawnState>();
			_deadState = GetComponentInChildren<BossDeadState>();
		}
	}
}
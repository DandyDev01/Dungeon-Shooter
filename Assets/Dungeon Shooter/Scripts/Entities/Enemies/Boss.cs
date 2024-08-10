using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
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
		public BossStateBase StageOneState => _stageOneState;
		public BossStateBase StageTwoState => _stageTwoState;

		public BossStateBase CurrentState { get; set; }

		protected override void Awake()
		{
			base.Awake();
			_moveState = GetComponentInChildren<BossMoveState>();
			_spawnState = GetComponentInChildren<BossSpawnState>();
			_deadState = GetComponentInChildren<BossDeadState>();
		}

		private void Start()
		{
			CurrentState = _stageOneState;
			CurrentState.Enter();
			CurrentState.SwitchSubState(_spawnState);
		}

		private void Update()
		{
			CurrentState.Run(); 
		}
	}
}
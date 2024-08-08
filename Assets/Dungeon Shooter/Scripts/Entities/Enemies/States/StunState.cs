using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class StunState : EnemyBaseState
	{
		private Timer _timer;
		private float _duration = 3f;

		private void Awake()
		{
			_timer = new Timer(_duration, false);
			_timer.OnTimerEnd += Exit;
		}

		private void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		public override void Enter()
		{
			_timer.Play();

			_enemy.SpeedModifier = 0;
		}

		public override void Exit()
		{
			_timer.Stop();
			_timer.Reset(_duration);

			_enemy.SpeedModifier = 1f;
		}

		public override void Run()
		{

		}

		protected override void CheckForStateSwitch()
		{
			if (_timer.Finished)
				SwitchState(_enemy.MoveState);
		}
	}
}
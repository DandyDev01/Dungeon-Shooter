using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class StunState : EnemyBaseState
	{
		private Timer _timer;
		private ParticleSystem _particleSystem;
		private float _duration = 3f;

		private void Start()
		{
			_particleSystem = GetComponentInChildren<ParticleSystem>();
			_timer = new Timer(_duration, false);

			_particleSystem.Stop();
		}

		public override void Enter()
		{
			_particleSystem.Play();

			_timer.Play();

			_enemy.SpeedModifier = 0;
		}

		public override void Exit()
		{
			_particleSystem.Stop();

			_timer.Stop();
			_timer.Reset(_duration);

			_enemy.SpeedModifier = 1f;
		}

		public override void Run()
		{
			CheckForStateSwitch();
			_timer.Tick(Time.deltaTime);
		}

		protected override void CheckForStateSwitch()
		{
			if (_timer.Finished)
				SwitchState(_enemy.MoveState);

			if (_enemy.Health.Current <= 0)
				SwitchState(_enemy.DeadState);
		}
	}
}
using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class SpawnState : EnemyBaseState
	{
		private const float _spawnTime = 2f;
		private Timer _timer;
		private ParticleSystem _particleSystem;
		private SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_enemy = GetComponentInParent<Enemy>();
			_particleSystem = GetComponentInChildren<ParticleSystem>();
			_spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
			_timer = new Timer(_spawnTime, false);
		}

		public override void Enter()
		{
			transform.parent.SetChildrenActive(false);

			gameObject.SetActive(true);

			_timer.Play();
			
			_particleSystem.Play();
		}

		public override void Exit()
		{
			transform.parent.SetChildrenActive(true);

			_timer.Stop();
			_timer.Reset(_spawnTime);
			
			_particleSystem.Stop();
		}

		public override void Run()
		{
			_timer.Tick(Time.deltaTime);
			
			CheckForStateSwitch();
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
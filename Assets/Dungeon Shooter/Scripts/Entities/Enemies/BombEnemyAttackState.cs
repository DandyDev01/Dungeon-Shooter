using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonShooter.Player;

namespace DungeonShooter
{
	public class BombEnemyAttackState : EnemyBaseState
	{
		[SerializeField] private GameObject _damageRadiusVisualizer;
		[SerializeField] private GameObject _damgeRadiusExpansion;

		private Vector2 _baseScale;
		private const float _timeToExplode = 2f;
		private float _expansionSpeed = 0f; 
		private float _timePassed = 0f;

		public override void Enter()
		{
			_timePassed = 0f;

			_damageRadiusVisualizer.SetActive(true);
			_damgeRadiusExpansion.SetActive(true);

			_baseScale = _damageRadiusVisualizer.transform.localScale;
			_damgeRadiusExpansion.transform.localScale = Vector2.one;

			_expansionSpeed = _baseScale.x / _timeToExplode;
		}

		public override void Exit()
		{
			_damageRadiusVisualizer.SetActive(false);
			_damgeRadiusExpansion.SetActive(false);
		}

		public override void Run()
		{
			_timePassed += Time.deltaTime;

			if (_timePassed >= _timeToExplode)
				Explode();

			_damgeRadiusExpansion.transform.localScale += Vector3.one * _expansionSpeed * Time.deltaTime;
		}

		private void Explode()
		{
			var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
			player.Health.Damage(1);

			Destroy(transform.parent.gameObject);
		}

		protected override void CheckForStateSwitch()
		{
			if (Vector2.Distance(_enemy.transform.position, _enemy.GetTargetPosition()) > 2.5f)
				SwitchState(_enemy.MoveState);
		}
	}
}
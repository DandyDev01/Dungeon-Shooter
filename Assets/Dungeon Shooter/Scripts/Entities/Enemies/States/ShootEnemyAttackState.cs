using Guns2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{

	public class ShootEnemyAttackState : EnemyBaseState
	{
		private Gun2D _gun;
		private Transform _gunPivotPont;

		private void Start()
		{
			_gun = GetComponentInChildren<Gun2D>();

			_gunPivotPont = _gun.transform.parent;

			_gun.Init();
		}

		public override void Enter()
		{
			_gun.Fire();
		}

		public override void Exit()
		{
			_gun.EndFire();
		}

		public override void Run()
		{
			CheckForStateSwitch();

			Vector2 direction = (Vector3)_enemy.GetTargetPosition() - _enemy.transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			_gunPivotPont.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
			_gun.RunActiveState(Time.deltaTime);
		}

		protected override void CheckForStateSwitch()
		{
			if (Vector2.Distance(_enemy.transform.position, _enemy.GetTargetPosition()) > 2.5f)
				SwitchState(_enemy.MoveState);

			if (_enemy.Health.Current <= 0)
				SwitchState(_enemy.DeadState);
		}
	}
}
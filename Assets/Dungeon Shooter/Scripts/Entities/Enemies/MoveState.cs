using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class MoveState : EnemyBaseState
	{
		private List<Node> _path;
		private Vector2 _currentPathTarget = Vector2.zero;
		private int _index = 0;

		public override void Enter()
		{
			_path = _enemy.PathBuilder.CalculatePath(_enemy.GetTargetPosition(), transform.position);
			_currentPathTarget = _path[_index]._worldPosition;
		}

		public override void Exit()
		{
			throw new System.NotImplementedException();
		}

		public override void Run()
		{
			CheckForStateSwitch();

			if (Vector2.Distance(_currentPathTarget, transform.position) < 0.05)
			{
				_index += 1;
				_currentPathTarget = _path[_index]._worldPosition;
			}

			_enemy.Move(_currentPathTarget);
		}

		protected override void CheckForStateSwitch()
		{
			if (Vector2.Distance(transform.position, _enemy.GetTargetPosition()) < 1)
				SwitchState(_enemy.AttackState);
		}
	}
}
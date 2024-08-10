using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonShooter.Enemies;

namespace DungeonShooter.Player
{
	public class StunEnemiesSpecial : PlayerSpecial
	{
		private float _range = 3f;

		public override void Activate()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range);

			foreach (Collider2D collider in colliders)
			{
				Enemy enemy = collider.GetComponent<Enemy>();

				if (enemy == null)
					continue;

				enemy.CurrentState.SwitchState(enemy.StunState);
			}
		}
	}
}
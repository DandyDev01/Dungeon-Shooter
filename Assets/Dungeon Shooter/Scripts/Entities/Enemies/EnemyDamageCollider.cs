using Guns2D;
using System;
using UnityEngine;

namespace DungeonShooter
{
	public class EnemyDamageCollider : MonoBehaviour
	{
		private Enemy _enemy;

		public bool IsDisabled { get; set; } = false;

		private void Awake()
		{
			_enemy = GetComponentInParent<Enemy>();	
		}

		private void Start()
		{
			_enemy.Health.OnDeath += Die;
		}

		private void OnDestroy()
		{
			_enemy.Health.OnDeath -= Die;
		}

		private void Die()
		{
			_enemy.CurrentState.SwitchState(_enemy.DeadState);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (IsDisabled)
				return;

			if (collision.transform.tag == "Projectile")
			{
				Projectile projectile = collision.gameObject.GetComponent<Projectile>();

				_enemy.Health.Damage(projectile.Damage);

				Destroy(projectile.gameObject);
			}
		}
	}
}
using Guns2D;
using UnityEngine;

namespace DungeonShooter
{
	public class EnemyDamageCollider : MonoBehaviour
	{
		private Enemy _enemy;

		public bool IsDisabled { get; set; }

		private void Awake()
		{
			_enemy = GetComponentInParent<Enemy>();	
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (IsDisabled)
				return;

			if (collision.transform.tag == "Projectile")
			{
				Debug.Log("Hit by " + collision.gameObject.name);
				Projectile projectile = collision.gameObject.GetComponent<Projectile>();

				_enemy.Health.Damage(projectile.Damage);

				Destroy(projectile.gameObject);
			}
		}
	}
}
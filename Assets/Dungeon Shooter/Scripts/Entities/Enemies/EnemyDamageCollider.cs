using Guns2D;
using System;
using System.Collections;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class EnemyDamageCollider : MonoBehaviour
	{
		[SerializeField] private Material _hitMaterial;

		private SpriteRenderer _spriteRenderer;
		private EnemyBase _enemy;

		public bool IsDisabled { get; set; } = false;

		private void Awake()
		{
			_spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();	
			_enemy = GetComponentInParent<EnemyBase>();	
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (IsDisabled || _enemy.Health.Current <= 0)
				return;

			if (collision.transform.tag == "Projectile")
			{
				Projectile projectile = collision.gameObject.GetComponent<Projectile>();

				_enemy.Health.Damage(projectile.Damage);

				StartCoroutine(MaterialSwap());

				Destroy(projectile.gameObject);
			}
		}

		private IEnumerator MaterialSwap()
		{
			Material defaultMaterial = _spriteRenderer.material;

			for (int i = 0; i < 3; i++)
			{
				_spriteRenderer.material = _hitMaterial;

				yield return new WaitForSeconds(0.1f);

				_spriteRenderer.material = defaultMaterial;

				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}
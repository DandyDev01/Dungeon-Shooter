using DungeonShooter.Player;
using DungeonShooter.Player.Effects;
using Guns2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
	[SerializeField] private Material _hitMaterial;

	private SpriteRenderer _spriteRenderer;
	private PlayerCharacter _player;
	
	public bool IsDisabled { get; set; }

	private void Awake()
	{
		_spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
		_player = GetComponentInParent<PlayerCharacter>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsDisabled)
			return;

		if (collision.transform.tag == "Projectile")
		{
			Projectile projectile = collision.gameObject.GetComponent<Projectile>();
			_player.Health.Damage(projectile.Damage);

			StartCoroutine(MaterialSwap());

			if (projectile.Effects.Any())
			{
				foreach (PlayerEffectType effect in projectile.Effects)
				{
					_player.AddEffect(PlayerEffect.Get(effect));
				}
			}

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

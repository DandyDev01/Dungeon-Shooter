using DungeonShooter.Player;
using DungeonShooter.Player.Effects;
using Guns2D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
	private PlayerCharacter _player;
	
	public bool IsDisabled { get; set; }

	private void Awake()
	{
		_player = GetComponentInParent<PlayerCharacter>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsDisabled)
			return;

		if (collision.transform.tag == "Projectile")
		{
			Debug.Log("Hit by " + collision.gameObject.name);
			Projectile projectile = collision.gameObject.GetComponent<Projectile>();
			_player.Health.Damage(projectile.Damage);

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
}

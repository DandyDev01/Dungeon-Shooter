using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player.Effects
{
	public class InvincibleEffect : PlayerEffect
	{
		private PlayerDamageCollider _effectedDamageCollider;
		private CapsuleCollider2D _capsuleCollider;

		public InvincibleEffect(float duration) : base(duration)
		{
		}

		public override void Tick(float delta)
		{
			_timer.Tick(delta);
		}

		public override void Start(PlayerCharacter effected)
		{
			_timer.Play();

			HasCompleted = false;
			
			_effectedDamageCollider = effected.GetComponentInChildren<PlayerDamageCollider>();
			_capsuleCollider = _effectedDamageCollider.GetComponent<CapsuleCollider2D>();
			
			_capsuleCollider.enabled = false;
			_effectedDamageCollider.IsDisabled = true;

			_effected = effected;
		}

		public override void Stop()
		{
			_timer.Stop();
			_timer.Reset(_duration);
			
			_effectedDamageCollider.IsDisabled = false;
			_capsuleCollider.enabled = true;
			
			_effectedDamageCollider = null;
			_capsuleCollider = null;

			HasCompleted = true;

			_effected = null;
		}
	}
}

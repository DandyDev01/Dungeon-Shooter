using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player.Effects
{
	public class InvincibleEffect : PlayerEffect
	{
		private PlayerDamageCollider _effectedDamageCollider;

		public InvincibleEffect(float duration) : base(duration)
		{
		}

		public override void Tick(float delta)
		{
			_timer.Tick(delta);
			Debug.Log("Invincible");
		}

		public override void Start(Player effected)
		{
			_timer.Play();

			HasCompleted = false;
			
			_effectedDamageCollider = effected.GetComponentInChildren<PlayerDamageCollider>();
			_effectedDamageCollider.IsDisabled = true;

			_effected = effected;
		}

		public override void Stop()
		{
			_timer.Stop();
			_timer.Reset(_duration);
			
			_effectedDamageCollider.IsDisabled = false;
			_effectedDamageCollider = null;

			HasCompleted = true;

			_effected = null;
		}
	}
}

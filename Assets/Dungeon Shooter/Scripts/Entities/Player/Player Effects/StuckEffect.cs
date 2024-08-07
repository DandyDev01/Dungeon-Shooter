using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player.Effects
{
	public class StuckEffect : PlayerEffect
	{
		public StuckEffect(float duration) : base(duration)
		{
			_effectType = Guns2D.PlayerEffectType.STICKY;
		}

		public override void Start(PlayerCharacter effected)
		{
			_timer.Play();
			
			effected.SpeedModifier = 0f;
			
			_effected = effected;
			
			HasCompleted = false;
		}

		public override void Stop()
		{
			_timer.Stop();
			_timer.Reset(_duration);
			
			_effected.SpeedModifier = 1f;
			_effected = null;
			
			HasCompleted = true;
		}

		public override void Tick(float delta)
		{
			_timer.Tick(delta);
		}
	}
}
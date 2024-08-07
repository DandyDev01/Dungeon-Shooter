using Guns2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player.Effects
{
	public abstract class PlayerEffect
	{
		protected readonly Timer _timer;
		protected readonly float _duration;
		protected PlayerCharacter _effected;
		protected PlayerEffectType _effectType;

		public PlayerEffectType EffectType => _effectType;
		public bool HasCompleted { get; protected set; }

		public PlayerEffect(float duration)
		{
			_timer = new Timer(duration, false);
			_duration = duration;

			_timer.OnTimerEnd += Stop;
		}

		public abstract void Tick(float delta);

		public abstract void Start(PlayerCharacter effected);

		public abstract void Stop();

		internal static PlayerEffect Get(PlayerEffectType effect)
		{
			switch (effect)
			{
				case PlayerEffectType.STICKY:
					return new StuckEffect(3f);
			}

			throw new Exception("Could not find effect: " + effect);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player.Effects
{
	public abstract class PlayerEffect
	{
		protected readonly Timer _timer;
		protected readonly float _duration;
		protected Player _effected;
		
		public bool HasCompleted { get; protected set; }

		public PlayerEffect(float duration)
		{
			_timer = new Timer(duration, false);
			_duration = duration;

			_timer.OnTimerEnd += Stop;
		}

		public abstract void Tick(float delta);

		public abstract void Start(Player effected);

		public abstract void Stop();
	}
}
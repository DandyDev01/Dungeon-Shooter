using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player
{
	public class SlowTimeSpecial : PlayerSpecial
	{
		private PlayerCharacter _player;
		private Timer _timer;
		private float _duration = 2.5f;

		private void Awake()
		{
			_timer = new Timer(_duration, false);
			_timer.OnTimerEnd += Stop;
		}

		private void Start()
		{
			_player = GetComponentInParent<PlayerCharacter>();	
		}

		private void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		private void Stop()
		{
			_timer.Stop();
			_timer.Reset(_duration);

			Time.timeScale = 1f;

			_player.SpeedModifier = 1f;
		}

		public override void Activate()
		{
			_timer.Play();

			Time.timeScale = 0.5f;

			_player.SpeedModifier = 1.5f;
		}
	}
}

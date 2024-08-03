using DungeonShooter.Player.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonShooter.Player
{
	public class PlayerDodgeState : PlayerStateBase
	{
		private readonly Rigidbody2D _rigidbody;
		private readonly Timer _timer;

		public PlayerDodgeState(Player player) : base(player)
		{
			_rigidbody = player.GetComponent<Rigidbody2D>();
			_timer = new Timer(0.5f, false);
		}

		public override void Enter()
		{
			_player.PlayAnimation("Dodge");
			_player.AddEffect(new InvincibleEffect(1f));

			_timer.Play();

			_rigidbody.AddForce(_player.MoveVector * _player.DodgeForce, ForceMode2D.Impulse);
		}

		public override void Exit()
		{
			_timer.Stop();
			_timer.Reset(0.5f);
		}

		public override void Run()
		{
			_timer.Tick(Time.deltaTime);
			CheckForStateSwitch();
		}

		protected override void CheckForStateSwitch()
		{
			if (_timer.Finished)
				SwitchState(_player.StateHolder.IdleState);
		}
	}
}

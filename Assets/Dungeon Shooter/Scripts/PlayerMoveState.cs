using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonShooter.Player
{
	public class PlayerMoveState : PlayerStateBase
	{
		private readonly Rigidbody2D _rigidbody;

		public PlayerMoveState(Player player) : base(player)
		{
			_rigidbody = player.GetComponent<Rigidbody2D>();
		}

		public override void Enter()
		{
			_player.PlayAnimation("Move");
		}

		public override void Exit()
		{
		}

		public override void Run()
		{
			//_rigidbody.velocity = _player.MoveVector * Time.fixedDeltaTime * _player.Speed * _player.SpeedModifier;

			_rigidbody.AddForce(_player.MoveVector * _player.Speed * _player.SpeedModifier);

			CheckForStateSwitch();
		}

		protected override void CheckForStateSwitch()
		{
			if (_player.MoveVector == Vector2.zero)
				SwitchState(_player.StateHolder.IdleState);
			else if (_player.DodgeInput)
				SwitchState(_player.StateHolder.DodgeState);
		}
	}
}

using DungeonShooter.Player;
using DungeonShooter.Player.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class ShieldInteractable : InteractableBase
	{
		public override void Interact(PlayerCharacter player)
		{
			player.AddEffect(new InvincibleEffect(5f));

			Destroy(gameObject);
		}
	}
}
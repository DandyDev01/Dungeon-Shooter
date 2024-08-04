using DungeonShooter.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class KeyInteractable : InteractableBase
{
		public override void Interact(PlayerCharacter player)
		{
			player.PickupBossRoomKey(this);
		}
	}
}

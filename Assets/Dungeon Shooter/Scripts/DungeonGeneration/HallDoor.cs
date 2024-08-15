using DungeonShooter.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace DungeonShooter.DungenGeneration
{
	public class HallDoor : InteractableBase
	{
		private Hall _hall;

		public override void Interact(PlayerCharacter player)
		{
			Room room = _hall.Attachments.OrderBy(x => Vector2.Distance(x.position, transform.position)).First().AttachedTo;

			if (player.BossRoomKey == false || room.IsBossRoom == false)
				return;

			player.PickupBossRoomKey(false);
			room.UnLockDoors();
		}

		private void Awake()
		{
			_hall = GetComponentInParent<Hall>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag != "Player")
				return;

			Room room = _hall.Attachments.OrderBy(x => Vector2.Distance(x.position, transform.position)).First().AttachedTo;

			if (room.IsActive == false && room.IsLocked == true && room.IsCleared == false)
			{
				Debug.Log("This is the boss room, you need the key to open the door.");
			}
		}
	}
}
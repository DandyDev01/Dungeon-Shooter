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

		public bool LeadsToBoss => _hall.Attachments.OrderBy(x => Vector2.Distance(x.position, transform.position)).First().AttachedTo.IsBossRoom;

		public override void Interact(PlayerCharacter player)
		{
			Room room = _hall.Attachments.OrderBy(x => Vector2.Distance(x.position, transform.position)).First().AttachedTo;

			if (player.BossRoomKey == false || room.IsBossRoom == false)
				return;

			player.PickupBossRoomKey(false);
			room.UnLockDoors();
		}

		private void Start()
		{
			_hall = GetComponentInParent<Hall>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag != "Player")
				return;

			Room room = _hall.Attachments.OrderBy(x => Vector2.Distance(x.position, transform.position)).First().AttachedTo;
			PlayerCharacter player = collision.GetComponent<PlayerCharacter>();

			string message = player.BossRoomKey ? "Open Boss Room" : "You need a key.";

			if (room.IsActive == false && room.IsLocked == true && room.IsCleared == false)
			{
				SetMessage(message);
			}
		}
	}
}
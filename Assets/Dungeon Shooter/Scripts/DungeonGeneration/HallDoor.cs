using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace DungeonShooter.DungenGeneration
{
	public class HallDoor : MonoBehaviour
	{
		private Hall _hall;
		private Room _room;

		private void Awake()
		{
			_hall = GetComponentInParent<Hall>();
			_room = _hall.Attachments.OrderBy(x => Vector2.Distance(x.position, transform.position)).First().AttachedTo;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (_room.IsActive == false && _room.IsLocked == true && _room.IsCleared == false)
			{
				Debug.Log("This is the boss room, you need the key to open the door.");
			}
		}
	}
}
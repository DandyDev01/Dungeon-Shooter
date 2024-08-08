using UnityEngine;

namespace DungeonShooter.DungenGeneration
{
	public class Door : MonoBehaviour
	{
		private Room _room;

		private void Awake()
		{
			_room = GetComponentInParent<Room>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (_room.IsActive || _room.IsCleared || collision.gameObject.tag != "Player")
				return;

			_room.Enter();
			Debug.Log("Enter", collision);
		}
	}
}
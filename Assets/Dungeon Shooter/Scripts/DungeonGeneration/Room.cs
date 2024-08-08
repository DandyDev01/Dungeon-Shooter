using System;
using UnityEngine;

namespace DungeonShooter.DungenGeneration
{
	public class Room : MonoBehaviour
	{
		[SerializeField] private AttachmentPoint<Hall>[] _attachmentPoints;
		[SerializeField] private Enemy[] _enemies;

		private Transform[] _doors;
		private Door[] _doorTriggers;
		private bool _isBossRoom = false;
		private bool _isCleared = false;
		private bool _isActive = false;
		private bool _isSpawn = false;
		private bool _isLoot = false;

		internal AttachmentPoint<Hall>[] Attachments => _attachmentPoints;

		public bool IsBossRoom => _isBossRoom;
		public bool IsCleared => _isCleared;
		public bool IsSpawn => _isSpawn;
		public bool IsActive => _isActive;

		private void Awake()
		{
			_doors = transform.GetChildrenWhere(x => x.tag == "Door");
			_doorTriggers = GetComponentsInChildren<Door>();
			
			UnLockDoors();
		}

		private void UnLockDoors()
		{
			foreach (Transform door in _doors)
			{
				door.gameObject.SetActive(false);
			}
		}

		private void LockDoors()
		{
			foreach (Transform door in _doors)
			{
				door.gameObject.SetActive(true);
			}
		}

		public void Enter()
		{
			_isActive = true;

			if (_isLoot)
				return;

			// lock doors
			LockDoors();

			// spawn enemies
			int index = UnityEngine.Random.Range(0, _enemies.Length);
			Enemy e = Instantiate(_enemies[index], transform.position, Quaternion.identity);
			
			// when all enemies are dead unlock the doors
			e.Health.OnDeath += UnLockDoors;
		}

		public void Clear()
		{
			_isActive = false;
			_isCleared = true;

			UnLockDoors();
		}

		public void SetSpawn(bool isSpawn)
		{
			_isSpawn = isSpawn;
			_isCleared = true;
		}

		public void SetBoss(bool isBoss)
		{
			_isBossRoom = isBoss;
			LockDoors();
		}

		public void SetIsLoot(bool isLoot) => _isLoot = isLoot;

		internal bool HasDoor(DoorLocation door)
		{
			return Attachments.Contains(x => x.door == door);
		}

		internal void UpdateAttachments(AttachmentPoint<Hall>[] attachments)
		{
			_attachmentPoints = attachments;
		}

		internal static Vector3Int DoorToDirection(DoorLocation door)
		{
			switch (door)
			{
				case DoorLocation.Left:
					return Vector3Int.left;
				case DoorLocation.Right:
					return Vector3Int.right;
				case DoorLocation.Top:
					return Vector3Int.up;
				case DoorLocation.Bottom:
					return Vector3Int.down;
			}

			throw new Exception("door error");
		}

		internal static DoorLocation DirectionToDoor(Vector3Int direction)
		{
			if (direction == Vector3Int.up)
			{
				return DoorLocation.Top;
			}
			else if (direction == Vector3Int.down)
			{
				return DoorLocation.Bottom;
			}
			else if (direction == Vector3Int.right)
			{
				return DoorLocation.Right;
			}
			else if (direction == Vector3Int.left)
			{
				return DoorLocation.Left;
			}
			else
			{
				return DoorLocation.Top;
			}

			throw new Exception("invalid direction");
		}
	}

	[Serializable]
	internal class AttachmentPoint<T>
	{
		[SerializeField] private Transform transform;
		 
		public DoorLocation door;
		public Vector3 position => transform.position;
		
		public T AttachedTo { get; set; }
	}
}

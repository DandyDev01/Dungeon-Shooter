using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.DungenGeneration
{
	public class Room : MonoBehaviour
	{
		[SerializeField] AttachmentPoint<Hall>[] _attachmentPoints;

		internal AttachmentPoint<Hall>[] Attachments => _attachmentPoints;

		internal bool HasDoor(Door door)
		{
			return Attachments.Contains(x => x.door == door);
		}

		internal void UpdateAttachments(AttachmentPoint<Hall>[] attachments)
		{
			_attachmentPoints = attachments;
		}

		internal static Vector3Int DoorToDirection(Door door)
		{
			switch (door)
			{
				case Door.Left:
					return Vector3Int.left;
				case Door.Right:
					return Vector3Int.right;
				case Door.Top:
					return Vector3Int.up;
				case Door.Bottom:
					return Vector3Int.down;
			}

			throw new Exception("door error");
		}

		internal static Door DirectionToDoor(Vector3Int direction)
		{
			if (direction == Vector3Int.up)
			{
				return Door.Top;
			}
			else if (direction == Vector3Int.down)
			{
				return Door.Bottom;
			}
			else if (direction == Vector3Int.right)
			{
				return Door.Right;
			}
			else if (direction == Vector3Int.left)
			{
				return Door.Left;
			}
			else
			{
				return Door.Top;
			}

			throw new Exception("invalid direction");
		}
	}

	[Serializable]
	internal class AttachmentPoint<T>
	{
		[SerializeField] private Transform transform;
		 
		public Door door;
		public Vector3 position => transform.position;
		
		public T AttachedTo { get; set; }
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class Room : MonoBehaviour
	{
		[SerializeField] AttachmentPoint[] _attachmentPoints;

		internal AttachmentPoint[] Attachments => _attachmentPoints;
	}

	[Serializable]
	internal struct AttachmentPoint
	{
		[SerializeField] private Transform transform;
		 
		public Door door;
		public Vector3 position => transform.position;
		
		public Room AttachedTo { get; set; }
	}
}

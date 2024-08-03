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

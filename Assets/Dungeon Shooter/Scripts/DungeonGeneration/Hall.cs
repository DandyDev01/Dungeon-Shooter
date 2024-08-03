using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.DungenGeneration
{
	public class Hall : MonoBehaviour
	{
		[SerializeField] AttachmentPoint<Room>[] _attachmentPoints;

		internal AttachmentPoint<Room>[] Attachments => _attachmentPoints;
	}
}

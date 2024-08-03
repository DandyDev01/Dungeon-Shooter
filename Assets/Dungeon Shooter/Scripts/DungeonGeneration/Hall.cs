using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.DungenGeneration
{
	public class Hall : MonoBehaviour
	{
		[SerializeField] AttachmentPoint[] _attachmentPoints;

		internal AttachmentPoint[] Attachments => _attachmentPoints;
	}
}

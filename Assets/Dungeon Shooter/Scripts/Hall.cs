using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall : MonoBehaviour
{
	[SerializeField] AttachmentPoint[] _attachmentPoints;

	internal AttachmentPoint[] Attachments => _attachmentPoints;
}

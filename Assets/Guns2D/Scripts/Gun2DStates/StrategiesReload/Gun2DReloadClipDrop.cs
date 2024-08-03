using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public class Gun2DReloadClipDrop : Gun2DReload
    {
		[SerializeField] private GameObject clipPrefab;
		[SerializeField] private Transform dropTransform;

		protected override void Reload(Gun2D gun)
		{
			base.Reload(gun);
			Instantiate(clipPrefab, dropTransform.position, Quaternion.identity);
		}
	}
}

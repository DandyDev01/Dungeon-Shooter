using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ReloadAction/CLipDrop")]
	public class ReloadActionClipDrop : Gun2DReloadAction
	{
		[SerializeField] private GameObject clipPrefab;

		public override void Activate(Gun2D gun)
		{
			float x = gun.transform.position.x;
			float y = gun.transform.position.y;

			Instantiate(clipPrefab,
					new Vector2(Random.Range(x - 0.5f, x + 0.5f), Random.Range(y - 0.5f, y + 0.5f)),
					Random.rotation);
		}
	}
}

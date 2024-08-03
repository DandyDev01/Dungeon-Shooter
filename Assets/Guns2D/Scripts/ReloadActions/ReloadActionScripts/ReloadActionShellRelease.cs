using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ReloadAction/ShellRelease")]
	public class ReloadActionShellRelease : Gun2DReloadAction
	{
		[SerializeField] GameObject shellPrefab;

		public override void Activate(Gun2D gun)
		{
			int shellsReleased = gun.ClipSize - gun.ClipAmmo;

			float x = gun.transform.position.x;
			float y = gun.transform.position.y;

			for (int i = 0; i < shellsReleased; i++)
			{
				Instantiate(shellPrefab,
					new Vector2(Random.Range(x - 0.5f, x + 0.5f), Random.Range(y - 0.5f, y + 0.5f)),
					Random.rotation);
			}
		}
	}
}

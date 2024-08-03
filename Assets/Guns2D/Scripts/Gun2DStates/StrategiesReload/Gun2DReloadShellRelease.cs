using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public class Gun2DReloadShellRelease : Gun2DReload
    {
		[SerializeField] protected Transform dropTransform;
        [SerializeField] protected GameObject shellPrefab;
		[SerializeField] protected float dropSpread;

		protected override void Reload(Gun2D gun)
		{

			int shellsReleased = gun.ClipSize - gun.ClipAmmo;
			float x = dropTransform.position.x;
			float y = dropTransform.position.y;
			base.Reload(gun);
			for (int i = 0; i < shellsReleased; i++)
			{
				Instantiate(shellPrefab, 
					new Vector2(Random.Range(x-dropSpread, x+dropSpread), Random.Range(y-dropSpread, y+dropSpread)),
					Random.rotation);
			}
		}
	}
}

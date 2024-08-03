using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ShootActions/ShellRelease")]
	public class ShootActionShellRelease : Gun2DShootAction
	{
		[SerializeField] GameObject shellPrefab;

		public override void Activate(Gun2D gun)
		{
			float x = gun.transform.position.x;
			float y = gun.transform.position.y;
			Instantiate(shellPrefab, gun.transform.position, Quaternion.identity);
			Instantiate(shellPrefab,
					new Vector2(Random.Range(x - 0.5f, x + 0.5f), Random.Range(y - 0.5f, y + 0.5f)),
					Random.rotation);
		}
	}
}

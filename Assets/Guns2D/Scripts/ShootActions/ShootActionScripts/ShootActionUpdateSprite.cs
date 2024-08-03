using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ShootActions/UpdateSprite")]
	public class ShootActionUpdateSprite : Gun2DShootAction
	{
		[SerializeField] protected Sprite sprite;

		public override void Activate(Gun2D gun)
		{
			SpriteRenderer spriteRenderer = gun.GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
		}
	}
}

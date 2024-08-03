using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ShootActions/FlashGraphic")]
	public class ShootActionFlashGraphic : Gun2DShootAction
	{
		public override void Activate(Gun2D gun)
		{
			gun.StartCoroutine(FlashGraphic(gun));
		}

		protected IEnumerator FlashGraphic(Gun2D gun)
		{
			gun.FlashGraphic.SetActive(true);
			yield return new WaitForSeconds(0.07f);
			gun.FlashGraphic.SetActive(false);
		}
	}
}

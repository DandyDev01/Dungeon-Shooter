using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ShootActions/MultiCreateProjectile")]
	public class ShootActionMultiProjectile : ShootActionCreateProjectile
	{
		[SerializeField] protected int numberOfProjectiles;
		[SerializeField] protected float timeBetweenShots;
		[SerializeField] protected ShootActionFlashGraphic graphicAction;

		public override void Activate(Gun2D gun)
		{
			gun.StartCoroutine(MultiShot(gun));
		}

		private IEnumerator MultiShot(Gun2D gun)
		{
			for (int i = 0; i < numberOfProjectiles; i++)
			{
				base.Activate(gun);
				graphicAction.Activate(gun);
				yield return new WaitForSeconds(timeBetweenShots);
			}
		}
	}
}

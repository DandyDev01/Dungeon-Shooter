using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	[CreateAssetMenu(menuName = "ShootActions/CreateProjectile")]
	public class ShootActionCreateProjectile : Gun2DShootAction
	{
		public override void Activate(Gun2D gun)
		{
			Projectile p = Instantiate(gun.ProjectilePrefab, gun.ShootTransform.position, gun.transform.rotation);
			Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
			rb.AddForce(gun.transform.up * gun.LaunchForce, ForceMode2D.Impulse);
			p.Init(1);
			gun.ClipAmmo -= 1;
		}
	}
}

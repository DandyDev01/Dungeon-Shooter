using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{

    [CreateAssetMenu(menuName = "ShootActions/ProjectileSpread")]
    public class ShootActionProjectileSpread : Gun2DShootAction
	{
		[SerializeField] int numberOfProjectiles;
		[SerializeField] float angle;
        [SerializeField] float timeBetweenCreations;

		public override void Activate(Gun2D gun)
		{
            // placement of each projectile for incrementation
            float angleBtwProjectiles = angle / numberOfProjectiles;
            // correct the shoot angle so that one projectile is at the center
            float offset = (angleBtwProjectiles * (numberOfProjectiles / 2));
            // angle to launch the projectile
            float angleBeforeOffset = gun.ShootTransform.rotation.eulerAngles.z;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                // add implementation for timer delay
                Rigidbody2D projectileRB = Instantiate(gun.ProjectilePrefab, gun.ShootTransform.position,
                         Quaternion.Euler(0, 0, angleBeforeOffset - offset)).GetComponent<Rigidbody2D>();

                Projectile projectile = projectileRB.gameObject.GetComponent<Projectile>();
                projectile.Init(1);

                projectileRB.AddForce(projectileRB.transform.up * gun.LaunchForce, ForceMode2D.Impulse);
               
                angleBeforeOffset += angleBtwProjectiles;
            }
            
            gun.ClipAmmo--;
        }
	}
}

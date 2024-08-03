using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	/// <summary>
	/// An implementation of Gun2DShootCharge where the projectile shot will chage
	/// based on how far along the charge is
	/// </summary>
	public class Gun2DShootChargeProjectileStages : Gun2DShootChargeFireOnRelease
	{
		[SerializeField] Projectile[] projectileStages;

		public override void Init(Gun2D gun, Gun2DState _idleState)
		{
			base.Init(gun, _idleState);
			gun.ProjectilePrefab = projectileStages[0];
		}

		protected override void Reset(Gun2D gun)
		{
			base.Reset(gun);
			gun.ProjectilePrefab = projectileStages[0];
		}

		protected override void UpdateChargeState(Gun2D gun)
		{
			base.UpdateChargeState(gun);
			gun.ProjectilePrefab = projectileStages[gfxIndex];
		}
	}
}

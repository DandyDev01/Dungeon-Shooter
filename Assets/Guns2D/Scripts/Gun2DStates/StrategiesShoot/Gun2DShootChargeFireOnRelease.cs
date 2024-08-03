using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	/// <summary>
	/// An implementation of Gun2DShootCharge where you do not need to be fully charged to shoot
	/// </summary>
    public class Gun2DShootChargeFireOnRelease : Gun2DShootCharge
    {
		public override Gun2DState Run(Gun2D gun, float delta)
		{
			if (!hasDoneFirstRun)
			{
				gfxIndex = 0;
				gfxUpdateTimer = timeToCharge / gfx.Length;
				spriteRenderer.sprite = gfx[gfxIndex];
				hasDoneFirstRun = true;
				if(gun.ClipAmmo > 0)
					gun.FlashGraphic.SetActive(true);
			}

			// update gfx
			if (gfxTimePassed >= gfxUpdateTimer && (gfxIndex + 1) < gfx.Length)
			{
				UpdateChargeState(gun);
			}

			timePassed += delta;
			gfxTimePassed += delta;

			// no longer charging, reset time passed
			if (fireIputDown)
			{
				// enough time has passed, start charging again
				if (Time.time > gun.FireRate + timeLastShotFired)
				{
					// there is enough ammo to start charging
					if (gun.ClipAmmo >= 1)
					{
						timeLastShotFired = Time.time;
						timePassed = 0;
						gfxTimePassed = 0;
						foreach (var item in actions)
						{
							item.Activate(gun);
						}
					}
				}

				Reset(gun);
				return idleState;
			}
			else
			{
				return this;
			}
		}
	}
}

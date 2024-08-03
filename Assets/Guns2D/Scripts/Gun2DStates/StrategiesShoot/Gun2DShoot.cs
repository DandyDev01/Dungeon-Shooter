using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	public class Gun2DShoot : Gun2DState
	{
		[SerializeField] protected Gun2DShootAction[] actions;
		protected float timeLastShotFired;
		protected bool fireIputDown;

		public override Gun2DState Run(Gun2D gun, float delta)
		{
			if (!hasDoneFirstRun)
			{
				hasDoneFirstRun = true;
			}

			// enough time has passed, can create another projectile
			if(Time.time > gun.FireRate + timeLastShotFired)
			{
				// there is enough ammo to shoot
				if(gun.ClipAmmo >= 1)
				{
					timeLastShotFired = Time.time;
					foreach (var item in actions)
					{
						item.Activate(gun);
					}
				}
			}

			if (!fireIputDown)
			{
				hasDoneFirstRun = false;
				return idleState;
			}
			else
				return this;
		}

		public void SetFireInputReleased(bool b)
		{
			fireIputDown = b;
		}
	}
}

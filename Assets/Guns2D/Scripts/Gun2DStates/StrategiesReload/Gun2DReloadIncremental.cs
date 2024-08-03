using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public class Gun2DReloadIncremental : Gun2DReload
    {
		protected float timeBetweenIncrements = 0.1f;
		protected bool finished = false;

		public override Gun2DState Run(Gun2D gun, float delta)
		{
			// don't need to reload, exit state
			if (gun.ClipAmmo == gun.ClipSize || gun.CurrentAmmo <= 0) return idleState;


			// do the first run
			if (!hasDoneFirstRun)
			{
				timeBetweenIncrements = gun.ReloadTime / gun.ClipSize;
				reloadTime = gun.ReloadTime;
				timePassed = 0;
				hasDoneFirstRun = true;
				finished = false;
				Reload(gun);
			}

			timePassed += delta;

			if (!finished)
			{
				return this;
			}
			else
			{
				hasDoneFirstRun = false;
				return idleState;
			}
		}

		protected override void Reload(Gun2D gun)
		{
			foreach (var item in actions)
			{
				item.Activate(gun);
			}
			StartCoroutine(Increment(gun));
		}

		protected virtual IEnumerator Increment(Gun2D gun)
		{
			int ammoNeeded = gun.ClipSize - gun.ClipAmmo;
			reloadTime = timeBetweenIncrements * ammoNeeded;

			for(int i = 0; i < ammoNeeded; i++)
			{
				yield return new WaitForSeconds(timeBetweenIncrements);
				if(gun.CurrentAmmo > 0)
				{
					gun.CurrentAmmo--;
					gun.ClipAmmo++;
				}
			}

			finished = true;
		}
	}
}

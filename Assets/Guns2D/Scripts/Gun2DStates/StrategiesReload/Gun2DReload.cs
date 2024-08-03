using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	public class Gun2DReload : Gun2DState
	{
		[SerializeField] protected Gun2DReloadAction[] actions;
		[SerializeField] private bool canInteruppt = false;
		protected float timePassed = 0;
		protected float reloadTime;

		public bool CanInteruppt { get { return canInteruppt; } }

		public override Gun2DState Run(Gun2D gun, float delta)
		{
			// don't need to reload, exit state
			if (gun.ClipAmmo == gun.ClipSize || gun.CurrentAmmo <= 0) return idleState;


			// do the first run
			if (!hasDoneFirstRun)
			{
				reloadTime = gun.ReloadTime;
				timePassed = 0;
				hasDoneFirstRun = true;
			}

			timePassed += delta;

			// enough time has passed, to to idle state
			if (timePassed >= reloadTime)
			{
				Reload(gun);

				hasDoneFirstRun = false;
				return idleState;
			}
			// not enough time has passed, continue reloading
			else
				return this;
		}

		protected virtual void Reload(Gun2D gun)
		{
			int ammoNeeded = gun.ClipSize - gun.ClipAmmo;

			// there is enough ammo to fully refill clip
			if (ammoNeeded < gun.CurrentAmmo)
			{
				gun.ClipAmmo += ammoNeeded;
				gun.CurrentAmmo -= ammoNeeded;
			}
			// there is enough ammo to partially refill clip
			else
			{
				gun.ClipAmmo += gun.CurrentAmmo;
				gun.CurrentAmmo = 0;
			}

			foreach (var item in actions)
			{
				item.Activate(gun);
			}
		}
	}
}

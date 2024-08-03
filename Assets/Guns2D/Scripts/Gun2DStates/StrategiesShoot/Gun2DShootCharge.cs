using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	/// <summary>
	/// A shoot strategy that will not launch a projectile until the trigger has been held down
	/// for some specified amount of time
	/// </summary>
	public class Gun2DShootCharge : Gun2DShoot
	{
		[SerializeField] protected float timeToCharge = 1;
		protected float timePassed;

		[Header("GFX")]
		[SerializeField] protected Sprite[] gfx;
		protected SpriteRenderer spriteRenderer;
		protected float gfxUpdateTimer = 0;
		protected float gfxTimePassed = 0;
		protected int gfxIndex = 0;

		public override void Init(Gun2D gun, Gun2DState _idleState)
		{
			base.Init(gun, _idleState);
			spriteRenderer = gun.FlashGraphic.GetComponentInChildren<SpriteRenderer>();
		}

		public override Gun2DState Run(Gun2D gun, float delta)
		{
			if (!hasDoneFirstRun)
			{
				gfxIndex = 0;
				gfxUpdateTimer = timeToCharge / gfx.Length;
				gun.FlashGraphic.SetActive(true);
				spriteRenderer.sprite = gfx[gfxIndex];
				hasDoneFirstRun = true;
			}

			// update gfx
			if(gfxTimePassed >= gfxUpdateTimer && (gfxIndex + 1) < gfx.Length)
			{
				UpdateChargeState(gun);
			}

			// enough time has passed, start charging again
			if (Time.time > gun.FireRate + timeLastShotFired)
			{
				// there is enough ammo to start charging
				if (gun.ClipAmmo >= 1)
				{
					// enough time has passed, create projectile
					if(timePassed >= timeToCharge)
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
			}

			timePassed += delta;
			gfxTimePassed += delta;

			// no longer charging, reset time passed
			if (fireIputDown)
			{
				Reset(gun);
				return idleState;
			}
			else
			{
				return this;
			}
		}

		/// <summary>
		/// rest the state
		/// </summary>
		/// <param name="gun"></param>
		protected virtual void Reset(Gun2D gun)
		{
			spriteRenderer.sprite = gfx[0];
			timePassed = 0;
			hasDoneFirstRun = false;
			gun.FlashGraphic.SetActive(false);
		}

		protected virtual void UpdateChargeState(Gun2D gun)
		{
			gfxTimePassed = 0;
			gfxIndex++;
			spriteRenderer.sprite = gfx[gfxIndex];
		}
	}
}

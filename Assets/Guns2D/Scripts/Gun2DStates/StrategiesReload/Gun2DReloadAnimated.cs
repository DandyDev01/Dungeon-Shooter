using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public class Gun2DReloadAnimated : Gun2DReloadIncremental
    {
		[SerializeField] protected Sprite[] frames;
		protected SpriteRenderer spriteRenderer;
		protected int currentFrame = 0;

		public override void Init(Gun2D gun, Gun2DState _idleState)
		{
			base.Init(gun, _idleState);
			spriteRenderer = gun.GetComponentInChildren<SpriteRenderer>();
		}

		protected override IEnumerator Increment(Gun2D gun)
		{
			timeBetweenIncrements = gun.ReloadTime / frames.Length;

			// cycle through animatino frames
			for (int i = 0; i < frames.Length; i++)
			{
				yield return new WaitForSeconds(timeBetweenIncrements);
				if (currentFrame + 1 < frames.Length)
				{
					currentFrame++;
					spriteRenderer.sprite = frames[currentFrame];
				}
			}

			// add ammo
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

			finished = true;
			currentFrame = 0;
		}
	}
}

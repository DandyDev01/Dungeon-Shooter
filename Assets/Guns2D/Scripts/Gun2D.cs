using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    /// <summary>
    /// this class is responsible for all things Gun
    /// </summary>
    public class Gun2D : MonoBehaviour
    {
        [Header("Fire/Reload Settings")]
        [SerializeField] private float launchForce = 30f;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float reloadTime = 1.0f;
       
        [Header("Ammo Settings")]
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private int currentAmmo = 30;
        [SerializeField] private int clipSize = 8;
        [SerializeField] private int clipAmmo = 8;

        [Header("SFX")]
        [SerializeField] private AudioClip fireClip;
        [SerializeField] private AudioClip reloadClip;

        [Header("GFX")]
        [SerializeField] private GameObject flashGraphic;

        [Header("Other")]
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform shootTransform;

        private Gun2DState idleState;
        private Gun2DShoot shootState;
        private Gun2DReload reloadState;
        private Gun2DState activeState;

        public float LaunchForce { get { return launchForce; } }
        public float FireRate { get { return fireRate; } }
        public float ReloadTime { get { return reloadTime; } }
        public int MaxAmmo { get { return maxAmmo; } }
        public int CurrentAmmo { get { return currentAmmo; } set { currentAmmo = value; }}
        public int ClipSize { get { return clipSize; } }
        public int ClipAmmo { get { return clipAmmo; } set { clipAmmo = value; } }
        public Projectile ProjectilePrefab { get { return projectilePrefab; } set { projectilePrefab = value; } }
        public Transform ShootTransform { get { return shootTransform; } }
        public GameObject FlashGraphic { get { return flashGraphic; } }
        public AudioClip FireClip { get { return fireClip; } }
        public AudioClip ReloadClip { get { return reloadClip; } }

        /// <summary>
        /// pass true when fire input is down false when up
        /// </summary>
        public event Action<bool> OnFire;
        /// <summary>
        /// pass true when fire input is down false when up
        /// </summary>
        public event Action<bool> OnFireEnd;
        public event Action OnReload;

        /// <summary>
        /// MUST BE CALLED! initilize the object. Used instead of Start.
        /// </summary>
        public virtual void Init()
		{
            shootState = GetComponentInChildren<Gun2DShoot>();
            idleState = GetComponentInChildren<Gun2DIdle>();
            reloadState = GetComponentInChildren<Gun2DReload>();
            activeState = idleState;

            shootState.Init(this, idleState);
            reloadState.Init(this, idleState);

            flashGraphic.SetActive(false);

            OnFire += shootState.SetFireInputReleased;
            OnFireEnd += shootState.SetFireInputReleased;

		}

        public void RunActiveState(float delta)
		{
            activeState = activeState.Run(this, delta);
		}

        /// <summary>
        /// the fire trigger is activated
        /// </summary>
        public void Fire()
		{
            if (activeState is Gun2DReload r)
            {
                if (!r.CanInteruppt) return;
            }
            //if (activeState is Gun2DShoot) return;

            activeState = shootState;
            OnFire?.Invoke(true);
		}

        /// <summary>
        /// the fire trigger is deactivated
        /// </summary>
        public void EndFire()
		{
            OnFireEnd?.Invoke(false);
		}

        /// <summary>
        /// the reload trigger is activated
        /// </summary>
        public void Reload()
        {
            if (activeState is Gun2DShoot) return;

            //if(activeState is Gun2DReload) return;

            activeState = reloadState;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public class Gun2DInput : MonoBehaviour
    {

        [SerializeField] private Gun2D gun;

        // Start is called before the first frame update
        void Start()
        {
            gun.Init();
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            gun.RunActiveState(Time.deltaTime);
        }

		private void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
                gun.Fire();
			}
            else if (Input.GetKeyUp(KeyCode.Mouse0))
			{
                gun.EndFire();
			}

            if (Input.GetKeyDown(KeyCode.R))
                gun.Reload();
		}
	}
}

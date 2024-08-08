using DungeonShooter.Player;
using Guns2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class GunInteractable : InteractableBase
	{
		[SerializeField] private Gun2D _pickup;

		public override void Interact(PlayerCharacter player)
		{
			Gun2D newGun = Instantiate(_pickup, Vector3.zero, Quaternion.identity);
			player.SwitchGun(newGun);
		}
	}
}
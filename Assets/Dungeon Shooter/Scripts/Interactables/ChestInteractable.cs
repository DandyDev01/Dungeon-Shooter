using DungeonShooter.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class ChestInteractable : InteractableBase
	{
		[SerializeField] private GameObject[] _lootOptions;

		public override void Interact(PlayerCharacter player)
		{
			Vector3 spawnLocation = transform.position + Vector3.down;

			int index = UnityEngine.Random.Range(0, _lootOptions.Length);

			Instantiate(_lootOptions[index], spawnLocation, Quaternion.identity);
		}
	}
}
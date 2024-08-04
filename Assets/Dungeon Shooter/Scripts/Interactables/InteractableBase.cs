using DungeonShooter.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public abstract class InteractableBase : MonoBehaviour
	{
		public abstract void Interact(PlayerCharacter player);
	}
}
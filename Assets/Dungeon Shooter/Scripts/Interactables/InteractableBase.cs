using DungeonShooter.Player;
using UnityEngine;

namespace DungeonShooter
{
	public abstract class InteractableBase : MonoBehaviour
	{
		public abstract void Interact(PlayerCharacter player);
	}
}
using DungeonShooter;
using DungeonShooter.Player;
using UnityEngine;

public class HealthInteractable : InteractableBase
{
    [SerializeField] private int _healAmount = 1;

	public override void Interact(PlayerCharacter player)
	{
		player.Health.Heal(_healAmount);
	}
}

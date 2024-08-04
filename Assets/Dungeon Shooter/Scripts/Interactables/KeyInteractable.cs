using DungeonShooter.Player;

namespace DungeonShooter
{
	public class KeyInteractable : InteractableBase
{
		public override void Interact(PlayerCharacter player)
		{
			player.PickupBossRoomKey(this);
		}
	}
}

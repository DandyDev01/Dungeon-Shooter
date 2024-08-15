using DungeonShooter.Player;
using UnityEngine;
using TMPro;

namespace DungeonShooter
{
	public abstract class InteractableBase : MonoBehaviour
	{
		private Canvas _interactionCanvas;

		private void Awake()
		{
			_interactionCanvas = GetComponentInChildren<Canvas>();
			CloseInteractionCanvas();
		}

		public abstract void Interact(PlayerCharacter player);

		public void OpenInteractionCanvas()
		{
			_interactionCanvas.gameObject.SetActive(true);
		}

		public void CloseInteractionCanvas()
		{
			_interactionCanvas.gameObject.SetActive(false);
		}

		public void SetMessage(string message)
		{
			var textbox = _interactionCanvas.GetComponentInChildren<TextMeshPro>();
			textbox.text = message;
		}
	}
}
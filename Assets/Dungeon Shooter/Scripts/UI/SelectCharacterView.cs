using DungeonShooter.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonShooter.UI
{
	public class SelectCharacterView : MonoBehaviour
	{
		[SerializeField] private GameController _gameController;

		[SerializeField] private PlayerCharacter _player1Prefab;
		[SerializeField] private PlayerCharacter _player2Prefab;

		[SerializeField] private Button _selectPlayer1Buton;
		[SerializeField] private Button _selectPlayer2Buton;

		private void Awake()
		{
			_selectPlayer1Buton.onClick.AddListener( delegate 
			{ 
				_gameController.SetSelectedPlayerCharacter(_player1Prefab); 
				gameObject.SetActive(false);
			});			
			_selectPlayer2Buton.onClick.AddListener( delegate 
			{ 
				_gameController.SetSelectedPlayerCharacter(_player2Prefab); 
				gameObject.SetActive(false);
			});			
		}
	}
}
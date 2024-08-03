using DungeonShooter.DungenGeneration;
using DungeonShooter.Player;
using DungeonShooter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonShooter
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private SelectCharacterView _selectCharacterView;
		[SerializeField] private DungeonBuilder _dungeonBuilder;

		private PlayerCharacter _selectPlayerCharacter;

		private void Awake()
		{
			SceneManager.LoadSceneAsync("SpacePortScene", LoadSceneMode.Additive);
		}

		private void Start()
		{

		}

		public void SetSelectedPlayerCharacter(PlayerCharacter player)
		{
			_selectPlayerCharacter = player;
			SpawnPlayer();
		}

		public void SpawnPlayer()
		{
			Instantiate(_selectPlayerCharacter, Vector3.zero, Quaternion.identity);
		}

		public void EnterDungeon()
		{
			SceneManager.UnloadSceneAsync("SpacePortScene");

			_dungeonBuilder.Build();
		}
	}
}
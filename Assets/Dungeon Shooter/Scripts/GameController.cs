using DungeonShooter.DungenGeneration;
using DungeonShooter.Player;
using DungeonShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonShooter
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private DungeonBuilder _dungeonBuilder;

		[Header("UI")]
		[SerializeField] private GameObject _hud;
		[SerializeField] private SelectCharacterView _selectCharacterView;

		private PlayerCharacter _selectPlayerCharacter;

		private void Awake()
		{
			_hud.gameObject.SetActive(false);
			SceneManager.LoadSceneAsync("SpacePortScene", LoadSceneMode.Additive);
		}

		private void Start()
		{

		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				SceneManager.LoadScene(0);
				Console.Clear();
			}
		}

		public void SetSelectedPlayerCharacter(PlayerCharacter player)
		{
			_selectPlayerCharacter = player;
			SpawnPlayer();
		}

		public void SpawnPlayer()
		{
			_selectPlayerCharacter = Instantiate(_selectPlayerCharacter, Vector3.zero, Quaternion.identity);
		}

		public void EnterDungeon()
		{
			SceneManager.UnloadSceneAsync("SpacePortScene");
			_hud.gameObject.SetActive(true);

			DungeonGrid grid = _dungeonBuilder.GetComponentInChildren<DungeonGrid>();
			Vector3 centerWorld = new Vector3((grid.Oragin.x + grid.Columns * grid.CellSize) / 2,
				(grid.Oragin.y + grid.Rows * grid.CellSize) / 2);
			_selectPlayerCharacter.transform.position = centerWorld;

			_dungeonBuilder.Build();

			_dungeonBuilder.InitRooms();
		}
	}
}
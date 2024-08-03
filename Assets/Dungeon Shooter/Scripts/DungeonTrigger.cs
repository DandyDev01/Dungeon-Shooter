using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrigger : MonoBehaviour
{
	private GameController _gameController;

	private void Awake()
	{
		_gameController = FindAnyObjectByType<GameController>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_gameController.EnterDungeon();
	}
}

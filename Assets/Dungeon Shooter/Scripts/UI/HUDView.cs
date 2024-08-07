using DungeonShooter.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

	private void Start()
	{
		PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

		player.Health.OnHealthChanged += UpdateHealth;

		_healthSlider.maxValue = player.Health.MaxHealth;
		_healthSlider.value = _healthSlider.maxValue;
	}

	private void OnDestroy()
	{
		PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
		player.Health.OnHealthChanged -= UpdateHealth;
	}

	private void UpdateHealth(int currentHealth, int maxHealth)
	{
		_healthSlider.maxValue = maxHealth;
		_healthSlider.value = currentHealth;
	}
}

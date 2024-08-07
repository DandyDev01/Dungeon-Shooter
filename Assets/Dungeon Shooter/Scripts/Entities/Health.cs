using System;
using UnityEngine;

namespace DungeonShooter
{
	public class Health
	{
		public int Current { get; private set; }
		public int MaxHealth { get; private set; }

		public Action OnDeath;
		public Action<int, int> OnHealthChanged;

		public Health(int maxHealth)
		{
			Current = maxHealth;
			MaxHealth = maxHealth; 
		}

		public void Damage(int damage)
		{
			Current = Mathf.Max(Current - damage, 0);

			if (Current == 0)
				OnDeath?.Invoke();

			OnHealthChanged?.Invoke(Current, MaxHealth);
		}

		public void Heal(int amount)
		{
			Current = Mathf.Min(Current + amount, MaxHealth);
			OnHealthChanged?.Invoke(Current, MaxHealth);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField] float _speed;

		[Header("States")]
		[SerializeField] private EnemyBaseState _moveState;
		[SerializeField] private EnemyBaseState _attackState;
		[SerializeField] private EnemyBaseState _spawnState;

		private Health _health;
		private Animator _animator;
		private float _speedModifier = 1;

		public Health Health => _health;

		public EnemyBaseState CurrentState { get; set; }

		private void Awake()
		{
			_health = new Health(5);
			_animator = GetComponentInChildren<Animator>();
			
			
		}

		private void Update()
		{
			CurrentState.Run();
		}

		public void PlayAnimation(string name)
		{
			_animator.Play(name);
		}
	}

}
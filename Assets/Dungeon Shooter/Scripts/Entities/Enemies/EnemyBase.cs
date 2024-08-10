using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public abstract class EnemyBase : MonoBehaviour
	{
		[SerializeField] protected float _speed;
		[SerializeField] protected float _attackDistance = 1f;
		[SerializeField] protected Transform _playerTransform;

		protected Health _health;
		protected Animator _animator;
		protected float _speedModifier = 1;

		public Health Health => _health;
		public float AttackDistance => _attackDistance;
		public float SpeedModifier { get => _speedModifier; set => _speedModifier = value; }

		protected virtual void Awake()
		{
			_health = new Health(5);
			_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
			_animator = GetComponentInChildren<Animator>();
		}

		/// <summary>
		/// Plays a specified animation.
		/// </summary>
		/// <param name="name">Name of the animation to play.</param>
		public void PlayAnimation(string name)
		{
			_animator.Play(name);
		}

		/// <summary>
		/// Get the position of the enemies target.
		/// </summary>
		/// <returns>Position of the target.</returns>
		public Vector2 GetTargetPosition()
		{
			return _playerTransform.position;
		}

		/// <summary>
		/// Moves the enemy around the world to their target.
		/// </summary>
		public void Move()
		{
			transform.position = Vector2.MoveTowards(transform.position, GetTargetPosition(), _speed * _speedModifier * Time.deltaTime);
		}
	}
}
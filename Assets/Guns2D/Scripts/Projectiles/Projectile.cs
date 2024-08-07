using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public enum PlayerEffectType { NONE, STICKY, INVINCIBLE };

    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected int damage;
        [SerializeField] protected PlayerEffectType[] effects;
        
        public int Damage { get { return damage; } }
        public PlayerEffectType[] Effects => effects;

        public virtual void Init(int damageMod = 1) 
        {
            damage *= damageMod;
        }
    }
}

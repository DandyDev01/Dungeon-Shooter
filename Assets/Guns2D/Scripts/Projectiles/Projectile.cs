using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected int damage;
        
        public int Damage { get { return damage; } }

        public virtual void Init(int damageMod = 1) 
        {
            damage *= damageMod;
        }
    }
}

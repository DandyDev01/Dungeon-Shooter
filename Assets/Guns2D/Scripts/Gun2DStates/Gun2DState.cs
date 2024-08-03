using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public abstract class Gun2DState : MonoBehaviour
    {
        protected Gun2DState idleState;
        protected bool hasDoneFirstRun = false;

        public abstract Gun2DState Run(Gun2D gun, float delta);

        public virtual void Init(Gun2D gun, Gun2DState _idleState)
		{
            idleState = _idleState;
		}
    }
}

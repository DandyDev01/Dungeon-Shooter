using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
    public abstract class Gun2DReloadAction : ScriptableObject
    {
        public abstract void Activate(Gun2D gun);
    }

}
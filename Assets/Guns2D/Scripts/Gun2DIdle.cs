using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns2D
{
	public class Gun2DIdle : Gun2DState
	{
		public override Gun2DState Run(Gun2D gun, float delta)
		{
			return this;
		}
	}
}

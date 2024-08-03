using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
	public bool IsDisabled { get; set; }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsDisabled)
			return;

		Debug.Log("Outch!");
	}
}

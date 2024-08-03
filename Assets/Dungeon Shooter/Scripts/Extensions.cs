using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using DungeonShooter.DungenGeneration;

namespace DungeonShooter
{
	public static class Extensions
	{
		public static bool Approx(this Vector3 me, Vector3 other)
		{
			float x1 = me.x;
			float y1 = me.y;
			float z1 = me.z;
			float x2 = other.x;
			float y2 = other.y;
			float z2 = other.z;

			if (Mathf.Approximately(x1, x2) && Mathf.Approximately(y1, y2) && Mathf.Approximately(z1, z2))
				return true;

			return false;
		}

		internal static Vector3Int DoorToDirection(this Vector3Int me, Door door)
		{
			switch (door) 
			{
				case Door.Left:
					return Vector3Int.left;
				case Door.Right:
					return Vector3Int.right;
				case Door.Top:
					return Vector3Int.up;
				case Door.Bottom:
					return Vector3Int.down;
			}

			throw new Exception("door error");
		}

		internal static Door DirectionToDoor(this Vector3Int me, Vector3Int direction)
		{
			if (direction == Vector3Int.up)
			{
				return Door.Top;
			}
			else if (direction == Vector3Int.down)
			{
				return Door.Bottom;
			}
			else if (direction == Vector3Int.right)
			{
				return Door.Right;
			}
			else if (direction == Vector3Int.left)
			{
				return Door.Left;
			}
			else
			{
				return Door.Top;
			}

			throw new Exception("invalid direction");
		}

		public static T GetRandom<T>(this T[] array)
		{
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static T GetRandom<T>(this IEnumerable<T> array)
		{
			return array.ElementAt(UnityEngine.Random.Range(0, array.Count()));
		}

		public static bool Contains<T>(this T[] collection, Func<T, bool> p)
		{
			return collection.Where(p).Any();
		}

		public static bool Contains<T>(this List<T> collection, Func<T, bool> p)
		{
			return collection.Where(p).Any();
		}

		public static Vector3Int ToVec3Int(this Vector3 vector)
		{
			return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
		}

	}

}
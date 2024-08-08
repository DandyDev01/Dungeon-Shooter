using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

		public static Transform GetChildWhere(this Transform me, System.Func<Transform, bool> x)
		{
			List<Transform> list = new List<Transform>();

			for (int i = 0; i < me.childCount; i++)
			{
				list.Add(me.GetChild(i));
			}

			var options = list.Where(x);
		
			if (options.Any())
				return options.First();


			return null;
		}

		public static Transform[] GetChildrenWhere(this Transform me, System.Func<Transform, bool> x)
		{
			List<Transform> list = new List<Transform>();

			for (int i = 0; i < me.childCount; i++)
			{
				list.Add(me.GetChild(i));
			}

			var options = list.Where(x);

			return options.ToArray();
		}

		internal static Vector3Int DoorToDirection(this Vector3Int me, DoorLocation door)
		{
			switch (door) 
			{
				case DoorLocation.Left:
					return Vector3Int.left;
				case DoorLocation.Right:
					return Vector3Int.right;
				case DoorLocation.Top:
					return Vector3Int.up;
				case DoorLocation.Bottom:
					return Vector3Int.down;
			}

			throw new Exception("door error");
		}

		internal static DoorLocation DirectionToDoor(this Vector3Int me, Vector3Int direction)
		{
			if (direction == Vector3Int.up)
			{
				return DoorLocation.Top;
			}
			else if (direction == Vector3Int.down)
			{
				return DoorLocation.Bottom;
			}
			else if (direction == Vector3Int.right)
			{
				return DoorLocation.Right;
			}
			else if (direction == Vector3Int.left)
			{
				return DoorLocation.Left;
			}
			else
			{
				return DoorLocation.Top;
			}

			throw new Exception("invalid direction");
		}

		public static T GetRandom<T>(this T[] array)
		{
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static T GetRandom<T>(this IEnumerable<T> array)
		{
			if (array.Count() == 0)
				return default(T);

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
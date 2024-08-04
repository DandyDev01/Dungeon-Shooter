using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DungeonShooter.DungenGeneration
{
	public class DungeonGrid : MonoBehaviour
	{
		[SerializeField] private Vector3 oragin = Vector3.zero;
		[SerializeField] private int columns = 1;
		[SerializeField] private int rows = 1;
		[SerializeField] private float cellSize = 1;

		private GridXY<List<Tuple<Room, bool>>> grid;

		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin { get { return oragin; } }

		public GridXY<List<Tuple<Room, bool>>> Grid => grid;

		public void Awake()
		{
			grid = new GridXY<List<Tuple<Room, bool>>>(oragin, columns, Rows, CellSize);
			InitilizeCells();
		}

		private void Start()
		{
		}

		private void InitilizeCells()
		{
			Room[] dungeonBuilderRooms = transform.parent.GetComponent<DungeonBuilder>().Rooms;

			for (int col = 0; col < columns; col++)
			{
				for (int row = 0; row < rows; row++)
				{
					var list = new List<Tuple<Room, bool>>();
					foreach (Room room in dungeonBuilderRooms)
					{
						list.Add(new Tuple<Room, bool>(room, false));
					}
					grid.SetElement(col, row, list);
				}
			}
		}

		public List<Room> GetRooms()
		{
			List<Room> rooms = new();

			for (int col = 0; col < columns; col++)
			{
				for (int row = 0; row < rows; row++)
				{
					List<Tuple<Room, bool>> optioins = grid.GetElement(col, row);
					if (optioins.Count == 1 && optioins.First().Item2)
						rooms.Add(optioins.First().Item1);
				}
			}

			return rooms;
		}

		public Vector3Int GetCell(Room room)
		{
			for (int col = 0; col < columns; col++)
			{
				for (int row = 0; row < rows; row++)
				{
					if (grid.GetElement(col, row).Count == 1 && grid.GetElement(col, row).First().Item2)
						return new Vector3Int(col, row);
				}
			}

			return Vector3Int.one * -1;
		}

		public Vector3 WorldPosition(int row, int col)
		{
			return grid.GetWorldPosition(col, row);
		}
	}
}

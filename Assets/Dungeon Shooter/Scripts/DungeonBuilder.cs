using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;


namespace DungeonShooter
{
	internal enum Door { Left, Right, Top, Bottom };

	public class DungeonBuilder : MonoBehaviour
	{
		private const int MAX_ROOMS = 12;
		private const int MIN_ROOMS = 6;
		
		[SerializeField] private Room[] _rooms;

		private GridXY<List<Tuple<Room, bool>>> _grid;
		private int _currentRoomCount = 0;

		public Room[] Rooms => _rooms;

		private void Awake()
		{
		}

		private void Start()
		{
			_grid = FindObjectOfType<DungeonGrid>().Grid;
			//Build();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.B))
				Build();
		}

		public void Build()
		{
			Vector3 centerWorld = new Vector3((_grid.Oragin.x + _grid.Columns * _grid.CellSize) / 2,
				(_grid.Oragin.y + _grid.Rows * _grid.CellSize) / 2);
			Vector3Int currentCell = _grid.GetCellPosition(centerWorld);

			Room currentRoom =_grid.GetElement(currentCell.x, currentCell.y).GetRandom().Item1;

			for (int i = 0; i < MAX_ROOMS; i++)
			{
				Instantiate(currentRoom, _grid.GetWorldPosition(currentCell.x, currentCell.y), Quaternion.identity);
				_currentRoomCount += 1;

				List<Tuple<Room, bool>> currentRoomOptions = _grid.GetElement(currentCell.x, currentCell.y).ToList();
				currentRoomOptions.Clear();
				currentRoomOptions.Add(new Tuple<Room, bool>(currentRoom, true));
				_grid.SetElement(currentCell.x, currentCell.y, currentRoomOptions);
				
				UpdateNeibours(currentCell, currentRoom);

				Vector3Int neighbourCellWithLeastOptions = GetNeighbourCellWithLeastOptions(currentCell);

				Tuple<Room, bool> roomToPlace = _grid.GetElement(neighbourCellWithLeastOptions.x, 
					neighbourCellWithLeastOptions.y).GetRandom();
				
				_grid.SetElement(neighbourCellWithLeastOptions.x, neighbourCellWithLeastOptions.y,
					new List<Tuple<Room, bool>> { new Tuple<Room, bool>(roomToPlace.Item1, true) });
				
				currentRoom = roomToPlace.Item1;
				currentCell = neighbourCellWithLeastOptions;

			}
		}

		private Vector3Int GetNeighbourCellWithLeastOptions(Vector3Int cell)
		{
			Vector3Int[] neighbourCells = _grid.GetNeighbourCells(cell.x, cell.y).ToArray();
			List<Tuple<Room, bool>> neighbourWithLeastOptions = null;
			Vector3Int cellOfNeighbourWithLeastOptions = Vector3Int.zero;
			foreach (Vector3Int neighbourCell in neighbourCells)
			{
				List<Tuple<Room, bool>> neighbourOptions = _grid.GetElement(neighbourCell.x, neighbourCell.y)
					.Where(x => x.Item2 == false).ToList();

				if (neighbourOptions.Any() == false)
					continue;

				if (neighbourWithLeastOptions == null)
				{
					neighbourWithLeastOptions = neighbourOptions;
					cellOfNeighbourWithLeastOptions = neighbourCell;
					continue;
				}

				if (neighbourWithLeastOptions.Count > neighbourOptions.Count)
				{
					neighbourWithLeastOptions = neighbourOptions;
					cellOfNeighbourWithLeastOptions = neighbourCell;
				}
			}

			return cellOfNeighbourWithLeastOptions;
		}

		private void UpdateNeibours(Vector3Int cellOfRoot, Room root)
		{
			Vector3Int[] neighbourCells = _grid.GetNeighbourCells(cellOfRoot.x, cellOfRoot.y).ToArray();

			foreach (var neighbourCell in neighbourCells)
			{
				List<Tuple<Room, bool>> neighbourCellsOptions = _grid.GetElement(neighbourCell.x, neighbourCell.y);

				Door rootToNeighbourDoor = neighbourCell.DirectionToDoor(neighbourCell - cellOfRoot);
				List<Tuple<Room, bool>> validNeighbourCellOptions = GetValidOptions(root, neighbourCellsOptions, 
					neighbourCell, cellOfRoot, rootToNeighbourDoor);
				_grid.SetElement(neighbourCell.x, neighbourCell.y, validNeighbourCellOptions);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="root">Room that cell neighbours</param>
		/// <param name="possibleOptions">possible options</param>
		/// <param name="cellToGetValidOptionsFrom">cell to get options for</param>
		/// <param name="rootCell">cell the root room is in</param>
		/// <returns>valid options</returns>
		private List<Tuple<Room, bool>> GetValidOptions(Room root, List<Tuple<Room, bool>> possibleOptions, 
			Vector3Int cellToGetValidOptionsFrom, Vector3Int rootCell, Door rootDoorThatLeadsToNeighbour)
		{
			// There are no valid options because root does not have a door facing cell.
			// NOTE: these ifs are causing all neighbours to have no valid. Comment out. Won't fix bug, but are issue
			if (root.Attachments.Contains(x => x.door == rootDoorThatLeadsToNeighbour) == false)
				return new List<Tuple<Room, bool>>();	

			List<Tuple<Room, bool>> valid = new();

			//this cell previously did not have any valid options, now there are some valid options.
			if (possibleOptions.Any() == false)
			{
				// if neighbour has room, then room with door to that neighbour is not valid
				Vector3Int[] cellsThatNeighbourCellToGetValidOptionsFrom = _grid.GetNeighbourCells(cellToGetValidOptionsFrom.x,
					cellToGetValidOptionsFrom.y).ToArray();

				foreach (Vector3Int neighbourCell in cellsThatNeighbourCellToGetValidOptionsFrom)
				{
					// since rootCell is a neighbour of cellToGetValidOptions, skip it.
					if (neighbourCell == rootCell)
					{
						continue;
					}

					if (_grid.GetElement(neighbourCell.x, neighbourCell.y).Count() == 1
						&& _grid.GetElement(neighbourCell.x, neighbourCell.y).First().Item2)
					{
						foreach (Room possibleOption in _rooms)
						{
							if (possibleOption.Attachments.Contains(x => neighbourCell.DoorToDirection(x.door) + cellToGetValidOptionsFrom == neighbourCell))
							{
								continue;
							}
					
							possibleOptions.Add(new Tuple<Room, bool>(possibleOption, false));
						}
					}
				}
			}

			// remove options that do not have a door facing root
			foreach (var possibleOption in possibleOptions)
			{
				if (possibleOption.Item1.Attachments.Contains(x => x.door == GetValidDoor(rootDoorThatLeadsToNeighbour))
					&& possibleOption.Item1.Attachments.Count() >= 2)
				{
					valid.Add(possibleOption);
				}
			}

			return valid;
		}

		internal Door GetValidDoor(Door door)
		{
			switch (door)
			{
				case Door.Left:
					return Door.Right;
				case Door.Right:
					return Door.Left;
				case Door.Top:
					return Door.Bottom;
				case Door.Bottom:
					return Door.Top;
			}

			throw new Exception("Error with doors");
		}
	}

	[Serializable]
	internal class BuilderRoom
	{
		public Door[] doors;
		public GameObject room;
	}
}

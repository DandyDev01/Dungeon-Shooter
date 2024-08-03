using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;


namespace DungeonShooter.DungenGeneration
{
	internal enum Door { Left, Right, Top, Bottom };

	public class DungeonBuilder : MonoBehaviour
	{
		private const int MAX_ROOMS = 12;
		private const int MIN_ROOMS = 6;
		
		[SerializeField] private Room[] _rooms;
		[SerializeField] private Hall[] _halls;

		private GridXY<List<Tuple<Room, bool>>> _grid;
		private int _currentRoomCount = 0;

		public Room[] Rooms => _rooms;

		private void Start()
		{
			_grid = GetComponentInChildren<DungeonGrid>().Grid;
		}

		/// <summary>
		/// Generates the dungeon layout.
		/// </summary>
		public void Build()
		{
			Vector3 centerWorld = new Vector3((_grid.Oragin.x + _grid.Columns * _grid.CellSize) / 2,
				(_grid.Oragin.y + _grid.Rows * _grid.CellSize) / 2);
			Vector3Int currentCell = _grid.GetCellPosition(centerWorld);

			Room currentRoom =_grid.GetElement(currentCell.x, currentCell.y).GetRandom().Item1;
			Room previousRoom = null;

			for (int i = 0; i < MAX_ROOMS; i++)
			{
				List<Tuple<Room, bool>> currentRoomOptions = _grid.GetElement(currentCell.x, currentCell.y).ToList();
				currentRoomOptions.Clear();
				currentRoomOptions.Add(new Tuple<Room, bool>(currentRoom, true));
				_grid.SetElement(currentCell.x, currentCell.y, currentRoomOptions);
				
				UpdateNeighbourOptions(currentCell, currentRoom);

				Vector3Int neighbourCellWithLeastOptions = GetNeighbourCellWithLeastOptions(currentCell);

				if (neighbourCellWithLeastOptions == Vector3Int.zero)
					break;

				Tuple<Room, bool> newRoom = _grid.GetElement(neighbourCellWithLeastOptions.x, 
					neighbourCellWithLeastOptions.y).GetRandom();
				
				previousRoom = currentRoom;

				// create and link rooms together with halls
				currentRoom = Instantiate(newRoom.Item1, _grid.GetWorldPosition(neighbourCellWithLeastOptions.x, neighbourCellWithLeastOptions.y), Quaternion.identity);
				currentRoom.transform.parent = transform;

				Vector3Int directionOfCurrentRoomFromPreviousRoom = neighbourCellWithLeastOptions - currentCell;
				
				Hall hall = _halls.Where(x => x.Attachments.Contains(y => y.door == currentCell.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom))).First();
				hall = Instantiate(hall, Vector2.zero, Quaternion.identity);
				hall.transform.parent = transform;

				AttachmentPoint<Room> hallToPreviousRoomAttachmentPoint = hall.Attachments.Where(x => x.door == currentCell.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom*-1)).First();
				AttachmentPoint<Room> hallToCurrentRoomAttachmentPoint = hall.Attachments.Where(x => x.door == currentCell.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom)).First();
				AttachmentPoint<Hall> previousRoomAttachmentPoint = previousRoom.Attachments.Where(x => x.door == currentCell.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom)).First();
				AttachmentPoint<Hall> currentRoomAttachmentPoint = currentRoom.Attachments.Where(x => x.door == currentCell.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom * -1)).First();

				hallToPreviousRoomAttachmentPoint.AttachedTo = previousRoom;
				hallToCurrentRoomAttachmentPoint.AttachedTo = currentRoom;
				previousRoomAttachmentPoint.AttachedTo = hall;
				currentRoomAttachmentPoint.AttachedTo = hall;

				hall.transform.position = hall.transform.position + (previousRoomAttachmentPoint.position - hall.transform.position) - (hallToPreviousRoomAttachmentPoint.position - hall.transform.position);

				currentRoom.transform.position = currentRoom.transform.position + (hallToCurrentRoomAttachmentPoint.position - currentRoom.transform.position) - (currentRoomAttachmentPoint.position - currentRoom.transform.position);

				currentCell = neighbourCellWithLeastOptions;
			
				_grid.SetElement(currentCell.x, currentCell.y,
					new List<Tuple<Room, bool>> { new Tuple<Room, bool>(currentRoom, true) });
				
				
				_currentRoomCount += 1;
			}

			//TODO: Update rooms that have unlinked doors.
		}

		/// <summary>
		/// Get the neighbour cell that has the least options for which room will go into that cell.
		/// </summary>
		/// <param name="cell">Cell to get the neighbour with the least options.</param>
		/// <returns>Cell with the least options that also neighbours the specified cell. Returns Vector3Int.zero when nothing can be placed.</returns>
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
				else if (neighbourWithLeastOptions.Count == neighbourOptions.Count)
				{
					int random = UnityEngine.Random.Range(0, 10);
					if (random < 5)
					{
						neighbourWithLeastOptions = neighbourOptions;
						cellOfNeighbourWithLeastOptions = neighbourCell;
					}
				}
			}

			return cellOfNeighbourWithLeastOptions;
		}

		/// <summary>
		/// Update the options that of all cells that neighbour cellOfRoot.
		/// </summary>
		/// <param name="cellOfRoot">Cell to get the neighbours of and update their options.</param>
		/// <param name="root">Room that is inside cellOfRoot.</param>
		private void UpdateNeighbourOptions(Vector3Int cellOfRoot, Room root)
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
						continue;

					if (_grid.GetElement(neighbourCell.x, neighbourCell.y).Count() == 1
						&& _grid.GetElement(neighbourCell.x, neighbourCell.y).First().Item2)
					{
						foreach (Room possibleOption in _rooms)
						{
							if (possibleOption.Attachments.Contains(x => neighbourCell.DoorToDirection(x.door) + cellToGetValidOptionsFrom == neighbourCell))
								continue;
					
							possibleOptions.Add(new Tuple<Room, bool>(possibleOption, false));
						}
					}
				}
			}

			int leastDoorsAllowed = _currentRoomCount >= MIN_ROOMS ? 1 : 2;
			int maxDoorsAllowed = _currentRoomCount < MAX_ROOMS - 1 ? MAX_ROOMS - _currentRoomCount : 1;

			// remove options that do not have a door facing root
			foreach (var possibleOption in possibleOptions)
			{
				if (possibleOption.Item1.Attachments.Contains(x => x.door == GetValidDoor(rootDoorThatLeadsToNeighbour))
					&& possibleOption.Item1.Attachments.Count() >= leastDoorsAllowed && possibleOption.Item1.Attachments.Count() <= maxDoorsAllowed)
				{
					valid.Add(possibleOption);
				}
			}

			return valid;
		}

		/// <summary>
		/// Get a door can connect to the specified door.
		/// </summary>
		/// <param name="door">Door to get a valid door for.</param>
		/// <returns>Door that connects to the specified door.</returns>
		/// <exception cref="Exception">When no valid do is found</exception>
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
}

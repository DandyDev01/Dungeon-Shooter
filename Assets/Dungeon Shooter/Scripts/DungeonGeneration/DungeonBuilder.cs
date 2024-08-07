using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
		private DungeonGrid _dungenGrid;
		private int _currentRoomCount = 0;

		public Room[] Rooms => _rooms;

		private void Start()
		{
			_dungenGrid = GetComponentInChildren<DungeonGrid>();
			_grid = _dungenGrid.Grid;
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

				// all neighbours have zero options
				if (neighbourCellWithLeastOptions == Vector3Int.zero)
				{
					Debug.Log("try again");
					continue;
				}

				Tuple<Room, bool> newRoom = _grid.GetElement(neighbourCellWithLeastOptions.x, 
					neighbourCellWithLeastOptions.y).GetRandom();
				
				previousRoom = currentRoom;

				// create and link rooms together with halls
				currentRoom = Instantiate(newRoom.Item1, _grid.GetWorldPosition(neighbourCellWithLeastOptions.x, neighbourCellWithLeastOptions.y), Quaternion.identity);
				currentRoom.transform.parent = transform;

				Vector3Int directionOfCurrentRoomFromPreviousRoom = neighbourCellWithLeastOptions - currentCell;
				
				Hall hall = _halls.Where(x => x.Attachments.Contains(y => y.door == Room.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom))).First();
				hall = Instantiate(hall, Vector2.zero, Quaternion.identity);
				hall.transform.parent = transform;

				AttachmentPoint<Room> hallToPreviousRoomAttachmentPoint;
				AttachmentPoint<Room> hallToCurrentRoomAttachmentPoint;
				AttachmentPoint<Hall> previousRoomAttachmentPoint;
				AttachmentPoint<Hall> currentRoomAttachmentPoint;

				try
				{
					hallToPreviousRoomAttachmentPoint = hall.Attachments.Where(x => x.door == Room.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom * -1)).First();
					hallToCurrentRoomAttachmentPoint = hall.Attachments.Where(x => x.door == Room.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom)).First();
					previousRoomAttachmentPoint = previousRoom.Attachments.Where(x => x.door == Room.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom)).First();
					currentRoomAttachmentPoint = currentRoom.Attachments.Where(x => x.door == Room.DirectionToDoor(directionOfCurrentRoomFromPreviousRoom * -1)).First();
				}
				catch
				{
					break;
				}

				hallToPreviousRoomAttachmentPoint.AttachedTo = previousRoom;
				hallToCurrentRoomAttachmentPoint.AttachedTo = currentRoom;
				previousRoomAttachmentPoint.AttachedTo = hall;
				currentRoomAttachmentPoint.AttachedTo = hall;

				hall.transform.position = hall.transform.position + (previousRoomAttachmentPoint.position - hall.transform.position) - (hallToPreviousRoomAttachmentPoint.position - hall.transform.position);

				//currentRoom.transform.position = currentRoom.transform.position + (hallToCurrentRoomAttachmentPoint.position - currentRoom.transform.position) - (currentRoomAttachmentPoint.position - currentRoom.transform.position);

				currentCell = neighbourCellWithLeastOptions;
			
				_grid.SetElement(currentCell.x, currentCell.y,
					new List<Tuple<Room, bool>> { new Tuple<Room, bool>(currentRoom, true) });
				
				_currentRoomCount += 1;
			}

			// cases: door that leads to an empty cell, door that leads to a cell that is not empty, door leads down hall that leads no where.
			List<Room> rooms = _dungenGrid.GetRooms();

			foreach (Room room in rooms)
			{
				if (room.Attachments.Contains(x => x.AttachedTo == null))
					UpdateRoom(room);
			}
		}

		/// <summary>
		/// Changes the room so that it only has doors that are connected to halls
		/// </summary>
		/// <param name="room">room that will be changed</param>
		private void UpdateRoom(Room room)
		{
			Vector3Int cell = _dungenGrid.GetCell(room);
			Vector3 worldPosition = room.transform.position;
			Room newRoom = null;
			List<Door> doors = new();

			foreach (AttachmentPoint<Hall> attachmentPoint in room.Attachments)
			{
				if (attachmentPoint.AttachedTo == null)
					continue;

				doors.Add(attachmentPoint.door);
			}

			foreach (Room possibleOption in _rooms)
			{
				if (possibleOption.Attachments.Contains(x => doors.Contains(x.door) == false) || possibleOption.Attachments.Length < doors.Count)
					continue;

				newRoom = possibleOption;
				break;
			}

			if (newRoom == null)
			{
				Debug.LogError("Could not find replacment for ", room.gameObject);
				return;
			}

			newRoom = Instantiate(newRoom, worldPosition, Quaternion.identity);
			newRoom.UpdateAttachments(room.Attachments.Where(x => x.AttachedTo != null).ToArray());

			Transform old = transform.GetChildWhere(x => x.position == newRoom.transform.position);

			newRoom.transform.parent = transform;
			_grid.SetElement(cell.x, cell.y, new List<Tuple<Room, bool>>() { new Tuple<Room,bool>(newRoom, true) });

			Destroy(old.gameObject);
		}

		[Obsolete("Use UpdateRoom instead.")]
		private void AddAttachmentsToRoomsMissingAttachments(Room room)
		{
			// add hall and room to rooms that have door going no where.
			if (room.Attachments.Contains(x => x.AttachedTo == null))
			{
				Vector3Int cellOfRoomMissingAttachments = _dungenGrid.GetCell(room);
				
				AttachmentPoint<Hall>[] missingAttachmentPoints = room.Attachments.Where((x => x.AttachedTo == null)).ToArray();

				foreach (AttachmentPoint<Hall> attachmentPoint in missingAttachmentPoints)
				{
					Hall hall = Instantiate(_halls.Where(x => x.Attachments.Contains(y => y.door == attachmentPoint.door)).First(), Vector3.zero, Quaternion.identity);
					hall.transform.parent = transform;
					attachmentPoint.AttachedTo = hall;

					Vector3Int[] neighbours = _grid.GetNeighbourCells(cellOfRoomMissingAttachments.x, cellOfRoomMissingAttachments.y).ToArray();
					Vector3Int cellOfNewRoom = neighbours.Where(n => cellOfRoomMissingAttachments + Room.DoorToDirection(attachmentPoint.door) == n).First();
					List<Door> doorsNeededByNewRoom = GetNeededDoors(cellOfNewRoom);
					Room[] newRoomOptions = _rooms;
					List<Room> validOptions = new();

					Debug.Log("Doors need by new room: " + doorsNeededByNewRoom.Count + cellOfNewRoom.ToString(), hall.gameObject);
					foreach (var item in doorsNeededByNewRoom)
					{
						Debug.Log(item.ToString());
					}
					Debug.Log("====================================" + "\n");

					//// remove optons that have doors other than the needed ones.
					//foreach (Room option in newRoomOptions)
					//{
					//	bool isValid = true;
					//	foreach (AttachmentPoint<Hall> item in option.Attachments)
					//	{
					//		if (doorsNeededByNewRoom.Contains(item.door) == false)
					//		{
					//			isValid = false;
					//			break;
					//		}
					//	}

					//	if (isValid)
					//		validOptions.Add(option);
					//}

					AttachmentPoint<Room> hallToRoom = hall.Attachments.Where(x => x.door == GetOpositeDoor(attachmentPoint.door)).First();
					hallToRoom.AttachedTo = room;

					//AttachmentPoint<Room> hallToNewRoom = hall.Attachments.Where(x => x.door == attachmentPoint.door).First();

					//if (validOptions.Count() == 0)
					//	Debug.Log("error");

					//Room newRoom = Instantiate(validOptions.GetRandom(), Vector3.zero, Quaternion.identity);
					//newRoom.transform.parent = transform;
					//newRoom.Attachments.First().AttachedTo = hall;
					//hallToNewRoom.AttachedTo = newRoom;

					//AttachmentPoint<Hall> newRoomToHall = newRoom.Attachments.Where(x => x.door == GetOpositeDoor(hallToNewRoom.door)).First();

					hall.transform.position = hall.transform.position + (attachmentPoint.position - hall.transform.position) - (hallToRoom.position - hall.transform.position);
					//newRoom.transform.position = newRoom.transform.position + (hallToNewRoom.position - newRoom.transform.position) - (newRoomToHall.position - newRoom.transform.position);

					//_grid.SetElement(cellOfNewRoom.x, cellOfNewRoom.y, new List<Tuple<Room, bool>>() { new Tuple<Room, bool>(newRoom, true) });

					//UpdateNeighbourOptions(cellOfNewRoom, newRoom);
				}
			}
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

			foreach (var neighbourToUpdateCell in neighbourCells)
			{
				List<Tuple<Room, bool>> neighbourCellsOptions = _grid.GetElement(neighbourToUpdateCell.x, neighbourToUpdateCell.y);

				Door rootToNeighbourDoor = Room.DirectionToDoor(neighbourToUpdateCell - cellOfRoot);
				
				List<Tuple<Room, bool>> validNeighbourCellOptions = GetValidOptions(root, neighbourCellsOptions, 
					neighbourToUpdateCell, rootToNeighbourDoor);

				_grid.SetElement(neighbourToUpdateCell.x, neighbourToUpdateCell.y, validNeighbourCellOptions);
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
			Vector3Int cellToGetValidOptionsFrom, Door rootDoorThatLeadsToNeighbour)
		{
			// There are no valid options because root does not have a door facing cell.
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
					if (neighbourCell == _dungenGrid.GetCell(root))
						continue;

					// neighbour cell has a set room.
					if (_grid.GetElement(neighbourCell.x, neighbourCell.y).Count() == 1
						&& _grid.GetElement(neighbourCell.x, neighbourCell.y).First().Item2)
					{

						// get rooms facing the neighbour
						foreach (Room possibleOption in _rooms)
						{
							if (possibleOption.Attachments.Contains(x => Room.DoorToDirection(x.door) == (neighbourCell - cellToGetValidOptionsFrom)))
								continue;

							possibleOptions.Add(new Tuple<Room, bool>(possibleOption, false));
						}
					}
				}
			}

			int leastDoorsAllowed = _currentRoomCount >= MIN_ROOMS ? 1 : 2;
			int maxDoorsAllowed = _currentRoomCount < MAX_ROOMS - 1 ? MAX_ROOMS - _currentRoomCount : 1;

			// Remove options that do not have a door facing all neighbours with a door facing it.
			foreach (Tuple<Room, bool> possibleOption in possibleOptions)
			{
				List<Door> neededDoors = GetNeededDoors(cellToGetValidOptionsFrom);

				foreach (Door door in neededDoors)
				{
					if (possibleOption.Item1.Attachments.Contains(x => x.door == door)
						&& possibleOption.Item1.Attachments.Count() >= leastDoorsAllowed
						&& possibleOption.Item1.Attachments.Count() <= maxDoorsAllowed)
					{
						valid.Add(possibleOption);
						break;
					}
				}


				//if (possibleOption.Item1.Attachments.Contains(x => x.door == GetValidDoor(rootDoorThatLeadsToNeighbour))
				//	&& possibleOption.Item1.Attachments.Count() >= leastDoorsAllowed 
				//	&& possibleOption.Item1.Attachments.Count() <= maxDoorsAllowed)
				//{
				//	valid.Add(possibleOption);
				//}
			}

			return valid;
		}

		/// <summary>
		/// Get the doors that a room in the specified cell would need in order to connect to its neighbours.
		/// </summary>
		/// <param name="cellToGetNeededDoorsFor">Cell of room you need the doors for.</param>
		/// <returns>Doors that the room in the specified cell would need to connect to its neighbours.</returns>
		private List<Door> GetNeededDoors(Vector3Int cellToGetNeededDoorsFor)
		{
			List<Door> neededDoors = new List<Door>();
			foreach (Vector3Int neighbour in _grid.GetNeighbourCells(cellToGetNeededDoorsFor.x, cellToGetNeededDoorsFor.y))
			{
				List<Tuple<Room, bool>> neighbourOptions = _grid.GetElement(neighbour.x, neighbour.y);
				
				// neighbour does not have a set room, so ther are no doors to connect to.
				if ((neighbourOptions.Count > 1 && neighbourOptions.Contains(x => x.Item2 == false)) || neighbourOptions.Count == 0)
					continue;

				Room neighbourRoom = neighbourOptions.First().Item1;

				if (neighbourRoom.HasDoor(Room.DirectionToDoor(cellToGetNeededDoorsFor - neighbour)))
				{
					neededDoors.Add(Room.DirectionToDoor(neighbour - cellToGetNeededDoorsFor));
				}
			}

			return neededDoors;
		}

		/// <summary>
		/// Get a door can connect to the specified door.
		/// </summary>
		/// <param name="door">Door to get a valid door for.</param>
		/// <returns>Door that connects to the specified door.</returns>
		/// <exception cref="Exception">When no valid do is found</exception>
		internal Door GetOpositeDoor(Door door)
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

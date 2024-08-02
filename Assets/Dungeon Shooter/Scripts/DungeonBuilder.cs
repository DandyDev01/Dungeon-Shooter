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
		
		[SerializeField] private BuilderRoom[] _builderRooms;
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
				foreach (Room possibleOption in _rooms)
				{
					// if neighbour has room, then room with door to that neighbour is not valid
					Vector3Int[] cellsThatNeighbourCellToGetValidOptionsFrom = _grid.GetNeighbourCells(cellToGetValidOptionsFrom.x,
						cellToGetValidOptionsFrom.y).ToArray();

					bool isValid = true;
					foreach (Vector3Int neighbourCell in cellsThatNeighbourCellToGetValidOptionsFrom)
					{
						// since rootCell is a neighbour of cellToGetValidOptions, skip it.
						if (neighbourCell == rootCell)
						{
							isValid = false;
							break;
						}

						if (_grid.GetElement(neighbourCell.x, neighbourCell.y).Count() == 1
							&& _grid.GetElement(neighbourCell.x, neighbourCell.y).First().Item2)
						{
							// if room has a door that leads to neighbour cell skip it
							if (possibleOption.Attachments.Contains(x => neighbourCell.DoorToDirection(x.door) + neighbourCell == neighbourCell))
							{
								isValid = false;
								break;
							}
						}
					}
					if (isValid)
						possibleOptions.Add(new Tuple<Room, bool>(possibleOption, false));
				}
			}

			// remove options that do not have a door facing root
			foreach (var possibleOption in possibleOptions)
			{
				if (possibleOption.Item1.Attachments.Contains(x => x.door == GetValidDoor(rootDoorThatLeadsToNeighbour)))
				{
					valid.Add(possibleOption);
				}
			}

			return valid;
		}

		internal BuilderRoom GetValidRoom(Door door, Room attachTo)
		{
			bool hasValidRoom = false;
			BuilderRoom validRoom = new();
			do
			{
			 //&&attachTo.Attachments.Contains(j => GetValidDoor(j.door) == x && j.AttachedTo == null))
				BuilderRoom room = _builderRooms.GetRandom();
				if (room.doors.Contains(x => x == GetValidDoor(door)))
				{
					validRoom = room;
					hasValidRoom = true;
				}
			} 
			while (hasValidRoom == false);

			return validRoom;
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

		private void LegacyBuild()
		{

			int numberOfRoomsPlaces = 0;
			int roomIndex = 0;
			Room[] placedRooms = new Room[12];
			BuilderRoom[] rooms = new BuilderRoom[MAX_ROOMS];

			BuilderRoom currentRoom = _builderRooms.GetRandom();
			rooms[numberOfRoomsPlaces] = currentRoom;

			Room attachTo = Instantiate(rooms[0].room, Vector3.zero, Quaternion.identity).GetComponent<Room>();
			placedRooms[0] = attachTo;

			numberOfRoomsPlaces += 1;

			// get rooms to build
			foreach (BuilderRoom room in rooms)
			{
				foreach (Door door in room.doors)
				{
					if (attachTo.Attachments.Contains(x => x.door == door && x.AttachedTo != null))
						continue;

					BuilderRoom validNeihbour = GetValidRoom(door, attachTo);

					Room newRoom = Instantiate(validNeihbour.room, Vector3.zero, Quaternion.identity).GetComponent<Room>();
					placedRooms[numberOfRoomsPlaces] = newRoom;

					// link rooms

					// get the attachment point from attachTo for door
					AttachmentPoint[] attachmentPoints = attachTo.Attachments.Where(x => x.door == door).ToArray();
					AttachmentPoint attachToLink;
					if (attachmentPoints.Length > 0)
						attachToLink = attachmentPoints.First();
					else
						throw new Exception("No attachment point found for door: " + door);

					attachToLink.AttachedTo = newRoom;

					// get the attachment point from newRoom that will plug into door
					attachmentPoints = newRoom.Attachments.Where(x => GetValidDoor(x.door) == door).ToArray();
					AttachmentPoint newRoomLink;
					if (attachmentPoints.Length > 0)
						newRoomLink = attachmentPoints.First();
					else
						throw new Exception("No attachment point found for door: " + door);

					newRoomLink.AttachedTo = attachTo;
					Vector2 roomPos = newRoom.transform.position;
					Vector2 roomAncherPos = newRoomLink.position;
					Vector2 target = attachToLink.position;


					newRoom.gameObject.transform.position = roomPos + (target - roomPos) - (roomAncherPos - roomPos);

					rooms[numberOfRoomsPlaces] = validNeihbour;
					numberOfRoomsPlaces += 1;

					if (numberOfRoomsPlaces == MAX_ROOMS)
						break;
				}

				roomIndex += 1;
				attachTo = placedRooms[roomIndex];

				if (numberOfRoomsPlaces == MAX_ROOMS)
					break;
			}

			// replace rooms with excess doors
		}
	}

	[Serializable]
	internal class BuilderRoom
	{
		public Door[] doors;
		public GameObject room;
	}
}

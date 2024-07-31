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
		[SerializeField] private BuilderRoom[] _rooms;

		private const int MAX_ROOMS = 12;

		private void Awake()
		{
			Build();
		}

		public void Build()
		{
			int numberOfRoomsPlaces = 0;
			int roomIndex = 0;
			Room[] placedRooms = new Room[12];
			BuilderRoom[] rooms = new BuilderRoom[MAX_ROOMS];

			BuilderRoom currentRoom = _rooms.GetRandom();
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

					// link rooms

					Room r = Instantiate(validNeihbour.room, Vector3.zero, Quaternion.identity).GetComponent<Room>();
					placedRooms[numberOfRoomsPlaces] = r;

					var test = attachTo.Attachments.Where(x => x.door == door).First();
					test.AttachedTo = r;
					var t = r.Attachments.Where(x => GetValidDoor(x.door) == door).First();
					t.AttachedTo = attachTo;

					rooms[numberOfRoomsPlaces] = validNeihbour;
					numberOfRoomsPlaces += 1;
				}

				roomIndex += 1;
				attachTo = placedRooms[roomIndex];

				if (numberOfRoomsPlaces == MAX_ROOMS - 1)
					break;
			}

			// replace rooms with excess doors

		}

		private BuilderRoom GetValidRoom(Door door, Room attachTo)
		{
			bool hasValidRoom = false;
			BuilderRoom validRoom = new();
			do
			{
			 //&&attachTo.Attachments.Contains(j => GetValidDoor(j.door) == x && j.AttachedTo == null))
				BuilderRoom room = _rooms.GetRandom();
				if (room.doors.Contains(x => x == GetValidDoor(door)))
				{
					validRoom = room;
					hasValidRoom = true;
				}
			} 
			while (hasValidRoom == false);

			return validRoom;
		}

		private Door GetValidDoor(Door door)
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
	internal struct BuilderRoom
	{
		public Door[] doors;
		public GameObject room;
	}
}

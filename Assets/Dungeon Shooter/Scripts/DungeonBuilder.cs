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

		internal BuilderRoom GetValidRoom(Door door, Room attachTo)
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

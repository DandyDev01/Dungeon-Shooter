using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PathBuilder
{
	private GridXY<bool> _grid;
	private GameObject _marker;
	private List<GameObject> _markers = new();

	public PathBuilder(GridXY<bool> grid, GameObject marker)
	{
		_grid = grid;
		_marker = marker;
	}

	/// <summary>
	/// Find a path from the start location to the target location.
	/// </summary>
	/// <param name="target">Where the goal is.</param>
	/// <param name="start">Where the starting localtion is (initial state.)</param>
	/// <returns>A path from the start location to target location</returns>
	public List<Node> CalculatePath(Vector2 target, Vector2 start)
	{
		DeletePathMarkers();

		Vector3Int begin;
		Vector3Int end;

		// normalize the start location to grid coords (alignment)
		begin = _grid.GetCellPosition(start);
		start = _grid.GetWorldPosition(begin.x, begin.y);

		// normalize the target location to the grid coords (alignment)
		end = _grid.GetCellPosition(target);
		target = _grid.GetWorldPosition(end.x, end.y);

		Node root = CreateRoot(start);
		List<Node> nodes = new();
		bool goalFound = false;

		nodes.Add(root);

		int index = 0;
		Node current = root;
		while (goalFound == false && index < 1300)
		{
			// node is not within the bounds of the search space.
			if (_grid.IsInRange(current._worldPosition) == false)
			{
				throw new Exception("Out of range");
			}

			// exit condition, path to goal is found.
			if (Vector3.Distance(current._worldPosition, target) < 0.1f)
			{
				goalFound = true;
				break;
			}

			Vector3[] neighborsWorldPosition = _grid.GetNeighboursWorldPositions(current._worldPosition).ToArray();

			// get the neighboring cells of current that are traversable and not already marked.
			foreach (Vector3 neightbor in neighborsWorldPosition)
			{
				Vector3 cell = _grid.GetCellPosition(neightbor);
				bool value = _grid.GetElement((int)cell.x, (int)cell.y);
				if (value == false)
					continue;

				var traversableNeighbor = new Node(neightbor, current);
				if (nodes.Contains(x => x._worldPosition.Approx(traversableNeighbor._worldPosition)) == false)
				{
					nodes.Add(traversableNeighbor);
					GameObject g = GameObject.Instantiate(_marker, traversableNeighbor._worldPosition, Quaternion.identity);
					_markers.Add(g);
				}
			}

			index += 1;
			// could not find a path
			if (index >= nodes.Count)
			{
				// check for path again but with higher exit tolerance
				foreach (Node node in nodes)
				{
					if (Vector3.Distance(node._worldPosition, target) < 2)
					{
						current = node;
						break;
					}
				}

				Debug.Log("Issue. Cannot get to target: " + target);
			}

			current = nodes[index];
		}

		DeletePathMarkers();

		List<Node> path = BuildPath(root, current);

		// draw path
		foreach (var item in path)
		{
			GameObject g = GameObject.Instantiate(_marker, item._worldPosition, Quaternion.identity);
			_markers.Add(g);
		}

		return path;
	}

	private void DeletePathMarkers()
	{
		foreach (var item in _markers)
		{
			GameObject.Destroy(item);
		}
		_markers.Clear();
	}

	/// <summary>
	/// build a path from and end node to start node
	/// </summary>
	/// <param name="start">the start of the path</param>
	/// <param name="end">The end of the path</param>
	/// <returns>path from start to end.</returns>
	private static List<Node> BuildPath(Node start, Node end)
	{
		List<Node> path = new();

		int i = 0;
		while (end._worldPosition.Approx(start._worldPosition) == false && i < 1300)
		{
			path.Add(end);
			end = end._parent;
			i++;
		}

		path.Reverse();

		return path;
	}

	/// <summary>
	/// Helper method to setup the root node.
	/// </summary>
	/// <param name="start">Location of the root node.</param>
	/// <returns>The root node.</returns>
	private Node CreateRoot(Vector2 start)
	{
		Vector3[] neighborsWorldPosition = _grid.GetNeighboursWorldPositions(start).ToArray();

		List<Vector3> traversable = new();

		foreach (Vector3 neightbor in neighborsWorldPosition)
		{
			Vector3 cell = _grid.GetCellPosition(neightbor);
			bool value = _grid.GetElement((int)cell.x, (int)cell.y);
			if (value)
				traversable.Add(neightbor);

		}

		Node root = new Node(start, traversable.ToArray());

		foreach (var item in root._children)
		{
			item._parent = root;
		}

		return root;
	}
}


public class Node
{
	public readonly List<Node> _children = new();
	public readonly Vector3 _worldPosition;
	public Node _parent;

	public Node(Vector3 item)
	{
		_worldPosition = item;
	}

	public Node(Vector3 item, Node parent)
	{
		_worldPosition = item;
		_parent = parent;
	}

	public Node(Vector2 start, Vector3[] traverableNeighbors)
	{
		_worldPosition = start;
		foreach (var item in traverableNeighbors)
		{
			_children.Add(new Node(item));
		};
	}
}


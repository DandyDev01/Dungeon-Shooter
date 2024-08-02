using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static Codice.CM.Common.CmCallContext;

namespace Grid
{
	public class SampleGridXY : MonoBehaviour
	{
		[SerializeField] private Vector3 oragin = Vector3.zero;
		[SerializeField] private int columns = 1;
		[SerializeField] private int rows = 1;
		[SerializeField] private float cellSize = 1;

		private GridXY<bool> grid;

		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin { get { return oragin; } }

		public GridXY<bool> Grid => grid;

		public void Awake()
		{
			grid = new GridXY<bool>(oragin, columns, Rows, CellSize);
		}

		public Vector3 WorldPosition(int row, int col)
		{
			return grid.GetWorldPosition(col, row);
		}

	}
}

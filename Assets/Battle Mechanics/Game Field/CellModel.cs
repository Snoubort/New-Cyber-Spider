using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CellModel
{
    public bool IsOccupied = false;
    public ElementTypeParams OccupiedElementType;

    public Vector2Int ThisCellCordinates;
    public Vector2Int CenterElementCordinates;

    public List<Vector2Int> OtherCellsOfElementCordinates = new();

	public CellModel(Vector2Int _coordinates)
	{
		ThisCellCordinates = _coordinates;
		OccupiedElementType = null;
		CenterElementCordinates = new(-1, -1);
		IsOccupied = false;
	}

	public void ClearCell()
    {
		OccupiedElementType = null;
		CenterElementCordinates = new Vector2Int(-1, -1);
		OtherCellsOfElementCordinates.Clear();
		IsOccupied = false;
	}
}

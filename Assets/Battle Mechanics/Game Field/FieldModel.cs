using Mono.Cecil.Cil;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class FieldModel : MonoBehaviour
{
	[SerializeField]
	public int p_horizontalNumberOfCells;

	[SerializeField]
	public int p_verticalNumberOfCells;

	public CellModel[,] FieldCells;

	public List<Vector2Int> GhostCells = new();

	private void Awake()
	{
		FieldCells = new CellModel[p_horizontalNumberOfCells, p_verticalNumberOfCells];

		for (int x = 0; x < p_horizontalNumberOfCells; x++)
		{
			for (int y = 0; y < p_verticalNumberOfCells; y++)
			{
				FieldCells[x, y] = new CellModel(new(x, y));
			}
		}
	}

	public void RemoveElementFromField(Vector2Int CordinatesOfCellOfElement)
	{
		var selectedCell = FieldCells[CordinatesOfCellOfElement.x, CordinatesOfCellOfElement.y];

		if (selectedCell.IsOccupied)
		{
			if(selectedCell.ThisCellCordinates != selectedCell.CenterElementCordinates)
			{
				selectedCell = FieldCells[selectedCell.CenterElementCordinates.x, selectedCell.CenterElementCordinates.y];
			}

			foreach (var cellCordinates in selectedCell.OtherCellsOfElementCordinates)
			{
				var cell = FieldCells[cellCordinates.x, cellCordinates.y];
				cell.ClearCell();
			}
			selectedCell.ClearCell();
		}
	}

	public bool IsCanElementPlaced (ElementTypeParams _elementType, int _rotation, Vector2Int _placingCell)
	{
		if(!IsInsideBoard(_placingCell))
			return false;

		var elementInFieldCords = ElementFromLocalToFieldCordinates(_elementType.CellsOfElement, _rotation, _placingCell);

		foreach (var cell in elementInFieldCords)
		{
			if (!IsInsideBoard(cell))
				return false;

			if (FieldCells[cell.x, cell.y].IsOccupied)
				return false;
		}

		return true;
	}

	public void AddGhostCells(ElementTypeParams _elementType, int _rotation, Vector2Int _placingCell)
	{
		if (IsInsideBoard(_placingCell))
			GhostCells.Add(_placingCell);

		var fieldCellsCords =  ElementFromLocalToFieldCordinates(_elementType.CellsOfElement, _rotation, _placingCell);

		foreach( var cell in fieldCellsCords)
		{
			if(IsInsideBoard(cell))
				GhostCells.Add(cell);
		}
	}

	public bool PlaceElement(ElementTypeParams _elementType, int _rotation, Vector2Int _placingCell)
	{
		if(IsCanElementPlaced(_elementType, _rotation, _placingCell))
		{
			GhostCells.Clear();

			var cellCordinates = ElementFromLocalToFieldCordinates(_elementType.CellsOfElement, _rotation, _placingCell);

			foreach (var cell in cellCordinates)
			{
				var fieldCell = FieldCells[cell.x, cell.y];
				fieldCell.CenterElementCordinates = _placingCell;
				fieldCell.IsOccupied = true;
				fieldCell.OccupiedElementType = _elementType;
				fieldCell.OtherCellsOfElementCordinates = cellCordinates;
			}

			var centalCell = FieldCells[_placingCell.x, _placingCell.y];
			centalCell.CenterElementCordinates = _placingCell;
			centalCell.IsOccupied = true;
			centalCell.OccupiedElementType = _elementType;
			centalCell.OtherCellsOfElementCordinates = cellCordinates;

			return true;
		}

		return false;
	}

	private bool IsInsideBoard(Vector2Int cell)
	{
		return cell.x >= 0 && cell.x < p_horizontalNumberOfCells
			&& cell.y >= 0 && cell.y < p_verticalNumberOfCells;
	}

	private List<Vector2Int> ElementFromLocalToFieldCordinates(List<Vector2Int> _localCords, int _rotation, Vector2Int _placingCell)
	{
		List<Vector2Int> elementCordinatesOnField = new();
		elementCordinatesOnField.Add(_placingCell);

		foreach (var cell in _localCords)
		{
			var cellLocalRotatedCordinates = RotateElementCellInLocalCordinates(cell, _rotation * 90);
			var cellInFieldCordinates = _placingCell + cellLocalRotatedCordinates;
			elementCordinatesOnField.Add(cellInFieldCordinates);
		}

		return elementCordinatesOnField;
	}

	private Vector2Int RotateElementCellInLocalCordinates(Vector2Int _cell, int _angle)
	{
		int steps = ((_angle / 90) % 4 + 4) % 4;

		for (int i = 0; i < steps; i++)
		{
			_cell = new Vector2Int(_cell.y, -_cell.x);
		}

		return _cell;
	}
}

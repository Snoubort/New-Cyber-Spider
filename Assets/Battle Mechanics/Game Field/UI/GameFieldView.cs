using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldView : MonoBehaviour
{
	[SerializeField]
	private Camera p_playerCamera;

	[SerializeField]
	private GameObject p_BoardCellPrefab;

	[SerializeField]
	private float p_horizontalCellMargin;

	[SerializeField]
	private float p_verticalCellMargin;

	[SerializeField]
	private FieldModel p_fieldModel;

	private List<GameFieldUiElement> p_uiCells = new();

	public void DrawFieldFromModel(ElementTypeParams _cardType = null)
	{
		var ghostCells = p_fieldModel.GhostCells;
		var fieldCells = p_fieldModel.FieldCells;

		foreach (var uiCell in p_uiCells)
		{
			uiCell.SetDefaultColor();
			if(ghostCells.Contains(uiCell.FieldCordinates) && _cardType != null)
			{
				uiCell.SetElementColor(_cardType.ElementColor);
			}

			if (fieldCells[uiCell.FieldCordinates.x, uiCell.FieldCordinates.y].IsOccupied)
			{
				uiCell.SetElementColor(fieldCells[uiCell.FieldCordinates.x, uiCell.FieldCordinates.y].OccupiedElementType.ElementColor);
			}
		}
	}
	
	private void Awake()
	{
		PlaceFieldLeft();
		p_uiCells = SpawnCells();
	}

	void PlaceFieldLeft()
	{
		var screenH = p_playerCamera.orthographicSize * 2f;
		var screenW = screenH * p_playerCamera.aspect;

		var fieldWidth = screenW * 0.35f;
		var fieldHeight = screenH * 0.5f;

		gameObject.transform.localScale = new(fieldWidth, fieldHeight, 1);

		var xFieldPosition = p_playerCamera.transform.position.x - screenW / 2f;
		var fieldHalfWidth = fieldWidth / 2f;

		var margin = 0.01f * screenW;

		gameObject.transform.position = new Vector3(
			xFieldPosition + fieldHalfWidth + margin + fieldWidth / p_fieldModel.p_horizontalNumberOfCells,
			p_playerCamera.transform.position.y + 0.25f * screenH - margin,
			gameObject.transform.position.z
		);
	}

	void PlaceFieldRight()
	{
		var screenH = p_playerCamera.orthographicSize * 2f;
		var screenW = screenH * p_playerCamera.aspect;

		var fieldWidth = screenW * 0.35f;
		var fieldHeight = screenH * 0.5f;

		gameObject.transform.localScale = new(fieldWidth, fieldHeight, 1);

		var xFieldPosition = p_playerCamera.transform.position.x + screenW / 2f;
		var fieldHalfWidth = fieldWidth / 2f;

		var margin = 0.01f * screenW;

		gameObject.transform.position = new Vector3(
			xFieldPosition - fieldHalfWidth - margin - fieldWidth / p_fieldModel.p_horizontalNumberOfCells,
			p_playerCamera.transform.position.y + 0.25f * screenH - margin,
			gameObject.transform.position.z
		);
	}

	public List<GameFieldUiElement> SpawnCells()
	{
		var horizontalCellScale = 1f / p_fieldModel.p_horizontalNumberOfCells;
		var verticalCellScale = 1f / p_fieldModel.p_verticalNumberOfCells;
		var cellScale = new Vector3(horizontalCellScale, verticalCellScale, 1);
		var cellMargin = new Vector3(p_horizontalCellMargin * horizontalCellScale, p_verticalCellMargin * verticalCellScale);

		var leftTopLocal = new Vector3(
			-0.5f,
			 0.5f,
			 0f
		);

		var createdUiCells = new List<GameFieldUiElement>();

		for (var horizontal = 0; horizontal < p_fieldModel.p_horizontalNumberOfCells; horizontal++)
		{
			for (var vertival = 0; vertival < p_fieldModel.p_verticalNumberOfCells; vertival++)
			{
				Vector3 localPos = new Vector3(
					(horizontal + 0.5f) / p_fieldModel.p_horizontalNumberOfCells,
					-(vertival + 0.5f) / p_fieldModel.p_verticalNumberOfCells,
					0f
				);

				var spawnedCell = Instantiate(
					p_BoardCellPrefab,
					transform
				);

				spawnedCell.transform.localPosition = leftTopLocal + localPos;
				spawnedCell.transform.localScale = cellScale - cellMargin;

				if(spawnedCell.TryGetComponent<GameFieldUiElement>(out var uiCell))
				{
					uiCell.FieldCordinates = new(horizontal, vertival);
					createdUiCells.Add(uiCell);
				}
			}
		}

		return createdUiCells;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		PlaceFieldLeft();
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(gameObject.transform.position, gameObject.transform.localScale);
		Gizmos.DrawWireCube(
			new(
				gameObject.transform.position.x - gameObject.transform.localScale.x / 2f - gameObject.transform.localScale.x / p_fieldModel.p_horizontalNumberOfCells / 2f,
				gameObject.transform.position.y + gameObject.transform.localScale.y / 2f - gameObject.transform.localScale.y / p_fieldModel.p_verticalNumberOfCells / 2f,
				gameObject.transform.position.z
			),
			new(gameObject.transform.localScale.x / p_fieldModel.p_horizontalNumberOfCells, gameObject.transform.localScale.y / p_fieldModel.p_verticalNumberOfCells, 1)
		);
	}
#endif
}

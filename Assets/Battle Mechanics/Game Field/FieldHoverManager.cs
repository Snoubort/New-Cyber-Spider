using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class FieldHoverManager : MonoBehaviour
{
	public Camera cam;
	public LayerMask cellMask;

	GameFieldUiElement currentCell;

	private int CurrentRotation = 0;

	public BaseCard HoveredCard;

	[SerializeField]
	private FieldModel p_fieldModel;

	[SerializeField]
	private GameFieldView p_fieldView;

	private Controls p_playerInput;

	private void Awake()
	{
		p_playerInput = new();
	}

	void Update()
	{
		if(HoveredCard == null)
			return;

		Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

		if (Physics.Raycast(ray, out RaycastHit hit, 100f, cellMask))
		{
			p_playerInput.Enable();
			p_playerInput.CoreGameplay.RotateElement.performed += RotateElement;

			var cell = hit.collider.GetComponent<GameFieldUiElement>();

			if (cell != currentCell)
			{
				p_fieldModel.AddGhostCells(HoveredCard.CardType, CurrentRotation, cell.FieldCordinates);
				currentCell = cell;
				p_fieldView.DrawFieldFromModel(HoveredCard.CardType);
			}
		}
		else
		{
			ClearHover();

			p_playerInput.CoreGameplay.RotateElement.performed -= RotateElement;
			p_playerInput.Disable();
		}
	}

	public bool PlaceElement()
	{
		if (currentCell == null)
			return false;

		if (p_fieldModel.PlaceElement(HoveredCard.CardType, CurrentRotation, currentCell.FieldCordinates))
		{
			CurrentRotation = 0;
			p_fieldView.DrawFieldFromModel(HoveredCard.CardType);
			HoveredCard = null;
			return true;
		}

		p_fieldModel.GhostCells.Clear();
		HoveredCard = null;

		p_fieldView.DrawFieldFromModel();
		return false;
	}

	//private void OnEnable()
	//{
	//	p_playerInput.Enable();
	//	p_playerInput.CoreGameplay.RotateElement.performed += RotateElement;
	//}

	//private void OnDisable()
	//{
	//	p_playerInput.CoreGameplay.RotateElement.performed -= RotateElement;
	//	p_playerInput.Disable();
	//}

	private void RotateElement (InputAction.CallbackContext ctx)
	{
		CurrentRotation += 1;
		CurrentRotation = CurrentRotation % 4;
		p_fieldModel.GhostCells.Clear();
		p_fieldModel.AddGhostCells(HoveredCard.CardType, CurrentRotation, currentCell.FieldCordinates);
		p_fieldView.DrawFieldFromModel(HoveredCard.CardType);
	}

	void ClearHover()
	{
		if (currentCell != null)
		{
			currentCell = null;
			p_fieldModel.GhostCells.Clear();
			p_fieldView.DrawFieldFromModel();
			CurrentRotation = 0;
		}
	}
}

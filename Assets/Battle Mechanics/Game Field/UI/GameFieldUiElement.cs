using UnityEngine;

public class GameFieldUiElement : MonoBehaviour
{
	Renderer rend;
	Color baseColor;
	public Vector2Int FieldCordinates;

	void Awake()
	{
		rend = GetComponent<Renderer>();
		baseColor = rend.material.color;
	}

	public void SetElementColor(Color _elementColor)
	{
		rend.material.color = _elementColor;
	}

	public void SetDefaultColor()
	{
		rend.material.color = baseColor;
	}
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementTypeParams", menuName = "Scriptable Objects/ElementTypeParams")]
public class ElementTypeParams : ScriptableObject
{
    public Vector2Int CentralCell = new(0, 0);
    public List<Vector2Int> CellsOfElement = new();
    public Color ElementColor = Color.white;
}

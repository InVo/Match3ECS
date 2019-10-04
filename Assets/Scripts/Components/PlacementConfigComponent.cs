using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlacementConfigComponent : IComponentData
{
    public int RowsCount;
    public int ColumnsCount;
    public float Spacing;
}

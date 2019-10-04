using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


/*
 * Two approaches are used here - update using EntityQuery and update using Entities (which is EntityQueryBuilder instance)
 * Both of them use Allocator.TempJob. Entities uses it internally, EntityQuery must use it explicitly
 */
public class BallsPlacementSystem : ComponentSystem
{
    private EntityQuery _entityQuery;

    protected override void OnCreate()
    {
        _entityQuery = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<MatchableComponent>());
    }

    protected override void OnUpdate()
    {
        UpdateUsingEntitiesForEach();

        //UpdateUsingEntityQuery();
    }

    /// <summary>
    /// This looks like more suitable for use in non-job systems
    /// </summary>
    private void UpdateUsingEntitiesForEach()
    {
        PlacementConfigComponent placementConfig = GetSingleton<PlacementConfigComponent>();
        int index = 0;
        Entities.ForEach((ref Translation translation, ref MatchableComponent matchable) =>
        {
            int columnIndex = index % placementConfig.ColumnsCount;
            int rowIndex = index / placementConfig.ColumnsCount;
            float x = 0 + columnIndex * placementConfig.Spacing;
            float y = 0 + rowIndex * placementConfig.Spacing;
            translation.Value = new float3(x, y, 0f);
            ++index;
        });
    }

    /// <summary>
    /// Using entity query is more suitable for Jobs
    /// </summary>
    private void UpdateUsingEntityQuery()
    {
        var translations = _entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        var entities = _entityQuery.ToEntityArray(Allocator.TempJob);
        PlacementConfigComponent placementConfig = _entityQuery.GetSingleton<PlacementConfigComponent>();

        int length = translations.Length;
        for (int i = 0; i < length; ++i)
        {
            Translation translation = translations[i];
            int columnIndex = i & placementConfig.ColumnsCount;
            int rowIndex = i / placementConfig.ColumnsCount;
            float x = 0 + columnIndex * placementConfig.Spacing;
            float y = 0 + rowIndex * placementConfig.Spacing;
            translation.Value = new float3(x, y, 0f);
            EntityManager.SetComponentData(entities[i], translation);
        }

        translations.Dispose();
        entities.Dispose();
    }
}

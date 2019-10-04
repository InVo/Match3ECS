using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class ConfigsInitializer
{
    private EntityManager _entityManager;
    public ConfigsInitializer(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }
    public void Initialize()
    {
        CreateBallsPlacementConfigEntity();
    }

    private void CreateBallsPlacementConfigEntity()
    {
        EntityArchetype archetype = _entityManager.CreateArchetype(typeof(PlacementConfigComponent));
        Entity configEntity = _entityManager.CreateEntity(archetype);

        _entityManager.SetComponentData(configEntity, new PlacementConfigComponent
        {
            ColumnsCount = 5,
            RowsCount = 5,
            Spacing = 1.5f
        });
    }
}

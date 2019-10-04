using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Mesh _ballMesh;
    [SerializeField] private Material _blueBallMaterial;
    [SerializeField] private Material _greenBallMaterial;
    [SerializeField] private Material _redBallMaterial;
    [SerializeField] private Material _yellowBallMaterial;

    private EntityManager _entityManager;

    private EntityArchetype _ballArchetype;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Game.Awake is called");
        Initialize();
    }

    private void Initialize()
    {
        _entityManager = World.Active.EntityManager;
        ConfigsInitializer configsIntializer = new ConfigsInitializer(_entityManager);
        configsIntializer.Initialize();

        CreateBallsArchetypes();
        CreateBallsByColor(10, ColorType.ColorBlue);
        CreateBallsByColor(10, ColorType.ColorGreen);
        CreateBallsByColor(10, ColorType.ColorRed);
        CreateBallsByColor(10, ColorType.ColorYellow);
        //BallsPlacementSystem ballsPlacementSystem  = World.Active.CreateSystem<BallsPlacementSystem>();
        //ballsPlacementSystem.Update();
    }

    private void CreateBallsByColor(int count, ColorType colorType)
    {
        Material material = GetMaterialByColorType(colorType);
        NativeArray<Entity> blueBallEntities = new NativeArray<Entity>(count, Allocator.Temp, NativeArrayOptions.ClearMemory);
        _entityManager.CreateEntity(_ballArchetype, blueBallEntities);
        int index = 0;
        foreach (var entity in blueBallEntities)
        {
            _entityManager.SetComponentData(entity, new MatchableComponent
            {
                ColorType = colorType
            });
            _entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _ballMesh,
                material = material,
                castShadows = ShadowCastingMode.Off,
                receiveShadows = false
            });
            ++index;
        }

        blueBallEntities.Dispose();
    }

    private Material GetMaterialByColorType(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.ColorBlue:
                return _blueBallMaterial;
            case ColorType.ColorYellow:
                return _yellowBallMaterial;
            case ColorType.ColorGreen:
                return _greenBallMaterial;
            case ColorType.ColorRed:
                return _redBallMaterial;
            default:
                return null;
        }
    }

    private void CreateBallsArchetypes()
    {
        _ballArchetype = _entityManager.CreateArchetype(
            typeof(MatchableComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld)
        );
    }
}

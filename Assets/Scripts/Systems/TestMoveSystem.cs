using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class TestMoveJobSystem : JobComponentSystem
{

    [BurstCompile]
    struct MoveJob : IJobForEach<Translation>
    {
        private float _deltaTime;

        public MoveJob(float deltaTime)
        {
            _deltaTime = deltaTime;
        }

        public void Execute(ref Translation translation)
        {
            translation.Value.x += _deltaTime * 2f;
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MoveJob moveJob = new MoveJob(Time.deltaTime);
        return moveJob.Schedule(this, inputDeps);
    }
}

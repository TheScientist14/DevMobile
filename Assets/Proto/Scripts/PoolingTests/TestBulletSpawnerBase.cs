using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestBulletSpawnerBase : MonoBehaviour
{
    // With ten seconds of bullet life time, the approximate result of nb of created object is
    // 10 * _spawnPerSecond * _nbPerSpawn
    protected float _spawnPerSecond = 30.0f;

    protected int _nbPerSpawn = 10;

    private float _spawnCounter = 0.0f;

    protected void Update()
    {
        _spawnCounter += Time.deltaTime;
        if (_spawnCounter >= 1 / _spawnPerSecond )
        {
            for (int i = 0; i < _nbPerSpawn; i++)
            {
                OnSingleSpawnCalled();
            }
            _spawnCounter = 0.0f;
        }
    }

    protected abstract void OnSingleSpawnCalled();
}

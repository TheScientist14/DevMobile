using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Data/Asteroids")]
public class AsteroidInfo : ScriptableObject
{
    [SerializeField]
    private float _maxDistanceToSpaceShip = 160.0f;
    public float MaxDistanceToSpaceShip { get { return _maxDistanceToSpaceShip; } }

    [SerializeField]
    private float _delayToTeleport = 2.0f;
    public float DelayToTeleport { get { return _delayToTeleport; } }

    [SerializeField, Min(0.0f)]
    private float _teleportationOffset = 0.0f;
    public float TeleportationOffset { get { return _teleportationOffset; } }

    [SerializeField]
    private float _maxSpeed = 30.0f;
    public float MaxSpeed { get { return _maxSpeed; } }
    
    [SerializeField]
    private float _maxTorn = 37.0f;
    public float MaxTorn { get { return _maxTorn; } }

    [SerializeField, Range(0.0f, 1.0f)]
    private float _debrisScale = 0.5f;
    public float DebrisScale { get { return _debrisScale; } }

    [SerializeField, Range(2, 40)]
    private int _maxBigAsteroidNb;
    public int MaxBigAsteroidNb { get { return _maxBigAsteroidNb; } }

    [SerializeField, Range(2, 40)]
    private int _maxSmallAsteroidNb;
    public int MaxSmallAsteroidNb { get { return _maxSmallAsteroidNb; } }

    [ReadOnly]
    public int BigAsteroidCounter;
    [ReadOnly]
    public int SmallAsteroidCounter;

    [SerializeField]
    private int _minBigAsteroidCount = 8;
    public int MinBigAsteroidCount { get { return _minBigAsteroidCount; } }


    [SerializeField]
    private int[] _AsteroidScoreArray = { 20, 50, 100 };
    /// <summary>
    /// Returns the score the asteroid should give considering his index
    /// </summary>
    /// <param name="index">The number of divisions he had</param>
    /// <returns></returns>
    public int GetAsteroidScore(int index)
    {
        return _AsteroidScoreArray[Mathf.Clamp(index, 0, _AsteroidScoreArray.Length)];
    }

    [SerializeField]
    private int _bigAsteroidScore = 20;
    public int BigAsteroidScore { get { return _bigAsteroidScore; } }
    [SerializeField]
    private int _smallAsteroidScore = 100;
    public int SmallAsteroidScore { get { return _smallAsteroidScore; } }
    // 20 then 50 then 100
    public void ResetValues()
    {
        BigAsteroidCounter = 0;
        SmallAsteroidCounter = 0;
    }
}

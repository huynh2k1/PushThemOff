using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataContainer 
{
    public List<LevelData> levels = new List<LevelData>();
}

[Serializable]
public class LevelData
{
    public int OrderId = 0;
    public int MapId;
    public PlayerData PlayerData;
    public List<AreaData> listArea = new List<AreaData>();
}

[Serializable]
public class AreaData
{
    public int AreaId;
    public List<EnemyData> listEnemy = new List<EnemyData>();
}

[Serializable]
public class PlayerData
{
    public TransformObj Transform;
}

[Serializable]
public class EnemyData
{
    public TransformObj Transform;
    public EnemyType Type;
}

[Serializable]
public enum EnemyType
{
    E1 = 0,
    E2 = 1,
    E3 = 2,
    E4 = 3,
    E5 = 4,
}

[Serializable]
public struct TransformObj
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 LocalScale;

    public static TransformObj FromTransform(Transform t)
    {
        return new TransformObj
        {
            Position = t.position,
            Rotation = t.rotation,
            LocalScale = t.localScale
        };
    }

    public void ApplyTo(Transform t)
    {
        t.position = Position;
        t.rotation = Rotation;
        t.localScale = LocalScale;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool I;

    [System.Serializable]
    public class EffectPrefab
    {
        public EffectType id;
        public PooledEffect prefab;
        public int preloadCount;
    }

    [SerializeField] List<EffectPrefab> effects;

    Dictionary<EffectType, Queue<PooledEffect>> poolDict = new();
    Dictionary<EffectType, PooledEffect> prefabDict = new();

    void Awake()
    {
        I = this;

        foreach (var e in effects)
        {
            Queue<PooledEffect> queue = new();
            prefabDict.Add(e.id, e.prefab);

            for (int i = 0; i < e.preloadCount; i++)
            {
                var obj = Instantiate(e.prefab, transform);
                obj.Init(e.id);
                obj.gameObject.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDict.Add(e.id, queue);
        }
    }

    void OnEnable()
    {
        PooledEffect.OnEffectCompleteAction += Release;
    }

    void OnDisable()
    {
        PooledEffect.OnEffectCompleteAction -= Release;
    }

    public PooledEffect Spawn(EffectType id, Vector3 pos, Quaternion? rot = null)
    {
        Queue<PooledEffect> queue = poolDict[id];

        PooledEffect effect = queue.Count > 0
            ? queue.Dequeue()
            : CreateNew(id);

        effect.transform.position = pos;

        if (rot.HasValue)
            effect.transform.rotation = rot.Value; // chỉ override khi cần

        effect.gameObject.SetActive(true);
        effect.Play();

        return effect;
    }

    PooledEffect CreateNew(EffectType id)
    {
        var obj = Instantiate(prefabDict[id], transform);
        obj.Init(id);
        return obj;
    }

    public void Release(PooledEffect effect)
    {
        effect.gameObject.SetActive(false);
        poolDict[effect.Type].Enqueue(effect);
    }
}
public enum EffectType
{
    KNIFEHIT,
    BOOMERANGHIT,
    HAMMERHIT,
    HAMMERLIGHTNING,
    SWORDHIT,
    EXPLOSION,
    COINFLYING,
    HITGROUND,
    SMOKEPUFF,
}

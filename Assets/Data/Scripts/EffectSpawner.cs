using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public PrefabPool Pool;
    public string PoolName;
    public bool UsePosition;
    public bool UseRotation;
    public bool Parent;

    public void Spawn(Transform origin)
    {
        var pool = Pool ?? PrefabPool.GetPool(PoolName);

        if (!pool)
        {
            Debug.LogError($"Unable to find prefab pool {PoolName}", this);
            return;
        }
        
        var go = pool.GetObject();

        Setup(go, origin);
        
        go.SetActive(true);
        
        Setup(go, origin);
    }

    private void Setup(GameObject go, Transform origin)
    {
        if (UsePosition) go.transform.position = origin.position;
        if (UseRotation) go.transform.rotation = origin.rotation;
        if (Parent) go.transform.SetParent(origin, true);
    }
}

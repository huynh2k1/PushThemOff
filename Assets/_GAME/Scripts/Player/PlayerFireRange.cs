using System.Collections.Generic;
using UnityEngine;

public class PlayerFireRange : MonoBehaviour
{
    private readonly List<Transform> enemiesInRange = new();
    public Transform CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (!enemiesInRange.Contains(other.transform))
        {
            enemiesInRange.Add(other.transform);

            // lock enemy đầu tiên
            if (CurrentTarget == null)
                CurrentTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (enemiesInRange.Contains(other.transform))
        {
            RemoveEnemy(other.transform);
        }
    }

    public void RemoveEnemy(Transform enemy)
    {
        if (enemiesInRange.Remove(enemy))
        {
            if (CurrentTarget == enemy)
                CurrentTarget = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
        }
    }
}

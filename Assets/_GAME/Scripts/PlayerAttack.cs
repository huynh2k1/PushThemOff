using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] float rangeFire = 5f;
    [SerializeField] GameObject rangeGraphic;

    private void Awake()
    {
        InitRangeGraphic();
    }

    private void OnEnable()
    {
        CharacterCtrl.OnPlayerAttackAction += ThrowBoomerang;
    }

    private void OnDestroy()
    {
        CharacterCtrl.OnPlayerAttackAction -= ThrowBoomerang;
    }

    void ThrowBoomerang()
    {
        GameObject boom = Instantiate(boomerangPrefab, firePoint.position, Quaternion.identity);

        Vector3 dir = firePoint.forward; // hoặc transform.forward nếu 2D thì dùng transform.right

        Boomerang boomerang = boom.GetComponent<Boomerang>();
        boomerang.Init(firePoint, dir, rangeFire);
    }

    void InitRangeGraphic()
    {
        rangeGraphic.transform.localScale = Vector3.one * rangeFire * 2f;
    }
}

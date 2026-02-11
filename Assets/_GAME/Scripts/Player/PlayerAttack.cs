using H_Utils;
using NaughtyAttributes;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] BaseWeapon boomerangPrefab;
    [SerializeField] WeaponData boomerangData;

    [SerializeField] BaseWeapon knifePrefab;
    [SerializeField] WeaponData knifeData;

    [SerializeField] BaseWeapon hammerPrefab;
    [SerializeField] WeaponData hammerData;

    BaseWeapon currentWeaponPrefab;
    WeaponData _curWeaponData;

    [Header("Fire")]
    [SerializeField] private Transform firePoint;

    [Header("Range UI")]
    [SerializeField] private GameObject rangeGraphic;

    private void OnEnable()
    {
        PlayerCtrl.OnPlayerAttackAction += Attack;
        ButtonWeapon.OnClickThisAction += SwapWeapon;
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerAttackAction -= Attack;
        ButtonWeapon.OnClickThisAction -= SwapWeapon;
    }

    public void OnInit()
    {
        SwapWeapon(GameDatas.CurWeapon);
    }

    public void SwapWeapon(int id)
    {
        switch (id)
        {
            case 0:
                ChangeKnife();
                break;
            case 1:
                ChangeBoomerang();
                break;
            case 2:
                ChangeHammer();
                break;
        }
    }

    public void ChangeBoomerang()
    {
        ChangeWeapon(boomerangPrefab, boomerangData);
    }

    public void ChangeKnife()
    {
        ChangeWeapon(knifePrefab, knifeData);
    }

    public void ChangeHammer()
    {
        ChangeWeapon(hammerPrefab, hammerData);
    }

    void Attack()
    {
        var weaponGO = PoolManager.I.Spawn(
            currentWeaponPrefab,
            firePoint.position,
            firePoint.rotation
        );

        BaseWeapon weapon = weaponGO.GetComponent<BaseWeapon>();
        if (weapon == null)
        {
            Debug.LogError("Weapon prefab thiếu BaseWeapon!");
            return;
        }

        weapon.Init(_curWeaponData, firePoint, firePoint.forward);
    }

    public void ChangeWeapon(BaseWeapon newPrefab, WeaponData data)
    {
        currentWeaponPrefab = newPrefab;
        _curWeaponData = data;
        UpdateRangeGraphic();
    }

    void UpdateRangeGraphic()
    {
        if (rangeGraphic == null)
            return;

        rangeGraphic.transform.localScale =
            Vector3.one * _curWeaponData.MaxDistance * 2f;
    }
}

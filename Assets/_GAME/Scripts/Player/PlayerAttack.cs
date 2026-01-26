using NaughtyAttributes;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] GameObject boomerangPrefab;
    [SerializeField] WeaponData boomerangData;

    [SerializeField] GameObject knifePrefab;
    [SerializeField] WeaponData knifeData;

    [SerializeField] GameObject hammerPrefab;
    [SerializeField] WeaponData hammerData;

    GameObject currentWeaponPrefab;
    WeaponData _curWeaponData;

    [Header("Fire")]
    [SerializeField] private Transform firePoint;

    [Header("Range UI")]
    [SerializeField] private GameObject rangeGraphic;

    [Button("Change Boomerang")]
    public void ChangeBoomerang()
    {
        ChangeWeapon(boomerangPrefab, boomerangData);
    }

    [Button("Change Knife")]
    public void ChangeKnife()
    {
        ChangeWeapon(knifePrefab, knifeData);
    }

    [Button("Change Hammer")]
    public void ChangeHammer()
    {
        ChangeWeapon(hammerPrefab, hammerData);
    }
    private void Awake()
    {
        ChangeKnife();
    }

    private void OnEnable()
    {
        PlayerCtrl.OnPlayerAttackAction += Attack;
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerAttackAction -= Attack;
    }

    void Attack()
    {
        GameObject weaponGO = Instantiate(
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

    public void ChangeWeapon(GameObject newPrefab, WeaponData data)
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

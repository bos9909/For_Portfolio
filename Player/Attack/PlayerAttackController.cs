using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public List<WeaponBase> Weapons = new List<WeaponBase>();
    private int currentWeaponIndex = 0;

    // void Update()
    // {
    //     Fire();
    // }

    // void Fire()
    // {
    //     //키 검사하고 준비되면 발사 실행
    //     if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButton("Fire1"))
    //     {
    //         var weapon = Weapons[currentWeaponIndex];
    //         if (weapon.IsReady())
    //         {
    //             weapon.Attack();
    //         }
    //     }
    //
    //     if (Input.GetButton("Fire2"))
    //     {
    //         currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
    //     }
    // }
    
    /// PlayerBase가 호출할 주 공격 함수
    public void FirePrimary()
    {
        //무기 장착 확인
        if (Weapons.Count == 0) return;

        var weapon = Weapons[currentWeaponIndex];
        if (weapon.IsReady())
        {
            weapon.Attack();
        }
    }
    
    /// <summary>
    /// PlayerBase가 호출할 무기 교체 함수.
    /// </summary>
    public void SwitchWeapon()
    {
        if (Weapons.Count <= 1) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
        Debug.Log("Weapon switched to: " + Weapons[currentWeaponIndex].name);
    }
    
}

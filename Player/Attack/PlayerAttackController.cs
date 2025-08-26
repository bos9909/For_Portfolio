using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public List<WeaponBase> Weapons = new List<WeaponBase>();
    private int currentWeaponIndex = 0;

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        //키 검사하고 준비되면 발사 실행
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButton("Fire1"))
        {
            var weapon = Weapons[currentWeaponIndex];
            if (weapon.IsReady())
            {
                weapon.Attack();
            }
        }

        if (Input.GetButton("Fire2"))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;


    private void Start()
    {
        weaponCollider.enabled = false;
    }

    public void AttackPressed()
    {
        weaponCollider.enabled = true;

        Invoke("DiableWeaponCollider", 1.45f); //keep time as length of attack anim

    }

    private void DiableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}

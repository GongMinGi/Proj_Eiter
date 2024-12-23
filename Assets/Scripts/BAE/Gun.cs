/*using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject chargedProjectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeTime;
    private bool isCharging;

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isCharging = true;
            chargeTime += Time.deltaTime * chargeSpeed;

            if (chargeTime >= 2)
            {
                chargeTime = 2; // ���� �ִ밪 ����
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (chargeTime >= 2)
            {
                FireChargedProjectile();
            }
            else
            {
                FireRegularProjectile();
            }

            chargeTime = 0;
            isCharging = false;
        }
    }

    void FireRegularProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile, firePoint.position, firePoint.rotation);
        ProjectileBAE projectileScript = projectileInstance.GetComponent<ProjectileBAE>();
        if (projectileScript != null)
        {
            projectileScript.isCharged = false; // �Ϲ� �Ѿ�
        }
    }

    void FireChargedProjectile()
    {
        GameObject chargedInstance = Instantiate(chargedProjectile, firePoint.position, firePoint.rotation);
        ProjectileBAE chargedScript = chargedInstance.GetComponent<ProjectileBAE>();
        if (chargedScript != null)
        {
            chargedScript.isCharged = true; // ���� ����
        }
    }
}
*/




using System.Collections; //https://www.youtube.com/watch?v=ZDkMhiNQOwo&t=5s����
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject chargedProjectile;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeTime;
    private bool isCharging;

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && chargeTime < 2)
        {
            isCharging = true;
            if (isCharging == true)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            chargeTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.F) && chargeTime >= 2)
        {
            ReleaseCharge();
        }
    }

    void ReleaseCharge()
    {
        Instantiate(chargedProjectile, firePoint.position, firePoint.rotation);
        isCharging = false;
        chargeTime = 0;
    }
}

using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth instance;

    public int maxHealth;
    private int health;

    public event Action DamageTaken;
    public event Action HealthUpgraded;

    public int Health
    {

        get 
        { 

            return health; 

        }

    }

    void Awake()
    {

        if (instance == null)
        {

            instance = this;

            DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체 유지

        }
        else
        {

            Destroy(gameObject);    // 중복 방지

        }

    }

    private void Start()
    {

        LoadHealth();   // 저장된 체력 데이터 복원

        if (health == 0)    // 데이터가 없으면 초기화
        {

            health = maxHealth;

        }

    }

    public void TakeDamage()
    {

        if (health <= 0)
            return;

        health -= 1;

        DamageTaken?.Invoke(); // DamageTaken 이벤트 호출

    }

    public void Heal()
    {

        if (health >= maxHealth)
            return;

        health += 1;

        DamageTaken?.Invoke(); // DamageTaken 이벤트 호출

    }

    public void UpgradeHealth()
    {

        maxHealth++;
        health = maxHealth;

        HealthUpgraded?.Invoke(); // HealthUpgraded 이벤트 호출

    }

    public void SaveHealth()
    {

        PlayerPrefs.SetInt("CurrentHealth", health);
        PlayerPrefs.SetInt("MaxHealth", maxHealth);
        PlayerPrefs.Save();

    }

    public void LoadHealth()
    {

        if (PlayerPrefs.HasKey("CurrentHealth"))
        {

            health = PlayerPrefs.GetInt("CurrentHealth");
            maxHealth = PlayerPrefs.GetInt("MaxHealth");

        }

    }

}
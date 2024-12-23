using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int maxHealth;
    private int health;

    public event Action DamageTaken;
    public event Action HealthUpgraded;
    public event Action PlayerDied; // ��� �̺�Ʈ �߰�

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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ��ü ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }
    }

    private void Start()
    {
        LoadHealth(); // ����� ü�� ������ ����

        if (health == 0) // �����Ͱ� ������ �ʱ�ȭ
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;

        DamageTaken?.Invoke(); // DamageTaken �̺�Ʈ ȣ��

        if (health <= 0)
        {
            Debug.Log("Die ȣ��");
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        if (health >= maxHealth)
            return;

        health += healAmount;

        if (health > maxHealth)
        {
            health = maxHealth; // �ִ� ü���� �ʰ����� �ʵ���
        }

        DamageTaken?.Invoke(); // DamageTaken �̺�Ʈ ȣ��
    }

    public void UpgradeHealth()
    {
        maxHealth++;
        health = maxHealth;

        HealthUpgraded?.Invoke(); // HealthUpgraded �̺�Ʈ ȣ��
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

    private void Die()
    {
        Debug.Log("Player is dead");
        PlayerDied?.Invoke(); // ��� �̺�Ʈ ȣ��
        // ���� �߰� ��� ó�� ������ ���⼭ ����
    }
}

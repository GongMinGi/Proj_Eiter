using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // �÷��̾� ���� ������
    public int maxHealth;
    public int health;

    void Awake()
    {
        // �̱��� ���� ����
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ����

        }
        else
        {

            Destroy(gameObject); // �ߺ��� GameManager ����

        }

    }

    // �÷��̾� ������ ����
    public void SavePlayerData(int currentHealth, int maxHealth)
    {

        this.health = currentHealth;
        this.maxHealth = maxHealth;

    }

    // �÷��̾� ������ �ε�
    public void LoadPlayerData(out int currentHealth, out int maxHealth)
    {

        currentHealth = this.health;
        maxHealth = this.maxHealth;

    }

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public GameObject heart;
    public List<Image> hearts;

    private PlayerHealth playerHealth;

    void Start()
    {

        playerHealth = PlayerHealth.instance;
        playerHealth.DamageTaken += UpdateHearts;
        playerHealth.HealthUpgraded += AddHearts;

        InitializeHearts();     // �ʱ� ��Ʈ ���� �� UI �ݿ�

    }

    void InitializeHearts()
    {

        foreach (Image i in hearts)
        {

            Destroy(i.gameObject);

        }

        hearts.Clear();

        for (int i = 0; i < playerHealth.maxHealth; i++)
        {

            GameObject h = Instantiate(heart, this.transform);

            hearts.Add(h.GetComponent<Image>());

        }

        UpdateHearts();     // ���� ü�� �ݿ�

    }

    void UpdateHearts()
    {

        int heartFill = playerHealth.Health;

        foreach (Image i in hearts)
        {

            i.fillAmount = Mathf.Clamp01((float)heartFill);     // 0�� 1 ���̷� ����

            heartFill -= 1;

        }

    }

    void AddHearts()
    {

        InitializeHearts(); // �ִ� ü�� ���� �� ��Ʈ ����

    }

    void OnDestroy()
    {

        if (playerHealth != null)
        {

            playerHealth.DamageTaken -= UpdateHearts;
            playerHealth.HealthUpgraded -= AddHearts;

        }

    }

}
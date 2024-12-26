using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{

    public GameObject heart;
    public List<Image> hearts;

    private PlayerHealth playerHealth;

    public GameObject gameOverPanel;    // ���� ���� ȭ��

    void Start()
    {

        gameOverPanel.SetActive(false);

        playerHealth = PlayerHealth.instance;

        playerHealth.DamageTaken += UpdateHearts;
        playerHealth.HealthUpgraded += AddHearts;
//        playerHealth.PlayerDied += ShowGameOverScreen;  // ��� �̺�Ʈ ����

        StartCoroutine(DelayedInitialize());    // �ʱ� ��Ʈ ���� �� UI �ݿ�
    }

    IEnumerator DelayedInitialize()
    {

        yield return null;  // �� ������ ����Ͽ� PlayerHealth �ʱ�ȭ ����

        InitializeHearts();

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

            GameObject h = Instantiate(heart, transform);

            hearts.Add(h.GetComponent<Image>());

        }

        UpdateHearts(); // ���� ü�� �ݿ�

    }

    void UpdateHearts()
    {

        int heartFill = playerHealth.Health;

        foreach (Image i in hearts)
        {

            i.fillAmount = Mathf.Clamp01((float)heartFill); // 0�� 1 ���̷� ����
            heartFill -= 1;

        }

    }

    void AddHearts()
    {

        InitializeHearts(); // �ִ� ü�� ���� �� ��Ʈ ����

    }

    //void ShowGameOverScreen()
    //{

    //    Debug.Log("Game Over!");

    //    if (gameOverPanel != null)
    //    {

    //        gameOverPanel.SetActive(true); // ���� ���� �г� Ȱ��ȭ
    //    }

    //    Time.timeScale = 0f;

    //}

    void OnDestroy()
    {

        if (playerHealth != null)
        {

            playerHealth.DamageTaken -= UpdateHearts;
            playerHealth.HealthUpgraded -= AddHearts;
 //           playerHealth.PlayerDied -= ShowGameOverScreen; // �̺�Ʈ ����

        }

    }

}
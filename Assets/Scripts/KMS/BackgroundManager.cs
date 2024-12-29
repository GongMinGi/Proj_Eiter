using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{
    [Header("��� Ÿ��")]
    public GameObject backgroundCity; // ��� �̹��� Ÿ�� A
    public GameObject backgroundLab; // ��� �̹��� Ÿ�� B

    [Header("Scene to Background Mapping")]
    public Dictionary<string, string> sceneBackgroundMap = new Dictionary<string, string>
    {
        { "LabABasementScene", "Lab" },
        { "CharacterAndTile", "City" },
        { "LabARooftopScene", "City" },
    };

    private void Start()
    {
        SetBackgroundForCurrentScene();
    }

    private void SetBackgroundForCurrentScene()
    {
        // ���� �� �̸� ��������
        string currentScene = SceneManager.GetActiveScene().name;

        // ���� ���� ��� �̹��� Ÿ�� Ȯ��
        if (sceneBackgroundMap.TryGetValue(currentScene, out string backgroundType))
        {
            // ��� Ÿ�Կ� ���� Ȱ��ȭ/��Ȱ��ȭ ����
            if (backgroundType == "TypeA")
            {
                backgroundCity.SetActive(true);
                backgroundLab.SetActive(false);
            }
            else if (backgroundType == "TypeB")
            {
                backgroundCity.SetActive(false);
                backgroundLab.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning($"No background mapping found for scene: {currentScene}");
            // �⺻������ ��� ��� ��Ȱ��ȭ
            backgroundCity.SetActive(false);
            backgroundLab.SetActive(false);
        }
    }
}

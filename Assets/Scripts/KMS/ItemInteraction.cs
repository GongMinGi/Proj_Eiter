using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] private PopupController popupController; // �˾� UI ����
    [SerializeField] private Tuto_BlinkUI[] blinkUIs; // ���� ���� <PressEforItem> ����
    [SerializeField] private GameObject[] pressEImages; // ���� ���� <PressEforItem> ������Ʈ
    [SerializeField] private bool[] initialActiveStates; // �ʱ� Ȱ��ȭ ���� ����
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // ����� ���� ���� Ű

    private bool isPlayerNearby = false; // �÷��̾ ��ó�� �ִ��� Ȯ��

    void Start()
    {
        // �� Image�� �ʱ� ���¸� ����
        for (int i = 0; i < pressEImages.Length; i++)
        {
            if (pressEImages[i] != null)
            {
                // initialActiveStates�� ���� ���� Ȱ��ȭ/��Ȱ��ȭ ����
                bool shouldActivate = i < initialActiveStates.Length && initialActiveStates[i];
                pressEImages[i].SetActive(shouldActivate);

                // Ȱ��ȭ�� ��� ��¦�� ȿ���� ����
                if (shouldActivate && i < blinkUIs.Length && blinkUIs[i] != null)
                {
                    blinkUIs[i].enabled = true;
                }
                else if (i < blinkUIs.Length && blinkUIs[i] != null)
                {
                    blinkUIs[i].enabled = false;
                }
            }
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactionKey))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (blinkUIs.Length > 0)
        {
            // ��� Press E Image�� ���̵�ƿ� ����
            StartCoroutine(HandleFadeAndDisable());
        }
        else
        {
            // blinkUI�� ������ �ٷ� ��Ȱ��ȭ ó��
            DisableObject();
        }
    }

    private IEnumerator HandleFadeAndDisable()
    {
        // ��� �̹����� ���̵�ƿ� ����
        foreach (var blinkUI in blinkUIs)
        {
            if (blinkUI != null)
            {
                blinkUI.StartFadeOut();
            }
        }

        // ��� �̹����� ���̵�ƿ��� ���� ������ ���
        bool allFaded = false;
        while (!allFaded)
        {
            allFaded = true;
            foreach (var blinkUI in blinkUIs)
            {
                if (blinkUI != null && blinkUI.tutorialImage.color.a > 0f)
                {
                    allFaded = false;
                    break;
                }
            }
            yield return null;
        }

        // ���̵�ƿ� �Ϸ� �� <ParticleForUmbrella> ��Ȱ��ȭ
        DisableObject();
    }

    private void DisableObject()
    {
        if (popupController != null)
        {
            popupController.ShowPopup(); // �˾� UI ǥ��
        }

        gameObject.SetActive(false); // <ParticleForUmbrella> ��Ȱ��ȭ
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // ��� Press E �̹����� Ȱ��ȭ�ϰ� ��¦�� ����
            for (int i = 0; i < pressEImages.Length; i++)
            {
                if (pressEImages[i] != null)
                {
                    pressEImages[i].SetActive(true);
                }

                if (i < blinkUIs.Length && blinkUIs[i] != null)
                {
                    blinkUIs[i].enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // ��� Press E �̹����� ��Ȱ��ȭ�ϰ� ��¦�� �ߴ�
            for (int i = 0; i < pressEImages.Length; i++)
            {
                if (pressEImages[i] != null)
                {
                    pressEImages[i].SetActive(false);
                }

                if (i < blinkUIs.Length && blinkUIs[i] != null)
                {
                    blinkUIs[i].enabled = false;
                }
            }
        }
    }
}

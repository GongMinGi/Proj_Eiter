using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] 
    private PopupController popupController;     // �˾� ���� ��ũ��Ʈ

    [SerializeField] 
    private Tuto_BlinkUI[] blinkUIs;    // Ʃ�丮�� UI ��ũ��Ʈ �迭

    [SerializeField] 
    private GameObject[] UIImages;   // UI �̹��� �迭

    [SerializeField] 
    private bool[] initialActiveStates;  // �ʱ� Ȱ��ȭ ���� ����

    private bool isPlayerNearby = false;

    void Start()
    {
        
        for (int i = 0; i < UIImages.Length; i++)       // UIImages �ʱ� ���� ����
        {

            if (UIImages[i] != null)
            {

                bool shouldActivate = i < initialActiveStates.Length && initialActiveStates[i];

                UIImages[i].SetActive(shouldActivate);

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
        
        if (isPlayerNearby && IsInteractionKeyPressed())        // �÷��̾ ��ó�� �ְ�, blinkUIs�� fadeOutKey �� �ϳ��� ������ ��ȣ�ۿ� ����
        {

            HandleInteraction();

        }

    }

    private bool IsInteractionKeyPressed()
    {
        
        foreach (var blinkUI in blinkUIs)       // ��� blinkUIs�� fadeOutKey�� Ȯ��
        {

            if (blinkUI != null && Input.GetKeyDown(blinkUI.fadeOutKey))
            {

                return true;

            }

        }

        return false;

    }

    private void HandleInteraction()
    {

        if (blinkUIs.Length > 0)
        {

            StartCoroutine(HandleFadeAndDisable());

        }
        else
        {

            DisableObject();

        }

    }

    private IEnumerator HandleFadeAndDisable()
    {
        
        foreach (var blinkUI in blinkUIs)       // ��� Ʃ�丮�� �̹����� ���̵�ƿ��� ����
        {

            if (blinkUI != null)
            {

                blinkUI.StartFadeOut();

            }

        }
        
        bool allFaded = false;      

        while (!allFaded)       // ��� Ʃ�丮�� �̹����� ���̵�ƿ��� �Ϸ�� ������ ���
        {

            allFaded = true;

            foreach (var blinkUI in blinkUIs)
            {

                if (blinkUI != null && !blinkUI.isFadedOut)
                {

                    allFaded = false;

                    break;

                }

            }

            yield return null;  // ���� �����ӱ��� ���

        }

        // ���̵�ƿ� �Ϸ� �� �˾� ǥ��
        if (popupController != null)
        {
            popupController.ShowPopup();
        }

        // ������ ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    private void DisableObject()
    {
        
        if (popupController != null)
        {

            popupController.ShowPopup();        // �˾� ǥ��

        }
        
        gameObject.SetActive(false);        // ������ ��Ȱ��ȭ

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))     // �÷��̾ ������ ������ UI Ȱ��ȭ
        {

            isPlayerNearby = true;

            for (int i = 0; i < UIImages.Length; i++)
            {

                if (UIImages[i] != null)
                {

                    UIImages[i].SetActive(true);

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
        
        if (other.CompareTag("Player"))     // �÷��̾ ������ ����� UI ��Ȱ��ȭ
        {

            isPlayerNearby = false;

            for (int i = 0; i < UIImages.Length; i++)
            {

                if (UIImages[i] != null)
                {

                    UIImages[i].SetActive(false);

                }

                if (i < blinkUIs.Length && blinkUIs[i] != null)
                {

                    blinkUIs[i].enabled = false;

                }

            }

        }

    }

}

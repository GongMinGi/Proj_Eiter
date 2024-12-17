using UnityEngine;
using UnityEngine.UI; // UI ���� Ŭ���� ����

public class ItemInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject itemUIPopup; // UI �˾� �г�
    private bool isPlayerNearby = false; // �÷��̾ ��ó�� �ִ��� üũ
    private bool isUIPopupActive = false; // UI �˾� ���¸� üũ

    void Start()
    {
        // UI �г��� ��Ȱ��ȭ ���·� ����
        if (itemUIPopup != null)
            itemUIPopup.SetActive(false);
    }

    void Update()
    {
        // UI �˾��� Ȱ��ȭ�� ���¿��� �ƹ� Ű�� ������ UI �ݱ�
        if (isUIPopupActive)
        {

            if (Input.anyKeyDown)
            {

                CloseUIPopup();

            }


        }

        // �÷��̾ ������ ��ó�� ���� �� E Ű�� ������ ����
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // �������� ����� UI �˾� ǥ��
            HideItem();
        }

    }

    private void HideItem()
    {
        // �������� ��Ȱ��ȭ (SetActive(false))
        gameObject.SetActive(false);

        // UI �˾� Ȱ��ȭ
        if (itemUIPopup != null)
        {
            itemUIPopup.SetActive(true);
            isUIPopupActive = true; // UI �˾��� Ȱ��ȭ��
        }

        Debug.Log("Item picked up! UI Popup displayed.");
    }

    private void CloseUIPopup()
    {
        // UI �˾� ��Ȱ��ȭ
        if (itemUIPopup != null)
        {
            itemUIPopup.SetActive(false);
            isUIPopupActive = false; // UI �˾� ���� ��Ȱ��ȭ
        }

        Debug.Log("UI Popup closed.");
    }

    // �÷��̾ Trigger Collider�� ������ ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // �÷��̾ ��ó�� ����
            Debug.Log("Press 'E' to pick up the item!");
        }
    }

    // �÷��̾ Trigger Collider���� ������ ��
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false; // �÷��̾ ������ ���
        }
    }
}

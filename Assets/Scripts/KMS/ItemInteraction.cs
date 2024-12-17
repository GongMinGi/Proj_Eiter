using UnityEngine;

public class ItemInteraction : MonoBehaviour
{

    [SerializeField]
    private PopupController popupController;
    private bool isPlayerNearby = false;        // �÷��̾ ��ó�� �ִ��� üũ

    void Update()
    {
        
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))      // �÷��̾ ������ ��ó�� ���� �� E Ű�� ������ ����
        {

            HideItem();

        }

    }

    private void HideItem()
    {
        
        gameObject.SetActive(false);        // �������� ��Ȱ��ȭ

        if (popupController != null)
        {

            popupController.ShowPopup();        // UI �˾� Ȱ��ȭ

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            isPlayerNearby = true;

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            isPlayerNearby = false;

        }

    }

}

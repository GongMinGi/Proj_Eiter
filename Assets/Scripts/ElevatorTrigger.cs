using UnityEngine;
using UnityEngine.UI;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject popupImage; // Inspector���� UI �˾� �̹��� ����
    private bool isPlayerOnPlatform = false;

    private void Start()
    {
        // ���� �� �˾� �̹����� ��Ȱ��ȭ
        if (popupImage != null)
            popupImage.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾ �÷����� �ö�Դ��� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            ShowPopup();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾ �÷������� ���������� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            HidePopup();
        }
    }

    private void ShowPopup()
    {
        if (popupImage != null)
        {
            popupImage.SetActive(true); // UI �˾� Ȱ��ȭ
            Debug.Log("Popup Image Displayed!");
        }
    }

    private void HidePopup()
    {
        if (popupImage != null)
        {
            popupImage.SetActive(false); // UI �˾� ��Ȱ��ȭ
            Debug.Log("Popup Image Hidden!");
        }
    }
}

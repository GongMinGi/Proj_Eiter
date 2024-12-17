using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{

    [SerializeField] 
    private GameObject popupImage;  // Inspector���� UI �˾� �̹��� ����

    private void Start()
    {

        if (popupImage != null)     // ���� �� �˾� �̹����� ��Ȱ��ȭ
        {

            popupImage.SetActive(false);

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)      // �÷��̾ �÷����� �ö�Դ��� Ȯ��
    {
        
        if (collision.gameObject.CompareTag("Player"))      
        {

            ShowPopup();

        }

    }

    private void OnCollisionExit2D(Collision2D collision)       // �÷��̾ �÷������� ���������� Ȯ��
    {
        
        if (collision.gameObject.CompareTag("Player"))      
        {

            HidePopup();

        }

    }

    private void ShowPopup()
    {

        if (popupImage != null)
        {

            popupImage.SetActive(true);     // UI �˾� Ȱ��ȭ

        }

    }

    private void HidePopup()
    {

        if (popupImage != null)
        {

            popupImage.SetActive(false);    // UI �˾� ��Ȱ��ȭ

        }

    }

}

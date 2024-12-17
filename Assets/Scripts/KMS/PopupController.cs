using UnityEngine;

public class PopupController : MonoBehaviour
{

    private bool isPopupActive = false;

    void Start()
    {

        gameObject.SetActive(false);

    }

    void Update()
    {
        
        if (isPopupActive && Input.anyKeyDown)      // �˾��� Ȱ��ȭ�� ���¿��� �ƹ� Ű�� ������ �˾� ��Ȱ��ȭ
        {

            ClosePopup();

        }

    }

    public void ShowPopup()
    {

        gameObject.SetActive(true);

        isPopupActive = true;

    }

    public void ClosePopup()
    {

        gameObject.SetActive(false);

        isPopupActive = false;

    }

}

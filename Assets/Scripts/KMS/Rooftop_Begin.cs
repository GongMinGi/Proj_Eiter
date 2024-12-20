using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rooftop_Begin : MonoBehaviour
{

    public Image blackoutImage;          // ������ �̹��� (UI)
    public float fadeSpeed = 2f;         // �̹����� �������� �ӵ�

    void Start()
    {
        
        if (blackoutImage != null)      // ���� ���۵Ǹ� �ڷ�ƾ ����
        {

            StartCoroutine(FadeOutEffect());

        }
        else
        {

            Debug.LogError("Blackout Image is not assigned!");      // ���ƿ� �̹��� ������ �̷��� ��

        }

    }

    private IEnumerator FadeOutEffect()
    {

        RectTransform blackoutRect = blackoutImage.rectTransform;

        Vector3 startPosition = Vector3.zero;       // �ʱ� ��ġ�� ȭ�� �߾����� ����
        Vector3 endPosition = new Vector3(0, -Screen.height, 0);        // ȭ�� �Ʒ��� �̵�

        float elapsedTime = 0f;
        
        while (elapsedTime < 1f / fadeSpeed)        // �̹����� ������ �Ʒ��� �������� ȿ��
        {

            float t = elapsedTime / (1f / fadeSpeed);

            blackoutRect.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;

            yield return null;

        }
        
        blackoutRect.localPosition = endPosition;       // ��ġ�� ���� ������ ���� 

        blackoutImage.gameObject.SetActive(false);      // �̹��� ��Ȱ��ȭ

    }

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rooftop_Begin : MonoBehaviour
{
    public Image blackoutImage;          // ������ �̹��� (UI)
    public float fadeSpeed = 2f;         // �̹����� �������� �ӵ�

    void Start()
    {
        // ���� ���۵Ǹ� �ڷ�ƾ ����
        if (blackoutImage != null)
        {
            StartCoroutine(FadeOutEffect());
        }
        else
        {
            Debug.LogError("Blackout Image is not assigned!");
        }
    }

    private IEnumerator FadeOutEffect()
    {
        RectTransform blackoutRect = blackoutImage.rectTransform;

        // �ʱ� ��ġ�� ȭ�� �߾����� ����
        Vector3 startPosition = Vector3.zero; // ȭ�� �߾�
        Vector3 endPosition = new Vector3(0, -Screen.height, 0); // ȭ�� �Ʒ��� �̵�

        float elapsedTime = 0f;

        // �̹����� ������ �Ʒ��� �������� ȿ��
        while (elapsedTime < 1f / fadeSpeed)
        {
            float t = elapsedTime / (1f / fadeSpeed);
            blackoutRect.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��ġ�� ���� ������ �����ϰ� �̹��� ��Ȱ��ȭ
        blackoutRect.localPosition = endPosition;
        blackoutImage.gameObject.SetActive(false);
        Debug.Log("Blackout Image has faded out.");
    }
}

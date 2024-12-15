using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Made by KMS
public class TutoScene_FadeScreen : MonoBehaviour
{
    public Image fadeImage; // ���� ȭ�� �̹���
    public float fadeDuration = 2f; // ���̵� �ð�

    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // ���̵� ���� �� ��Ȱ��ȭ
    }
}

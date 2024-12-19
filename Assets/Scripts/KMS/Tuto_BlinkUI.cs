using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tuto_BlinkUI : MonoBehaviour
{
    public Image tutorialImage; // ��¦�� ȿ���� ������ �̹���
    public float fadeSpeed = 2f; // ���̵� �ӵ�
    public bool useAnyKey = false; // ��� Ű �Է� ���� ����

    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        // Ȱ��ȭ�� ������ ��¦�� ����
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkImage());
    }

    public void StartFadeOut()
    {
        // ���̵�ƿ� ����
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        StartCoroutine(FadeOutImage());
    }

    IEnumerator BlinkImage()
    {
        Color color = tutorialImage.color;

        while (true)
        {
            while (color.a < 1f)
            {
                color.a += fadeSpeed * Time.deltaTime;
                tutorialImage.color = color;
                yield return null;
            }

            while (color.a > 0.2f)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                tutorialImage.color = color;
                yield return null;
            }
        }
    }

    IEnumerator FadeOutImage()
    {
        Color color = tutorialImage.color;

        while (color.a > 0f)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            tutorialImage.color = color;
            yield return null;
        }

        tutorialImage.gameObject.SetActive(false); // ������ �������� �� ��Ȱ��ȭ
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tuto_BlinkUI : MonoBehaviour
{

    public Image tutorialImage;      // ������/���̵�ƿ� ��� �̹���

    public float fadeSpeed = 2f;     // ���̵� �ӵ�

    public KeyCode fadeOutKey = KeyCode.E;  // �⺻���� E Ű

    public bool useAnyKey = false;      // �ƹ� Ű�� �Է����� ���̵�ƿ�
    public bool isFadedOut = false;      // ���̵�ƿ� �Ϸ� ����

    private Coroutine activeCoroutine;   // ���� ���� ���� �ڷ�ƾ


    void OnEnable()
    {

        isFadedOut = false;  // Ȱ��ȭ�� �� ���̵�ƿ� ���� �ʱ�ȭ

        StopActiveCoroutine();   // ���� ���� ���� �ڷ�ƾ ����

        activeCoroutine = StartCoroutine(FadeInImage());    // ���̵��� ����

    }


    public void StartFadeOut()
    {

        StopActiveCoroutine();   // ���� ���� ���� �ڷ�ƾ ����

        activeCoroutine = StartCoroutine(FadeOutImage());

    }


    private void StopActiveCoroutine()
    {

        if (activeCoroutine != null)
        {

            StopCoroutine(activeCoroutine);     // ���� ���� ���� �ڷ�ƾ ����

            activeCoroutine = null;

        }

    }


    IEnumerator FadeInImage()
    {

        Color color = tutorialImage.color;
        color.a = 0f;
        tutorialImage.color = color;


        while (color.a < 1f)
        {

            color.a += fadeSpeed * Time.deltaTime;
            tutorialImage.color = color;

            yield return null;

        }

        color.a = 1f;   // ���� �� ����
        tutorialImage.color = color;

        activeCoroutine = StartCoroutine(BlinkImage());     // ���̵��� �Ϸ� �� ������ ȿ�� ����

    }


    IEnumerator BlinkImage()
    {

        float timer = 0.8f / fadeSpeed;     // Mathf.PingPong�� �ִ밪�� ��ȯ�ϴ� ����

        while (true)
        {

            float alpha = Mathf.PingPong(timer * fadeSpeed, 0.8f) + 0.2f;
            
            tutorialImage.color = new Color(tutorialImage.color.r, tutorialImage.color.g, tutorialImage.color.b, alpha);

            timer += Time.deltaTime;

            yield return null;

        }

    }



    IEnumerator FadeOutImage()
    {

        Color color = tutorialImage.color;


        while (color.a > 0f)
        {

            color.a -= fadeSpeed * Time.deltaTime;  // ���� �� ����
            tutorialImage.color = color;

            yield return null;

        }

        tutorialImage.gameObject.SetActive(false);  // ���̵�ƿ� �Ϸ� �� ��Ȱ��ȭ

        isFadedOut = true;  // ���̵�ƿ� �Ϸ� ���� ������Ʈ

    }

}

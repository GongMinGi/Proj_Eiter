using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tuto_BlinkUI : MonoBehaviour
{

    public Image tutorialImage;     // ������/���̵�ƿ� ��� �̹���

    public float fadeSpeed = 2f;    // ���̵� �ӵ�

    public KeyCode fadeOutKey = KeyCode.E;  // �⺻���� E Ű

    public bool useAnyKey = false;  // �ƹ� Ű�� �Է����� ���̵�ƿ�
    public bool isFadedOut = false;     // ���̵�ƿ� �Ϸ� ����

    private Coroutine blinkCoroutine;

    void OnEnable()
    {

        isFadedOut = false;     // Ȱ��ȭ�� �� ���̵�ƿ� ���� �ʱ�ȭ

        if (blinkCoroutine != null)
        {

            StopCoroutine(blinkCoroutine);

        }

        blinkCoroutine = StartCoroutine(BlinkImage());

    }

    void Update()
    {
        
        if ((useAnyKey && Input.anyKeyDown) || Input.GetKeyDown(fadeOutKey))        // Ű �Է� ����
        {

            StartFadeOut();

        }

    }

    public void StartFadeOut()
    {

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

                color.a += fadeSpeed * Time.deltaTime;      // ���� -> ������
                tutorialImage.color = color;

                yield return null;

            }

            
            while (color.a > 0.2f)
            {   

                color.a -= fadeSpeed * Time.deltaTime;      // ������ -> ����
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

            color.a -= fadeSpeed * Time.deltaTime;      // ���� �� ����
            tutorialImage.color = color;

            yield return null;

        }

        // ���̵�ƿ� �Ϸ�
        tutorialImage.gameObject.SetActive(false);
        isFadedOut = true;  // ���̵�ƿ� �Ϸ� ���� ������Ʈ

    }

}

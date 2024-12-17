using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tuto_BlinkUI : MonoBehaviour
{

    public Image tutorialImage;     // ��¦�̱� ȿ���� ������ Ʃ�丮�� �̹���
    
    public float fadeSpeed = 2f;    // ���̵� ȿ�� �ӵ�

    public KeyCode toggleKey = KeyCode.E; // Ư�� Ű (Inspector���� ���� ����)

    public bool useAnyKey = false;  // ��� Ű �Է��� �������� ���� ����

    private bool isFadingOut = false; // ���̵�ƿ� ������ Ȯ���ϴ� ����

    private Coroutine blinkCoroutine; // ��¦�̱� ȿ�� �ڷ�ƾ�� ����

    void Start()
    {

        blinkCoroutine = StartCoroutine(BlinkImage());      // ���� ���� �� ��¦�̱� ȿ�� �ڷ�ƾ ����

    }

    void Update()
    {

        if (!isFadingOut && ((useAnyKey && Input.anyKeyDown) || Input.GetKeyDown(toggleKey)))       // ��� Ű �Է��� �����ϰų�, Ư�� Ű�� ����
        {
            
            if (blinkCoroutine != null)
            {

                StopCoroutine(blinkCoroutine);      // ������ ��¦�̱� ȿ�� �ڷ�ƾ ����

            }

            StartCoroutine(FadeOutImage());     // ���̵�ƿ� ȿ�� ����

        }

    }

    IEnumerator BlinkImage()
    {

        Color color = tutorialImage.color; // Ʃ�丮�� �̹����� �ʱ� ���� ��������

        while (true) // ���� ����
        {

            while (color.a < 1f)        // ���̵� ��: ���� ���� ���� �������� ���� ������(1)���� ����
            {

                color.a += fadeSpeed * Time.deltaTime; // ���� ����
                tutorialImage.color = color;          // ����� ���� ����

                yield return null;                   // ���� �����ӱ��� ���

            }

            while (color.a > 0.2f)      // ���̵� �ƿ�: ���� ���� ���ҽ��� ������ ����(0.2)�� ����
            {

                color.a -= fadeSpeed * Time.deltaTime; // ���� ����
                tutorialImage.color = color;          // ����� ���� ����

                yield return null;                   // ���� �����ӱ��� ���

            }

        }

    }

    IEnumerator FadeOutImage()
    {

        isFadingOut = true; // ���̵�ƿ� ���� ����

        Color color = tutorialImage.color; // ���� ���� ��������
        
        while (color.a > 0f)        // ���̵�ƿ�: ���� ���� ���� ���ҽ��� ���� ����(0)���� ����
        {

            color.a -= fadeSpeed * Time.deltaTime; // ���� ����
            tutorialImage.color = color;          // ����� ���� ����

            yield return null;                   // ���� �����ӱ��� ���

        }
        
        tutorialImage.gameObject.SetActive(false);      // ������ �������� �� ������Ʈ�� ��Ȱ��ȭ

    }

}

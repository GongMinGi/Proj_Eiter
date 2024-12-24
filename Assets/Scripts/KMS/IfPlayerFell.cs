using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IfPlayerFell : MonoBehaviour
{

    private Vector3 lastSafePosition;       // ���������� �������� ���� ������ ������ ��ġ ����
    private Rigidbody2D playerRigidbody;    // �÷��̾� Rigidbody2D

    public float dangerYThreshold = -100f;

    [Header("Fade Settings")]
    public Image fadeScreen;    // Inspector���� �巡���Ͽ� ����
    public float fadeDuration = 1.0f;   // ���̵� ��/�ƿ� ���� �ð�

    private void Awake()
    {

        playerRigidbody = GetComponent<Rigidbody2D>();

        lastSafePosition = transform.position;

        if (fadeScreen == null)
        {

            Debug.LogError("FadeScreen�� �������� �ʾҽ��ϴ�. Inspector���� FadeScreen�� �������ּ���.");

        }
        else
        {

            fadeScreen.gameObject.SetActive(false);     // ó���� ��Ȱ��ȭ

        }

    }

    void Update()
    {

        if (transform.position.y < dangerYThreshold) 
        {

            StartCoroutine(ComeBack());

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("FellPoint"))
        {

            lastSafePosition = transform.position;
            Debug.Log("lastSafePosition updated");

        }

    }

    IEnumerator ComeBack()
    {

        if (fadeScreen == null)
        {

            Debug.LogError("FadeScreen�� �������� �ʾҽ��ϴ�. ���̵� ȿ���� �ǳʶݴϴ�.");

            yield break;

        }

        fadeScreen.gameObject.SetActive(true);      // ���̵彺ũ�� Ȱ��ȭ

        // ���̵�ƿ�
        float fadeTimer = 0.0f;
        Color fadeColor = fadeScreen.color;

        while (fadeTimer < fadeDuration)
        {

            fadeTimer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0, 1, fadeTimer / fadeDuration);
            fadeScreen.color = fadeColor;

            yield return null;

        }

        fadeColor.a = 1;
        fadeScreen.color = fadeColor;

        
        transform.position = lastSafePosition;      // �÷��̾� ��ġ�� ������ ��ġ�� �����ϰ�

        playerRigidbody.linearVelocity = Vector3.zero;      // �ӵ� 0���� �����

        // ���̵���
        fadeTimer = 0.0f;

        while (fadeTimer < fadeDuration)
        {

            fadeTimer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);
            fadeScreen.color = fadeColor;

            yield return null;

        }

        fadeColor.a = 0;
        fadeScreen.color = fadeColor;

        
        fadeScreen.gameObject.SetActive(false);     // ���̵彺ũ�� ��Ȱ��ȭ
    }

}

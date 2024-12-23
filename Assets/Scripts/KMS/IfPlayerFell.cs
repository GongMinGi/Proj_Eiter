using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IfPlayerFell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 lastSafePosition;       // ���������� �������� ���� ������ ������ ��ġ ����
    public float dangerYThreshold = -100f;

    [Header("Fade Settings")]
    public Image fadeScreen; // Inspector���� �巡���Ͽ� ����
    public float fadeDuration = 1.0f; // ���̵� ��/�ƿ� ���� �ð�

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSafePosition = transform.position;

        if (fadeScreen == null)
        {
            Debug.LogError("FadeScreen�� �������� �ʾҽ��ϴ�. Inspector���� FadeScreen�� �������ּ���.");
        }
        else
        {
            fadeScreen.gameObject.SetActive(false); // ó���� ��Ȱ��ȭ
        }
    }

    void Update()
    {
        CheckFell();
    }

    private void CheckFell()
    {
        if (!IsEdgeAhead(transform.position))
        {
            Debug.Log($"Last Safe Position updated to: {lastSafePosition}"); // ����� �α� �߰�
            lastSafePosition = transform.position;
        }
        else
        {
            if (transform.position.y < dangerYThreshold)
            {
                StartCoroutine(ComeBack());
            }
        }
    }

    private bool IsEdgeAhead(Vector3 position)
    {
        Vector2 frontVec = new Vector2(position.x + (spriteRenderer.flipX ? -0.5f : 0.5f), position.y); //��������Ʈ ������ ��ġ ���
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));  //����� ���̸� �Ʒ��� ��
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 5, LayerMask.GetMask("Platform")); // "Platform" ���̾�� �����ɽ�Ʈ ����

        if (rayHit.collider == null)    // �����ɽ�Ʈ�� �浹���� ������ ����������
        {
            return true;
        }
        return false;   //���������� �ƴ�
    }

    IEnumerator ComeBack()
    {
        if (fadeScreen == null)
        {
            Debug.LogError("FadeScreen�� �������� �ʾҽ��ϴ�. ���̵� ȿ���� �ǳʶݴϴ�.");
            yield break;
        }

        // ���̵彺ũ�� Ȱ��ȭ
        fadeScreen.gameObject.SetActive(true);

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

        // �÷��̾� ��ġ�� ������ ��ġ�� ����
        transform.position = lastSafePosition;

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

        // ���̵彺ũ�� ��Ȱ��ȭ
        fadeScreen.gameObject.SetActive(false);
    }
}

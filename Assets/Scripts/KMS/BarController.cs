using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Image chargeBar;          // ������ �� (UI �̹���)
    public float chargeDuration = 2f; // �������� ������ ���� �� �ɸ��� �ð� (��)
    public float fadeOutDuration = 1f; // ������ �ٰ� ������� ���̵�ƿ� �ð� (��)

    private bool isCharging = false; // ���� ������ Ȯ��
    private bool isFadingOut = false; // ���̵�ƿ� ������ Ȯ��
    private bool isInTrigger = false; // Ʈ���� ���� ���� �ִ��� Ȯ��
    private float chargeTimer = 0f;  // ���� �ð��� ����

    void Start()
    {
        if (chargeBar != null)
        {
            chargeBar.fillAmount = 0;  // ������ �ʱ�ȭ
            SetChargeBarAlpha(0); // ���� �� ���İ� 0���� ���� (�� ���̰�)
        }
    }

    void Update()
    {
        if (isInTrigger && !isFadingOut)
        {
            // ���콺 ��Ŭ�� ���� �� ���� ����
            if (Input.GetMouseButton(0))
            {
                if (!isCharging)
                {
                    StartCharging(); // ���� ����
                }

                UpdateCharging(); // ���� ������Ʈ
            }
            else if (isCharging)
            {
                StopCharging(); // ���� ���
            }
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0f; // Ÿ�̸� �ʱ�ȭ
        if (chargeBar != null)
        {
            chargeBar.fillAmount = 0;
        }
    }

    private void UpdateCharging()
    {
        if (chargeBar != null)
        {
            chargeTimer += Time.deltaTime;
            chargeBar.fillAmount = chargeTimer / chargeDuration; // ������ ä���

            if (chargeTimer >= chargeDuration)
            {
                CompleteCharging(); // ���� �Ϸ�
            }
        }
    }

    private void StopCharging()
    {
        isCharging = false;
        if (chargeBar != null)
        {
            chargeBar.fillAmount = 0; // ������ �ʱ�ȭ
        }
    }

    private void CompleteCharging()
    {
        isCharging = false; // ���� ���� ����
        if (chargeBar != null)
        {
            chargeBar.fillAmount = 1; // �������� ������ ä��
            Debug.Log("Charging Complete!"); // ����� �޽��� ���

            // ���̵�ƿ� ����
            StartCoroutine(FadeOutChargeBar());
        }
    }

    private System.Collections.IEnumerator FadeOutChargeBar()
    {
        isFadingOut = true;
        float fadeTimer = 0f;
        Color color = chargeBar.color;

        while (fadeTimer < fadeOutDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, fadeTimer / fadeOutDuration); // ���İ� ������ ����
            color.a = alpha;
            chargeBar.color = color;
            yield return null;
        }

        color.a = 0; // ������ ���� ����
        chargeBar.color = color;

        isFadingOut = false;

        // ������ �ʱ�ȭ
        chargeTimer = 0f;
        chargeBar.fillAmount = 0;
        Debug.Log("Charge Bar faded out.");
    }

    private void SetChargeBarAlpha(float alpha)
    {
        if (chargeBar != null)
        {
            Color color = chargeBar.color;
            color.a = alpha; // ���İ� ����
            chargeBar.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFadingOut)
        {
            isInTrigger = true;
            SetChargeBarAlpha(1); // ������ �� ���̱�
            Debug.Log("Player entered trigger. Charge bar visible.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            if (!isCharging && !isFadingOut) // ���� ���̰ų� ���̵�ƿ� ���� �ƴ� ��쿡�� �����
            {
                SetChargeBarAlpha(0); // ������ �� �����
                Debug.Log("Player exited trigger. Charge bar hidden.");
            }
        }
    }
}

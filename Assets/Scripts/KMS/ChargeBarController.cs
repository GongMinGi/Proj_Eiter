using UnityEngine;
using UnityEngine.UI;

public class ChargeBarController : MonoBehaviour
{
    public Image chargeBar;          // ������ �� (UI �̹���)
    public float chargeDuration = 2f; // �������� ������ ���� �� �ɸ��� �ð� (��)
    public float fadeOutDuration = 1f; // ������ �ٰ� ������� ���̵�ƿ� �ð� (��)

    private bool isCharging = false; // ���� ������ Ȯ��
    private bool isFadingOut = false; // ���̵�ƿ� ������ Ȯ��
    private bool isInTrigger = false; // Ʈ���� ���� ���� �ִ��� Ȯ��
    private bool isHidden = false;    // �������� ������ �������� Ȯ��
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
        if (isHidden) return; // �������� ������ ���¶�� �ƹ��͵� ���� ����

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
            SetChargeBarAlpha(1); // ������ �� ���̵��� ����
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
            chargeBar.fillAmount = 0;
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
        isHidden = true; // �������� ������ ���������� ǥ��
        Debug.Log("Charge Bar faded out and hidden.");
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
        if (other.CompareTag("Player") && !isFadingOut && !isHidden)
        {
            isInTrigger = true;
            SetChargeBarAlpha(1); // ������ �� ������ ��Ÿ��
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFadingOut && !isHidden)
        {
            isInTrigger = false;
            StartCoroutine(FadeOutChargeBar()); // ������ �� ������ �������
        }
    }
}

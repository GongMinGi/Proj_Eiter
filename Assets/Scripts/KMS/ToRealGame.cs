using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ToRealGame : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Transform cameraTransform;
    private Vector3 cameraOriginalPosition;
    public float dangerYThreshold = -50f;

    [Header("Fade Settings")]
    public Image fadeScreen;
    public float fadeSpeed = 1.0f;

    [Header("Scene Settings")]
    public string sceneToLoad = "CharacterAndTile";

    private bool isTransitioning = false;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;

        // ���̵� ȭ�� �ʱ�ȭ
        if (fadeScreen == null)
        {
            Debug.LogError("FadeScreen�� �������� �ʾҽ��ϴ�. Inspector���� FadeScreen�� �������ּ���.");
        }
        else
        {
            fadeScreen.gameObject.SetActive(false);
            Color initialColor = fadeScreen.color;
        }
    }

    private void Update()
    {
        if (!isTransitioning && transform.position.y < dangerYThreshold)
        {
            playerRigidbody.linearVelocity = Vector2.zero; // �÷��̾� ������ ����

            // ī�޶� ���� ��ġ ����
            cameraOriginalPosition = cameraTransform.position;

            // ī�޶�� �÷��̾��� Parent ���� ����
            if (cameraTransform.parent == transform)
            {
                cameraTransform.SetParent(null, true); // �θ� ���� ���� (���� ��ǥ ����)
            }

            StartCoroutine(TransitionScene());
        }
    }

    IEnumerator TransitionScene()
    {
        isTransitioning = true;

        // ī�޶� ��ġ ����
        Vector3 fixedCameraPosition = cameraTransform.position;

        // 0.5�� ���
        yield return new WaitForSeconds(0.8f);

        // ���� ȭ�� ���̵� �� ȿ��
        yield return StartCoroutine(FadeOutEffect());

        yield return new WaitForSeconds(0.5f);

        // �� ��ȯ
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeOutEffect()
    {
        RectTransform fadeScreenRect = fadeScreen.rectTransform;

        // �ʱ� ��ġ: ȭ�� �Ʒ�
        Vector3 startPosition = new Vector3(0, -Screen.height, 0);
        Vector3 endPosition = Vector3.zero; // ȭ�� �߾����� �̵�

        fadeScreenRect.localPosition = startPosition; // �ʱ� ��ġ ����
        fadeScreen.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < 1f / fadeSpeed) // ���̵� �ӵ��� ���� �ð� ���
        {
            float t = elapsedTime / (1f / fadeSpeed);

            fadeScreenRect.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            // ī�޶� ��ġ�� ��� ����
            cameraTransform.position = cameraOriginalPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        fadeScreenRect.localPosition = endPosition; // ���� ��ġ ����
    }
}

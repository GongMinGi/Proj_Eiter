using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�
using System.Collections;

public class ElevatorMovement : MonoBehaviour
{
    public Transform player;                // �÷��̾� Transform
    public Transform mainCamera;            // ���� ī�޶� Transform
    public float moveSpeed = 2f;            // ���������� �̵� �ӵ�
    public float moveDuration = 5f;         // ���������Ͱ� �ö� �ð�
    public Image blackoutImage;             // ������ �ܻ� �̹��� (UI)
    public float blackoutSpeed = 2f;        // ������ �̹����� �������� �ӵ�
    public string nextSceneName = "LabARooftopScene"; // �̵��� �� �̸�

    private bool isPlayerOnPlatform = false; // �÷��̾ �÷��� ���� �ִ��� Ȯ��
    private bool isMoving = false;          // ���������Ͱ� �����̴��� Ȯ��
    private float moveTimer = 0f;           // �̵� �ð� Ÿ�̸�
    private Vector3 cameraFixedPosition;    // ī�޶� ���� ��ġ ����

    void Start()
    {
        // ���ƿ� �̹��� �ʱ� ���¸� ��Ȱ��ȭ
        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // �÷��̾ �÷��� ���� �ְ� E Ű�� ������ ���������� ����
        if (isPlayerOnPlatform && !isMoving && Input.GetKeyDown(KeyCode.E))
        {
            isMoving = true;
            moveTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            FixCameraPosition(); // ī�޶� ��ġ ����
        }

        // ���������� �̵�
        if (isMoving)
        {
            MoveElevatorUp();
        }
    }

    private void MoveElevatorUp()
    {
        if (moveTimer < moveDuration)
        {
            // �÷����� �÷��̾��� Y ��ǥ�� ������ ����
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            player.position += Vector3.up * moveSpeed * Time.deltaTime;

            // Ÿ�̸� ����
            moveTimer += Time.deltaTime;

            // �ð��� 1�� ������ �� ���� �̹��� �ִϸ��̼� ����
            if (moveDuration - moveTimer <= 1f)
            {
                StartCoroutine(BlackoutEffect());
            }
        }
        else
        {
            isMoving = false;
            Debug.Log("Elevator stopped after reaching the time limit.");
            StartCoroutine(TransitionToNextScene()); // ���� ������ �̵�
        }
    }

    private void FixCameraPosition()
    {
        // ī�޶��� ���� ��ġ�� �����ϰ� ����
        cameraFixedPosition = mainCamera.position;
    }

    private void LateUpdate()
    {
        // ī�޶� ������ ��ġ�� ����
        if (isMoving)
        {
            mainCamera.position = cameraFixedPosition;
        }
    }

    private IEnumerator BlackoutEffect()
    {
        if (blackoutImage.gameObject.activeSelf) yield break; // �ߺ� ���� ����

        blackoutImage.gameObject.SetActive(true); // ������ �̹��� Ȱ��ȭ

        RectTransform blackoutRect = blackoutImage.rectTransform;

        // �̹��� ���� ��ġ (ȭ�� �� �ٱ���)�� �� ��ġ (ȭ�� �߾�) ����
        Vector3 startPosition = new Vector3(0, Screen.height, 0); // ȭ�� ���� �ٱ�
        Vector3 endPosition = Vector3.zero; // ȭ�� �߾�

        float elapsedTime = 0f;

        while (elapsedTime < 1f / blackoutSpeed)
        {
            // �̹����� ��ġ�� ������ �Ʒ��� ������ �̵�
            float t = elapsedTime / (1f / blackoutSpeed);
            blackoutRect.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blackoutRect.localPosition = endPosition; // ��ġ ����
        Debug.Log("Screen blacked out.");
    }

    private IEnumerator TransitionToNextScene()
    {
        // ���ƿ��� ���� �� ��� ��ٸ� �� �� ��ȯ
        yield return new WaitForSeconds(1f);
        Debug.Log("Transitioning to next scene...");
        SceneManager.LoadScene(nextSceneName); // ���� ������ �̵�
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            Debug.Log("Player is on the platform. Press 'E' to start elevator.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            Debug.Log("Player left the platform.");
        }
    }
}

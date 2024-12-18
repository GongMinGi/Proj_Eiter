using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ElevatorMovement : MonoBehaviour
{

    public Transform player;                // �÷��̾� Transform
    public Transform mainCamera;            // ���� ī�޶� Transform

    public Image blackoutImage;             // ������ �ܻ� �̹��� (UI)

    public float moveSpeed = 2f;            // ���������� �̵� �ӵ�
    public float moveDuration = 5f;         // ���������Ͱ� �ö� �ð�
    public float blackoutSpeed = 2f;        // ������ �̹����� �������� �ӵ�

    private bool isPlayerOnPlatform = false; // �÷��̾ �÷��� ���� �ִ��� Ȯ��
    private bool isMoving = false;          // ���������Ͱ� �����̴��� Ȯ��

    private float moveTimer = 0f;           // �̵� �ð� Ÿ�̸�

    private Vector3 cameraFixedPosition;    // ī�޶� ���� ��ġ ����

    void Start()
    {
        
        if (blackoutImage != null)      // ���ƿ� �̹��� �ʱ� ���¸� ��Ȱ��ȭ
        {

            blackoutImage.gameObject.SetActive(false);
        
        }

    }

    void Update()
    {
        
        if (isPlayerOnPlatform && !isMoving && Input.GetKeyDown(KeyCode.E))     // �÷��̾ �÷��� ���� �ְ� E Ű�� ������ ���������� ����
        {

            isMoving = true;

            moveTimer = 0f; // Ÿ�̸� �ʱ�ȭ

            FixCameraPosition(); // ī�޶� ��ġ ����

        }

        if (isMoving)        // ���������� �̵�
        {

            MoveElevatorUp();

        }

    }

    private void MoveElevatorUp()
    {

        if (moveTimer < moveDuration)
        {
            
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;      // �÷����� Y ��ǥ�� ������ ����
            player.position += Vector3.up * moveSpeed * Time.deltaTime;         // �÷��̾��� Y ��ǥ�� ������ ����

            moveTimer += Time.deltaTime;        // Ÿ�̸� ����
            
            if (moveDuration - moveTimer <= 1f)     // �ð��� 1�� ������ �� ���� �̹��� �ִϸ��̼� ����
            {

                StartCoroutine(BlackoutEffect());

            }

        }
        else
        {

            isMoving = false;

            SceneManager.LoadScene("LabARooftopScene");

        }

    }

    private void FixCameraPosition()
    {
        
        cameraFixedPosition = mainCamera.position;      // ī�޶��� ���� ��ġ�� �����ϰ� ����

    }

    private void LateUpdate()
    {
        
        if (isMoving)       
        {

            mainCamera.position = cameraFixedPosition;      // ī�޶� ������ ��ġ�� ����

        }

    }

    private IEnumerator BlackoutEffect()
    {

        if (blackoutImage.gameObject.activeSelf) yield break;   // �ߺ� ���� ����

        blackoutImage.gameObject.SetActive(true);   // ������ �̹��� Ȱ��ȭ

        RectTransform blackoutRect = blackoutImage.rectTransform;

        Vector3 startPosition = new Vector3(0, Screen.height, 0);   // �̹��� ���� ��ġ (ȭ�� ���� �ٱ�) ����
        Vector3 endPosition = Vector3.zero;     // �̹��� �� ��ġ (ȭ�� �߾�) ����

        float elapsedTime = 0f;

        while (elapsedTime < 1f / blackoutSpeed)
        {
            
            float t = elapsedTime / (1f / blackoutSpeed);
            blackoutRect.localPosition = Vector3.Lerp(startPosition, endPosition, t);       // �̹����� ��ġ�� ������ �Ʒ��� ������ �̵�

            elapsedTime += Time.deltaTime;

            yield return null;

        }

        blackoutRect.localPosition = endPosition; // ��ġ ����

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            isPlayerOnPlatform = true;

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            isPlayerOnPlatform = false;

        }

    }

}

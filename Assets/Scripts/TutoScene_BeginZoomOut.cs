using System.Collections;
using UnityEngine;

// Made by KMS
public class TutoScene_BeginZoomOut : MonoBehaviour
{
    public Camera mainCamera;      // ���� ī�޶�
    public Transform target;       // ĳ���� ��ġ
    public float zoomInSize = 2f;  // �� �� �� ī�޶� ũ��
    public float normalSize = 5f;  // ���� ������ ī�޶� ũ��
    public float zoomDuration = 2f; // �� ��/�ƿ� �ð�

    private float elapsedTime = 0f;

    void Start()
    {
        // �ʱ� ī�޶� ��ġ�� ĳ���Ϳ� ���߰� �� �� ����
        mainCamera.transform.position = new Vector3(target.position.x, target.position.y, mainCamera.transform.position.z);
        mainCamera.orthographicSize = zoomInSize;

        // �� �ƿ��� ����
        StartCoroutine(ZoomOutEffect());
    }

    IEnumerator ZoomOutEffect()
    {
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            // Orthographic Size�� ���������� ����
            mainCamera.orthographicSize = Mathf.Lerp(zoomInSize, normalSize, elapsedTime / zoomDuration);
            yield return null;
        }

        // �� �ƿ� �Ϸ� �� ��Ȯ�� ������ ����
        mainCamera.orthographicSize = normalSize;
    }
}

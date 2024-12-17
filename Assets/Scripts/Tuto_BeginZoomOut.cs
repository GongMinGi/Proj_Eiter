using UnityEngine;
using System.Collections;

// Made by KMS
public class Tuto_BeginZoomOut : MonoBehaviour
{

    public Camera mainCamera;      // ���� ī�޶�
    public Transform target;       // ĳ���� ��ġ
    public float zoomInSize = 2f;  // �� �� �� ī�޶� ũ��
    public float normalSize = 5f;  // ���� ������ ī�޶� ũ��
    public float zoomDuration = 2f; // �� �� �� �ƿ� �ð�

    private float elapsedTime = 0f;

    void Start()
    {
        
        mainCamera.transform.position = new Vector3(target.position.x, target.position.y, mainCamera.transform.position.z);     // �ʱ� ī�޶� ��ġ�� ĳ���Ϳ� ���߰� �� �� ����
        mainCamera.orthographicSize = zoomInSize;
        
        StartCoroutine(ZoomOutEffect());        // �� �ƿ��� ����

    }

    void Update()
    {

        mainCamera.transform.position = new Vector3(target.position.x, target.position.y, mainCamera.transform.position.z);

    }


    IEnumerator ZoomOutEffect()
    {

        while (elapsedTime < zoomDuration)
        {

            elapsedTime += Time.deltaTime;

            mainCamera.orthographicSize = Mathf.Lerp(zoomInSize, normalSize, elapsedTime / zoomDuration);       // Orthographic Size�� ���������� ����

            yield return null;

        }
        
        mainCamera.orthographicSize = normalSize;       // �� �ƿ� �Ϸ� �� ��Ȯ�� ������ ����

    }

}
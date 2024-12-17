using UnityEngine;

public class LineTrajectory : MonoBehaviour
{
    public Transform startPoint;        // ���� ����
    public Transform endPoint;          // �� ����
    public float lineDuration = 2.0f;    // ���� ���� �ð�

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawStraightLine();
        lineRenderer.material = GetComponent<LineRenderer>().material;
    }

    private void DrawStraightLine()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        // ���� ���� �ð� �Ŀ� ������ ��Ȱ��ȭ�մϴ�.
        StartCoroutine(DisableLineAfterDelay());
    }

    private System.Collections.IEnumerator DisableLineAfterDelay()
    {
        yield return new WaitForSeconds(lineDuration);
        lineRenderer.positionCount = 0; // ������ ��Ȱ��ȭ�մϴ�.
    }
}
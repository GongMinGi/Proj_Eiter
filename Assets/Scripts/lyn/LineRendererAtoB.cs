using UnityEngine;

public class LineRenderAtoB : MonoBehaviour
{
    public Transform startPoint; // ���� ����
    public Transform endPoint;   // �� ����

    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer ������Ʈ �߰�
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer �ʱ� ����
        lineRenderer.positionCount = 2; // �� ���� �� �ʿ� (������, ����)
        lineRenderer.startWidth = 0.1f; // ���� ���� �β�
        lineRenderer.endWidth = 0.1f;   // ���� �� �β�
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Material ����
        lineRenderer.startColor = Color.red; // �� ����
        lineRenderer.endColor = Color.red;

        // �ʱ� ��ġ ������Ʈ
        UpdateLinePosition();
    }

    void Update()
    {
        // �� �����Ӹ��� �� ��ġ ������Ʈ
        UpdateLinePosition();
    }

    private void UpdateLinePosition()
    {
        // �������� ������ null�� �ƴ� ��쿡�� ����
        if (startPoint != null && endPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position); // ������ ��ġ
            lineRenderer.SetPosition(1, endPoint.position);   // ���� ��ġ
        }
    }
}

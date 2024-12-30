using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShootBeamWithSprite : MonoBehaviour
{
    private SpriteRenderer sr;


    private void Awake()
    {
        // �ݵ�� SpriteRenderer�� �����´�
        sr = GetComponent<SpriteRenderer>();

        // Ȥ�� Inspector���� �� �ٲ�ٸ�, ���⼭�� Sliced ���� ���� ����
        sr.drawMode = SpriteDrawMode.Sliced;
    }

    /// <summary>
    /// ���� ���̸� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="length">�ø��� ���� ����</param>
    public void SetBeamLength(float length)
    {
        // 1) Sliced�� �ƴ� ��� ����ó��
        if (sr.drawMode != SpriteDrawMode.Sliced)
        {
            Debug.LogWarning("SpriteRenderer.drawMode�� Sliced�� �ƴմϴ�.");
            return;
        }

        // 2) ���� size���� x��(����)�� ����
        Vector2 newSize = sr.size;
        newSize.x = length;
        sr.size = newSize;
    }
}

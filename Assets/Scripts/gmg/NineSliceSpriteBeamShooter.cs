using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class NineSliceSpriteBeamShooter : MonoBehaviour
{

    public float minbeamLength = 2f; //���� 0% �϶� ����
    public float maxBeamLength = 8f; // ���� 100% �϶��� ����
    public float minBeamHeight = 0.2f; //���� 0% �� �� ���� (�β�)
    public float maxBeamHeight = 0.5f; //���� 100% �� �� ����


    public float shortBeamDuration = 0.2f; // �߻� ���� ��� ǥ���� �ð�

    public LayerMask hitlayer; // �浹 ������

    private SpriteRenderer spriteR;
    private Coroutine beamRoutine;


    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();

        //��������Ʈ�� ��ο� ���� �̸� �ν����Ϳ��� sliced�� �صΰų� ���⼭ ����

        spriteR.enabled = false; // ���� �ÿ��� ���� ������ �ʰ�
    }


    public void ShootBeam(float chargeRatio)
    {
        //Ȥ�� ���� �ڷ�ƾ�� �������̸� �ߴ�
        if(beamRoutine != null)
        {
            StopCoroutine(beamRoutine);
        }

        //���� ������ ���� ����/ �β� ���
        float beamLength = Mathf.Clamp(minbeamLength, maxBeamLength, chargeRatio);
        float beamHeight = Mathf.Clamp(minBeamHeight, maxBeamHeight, chargeRatio);
    }


    private IEnumerator DoBeamRoutine(float length, float height)
    {
        // 1) �ϴ� ��������Ʈ Ȱ��ȭ
        spriteR.enabled = true;


        // 2) �����ɽ�Ʈ�� �浹���� Ȯ�� 
        Vector2 startPos = transform.position;
        Vector2 dir = transform.right;


        RaycastHit2D hit = Physics2D.Raycast(startPos, dir, length, hitlayer);
        Vector2 endPos;
        if(hit.collider != null)
        {
            endPos = hit.point;
            // ���� ����� �ı� �����̶�� �ı�
            if(hit.collider.CompareTag("Destroyable"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            endPos = startPos + dir * length;
        }

        // 3) 9-slice ��������Ʈ�� "size" ����
        float actualDistance = Vector2.Distance(startPos, endPos);

        spriteR.size = new Vector2(actualDistance, height);

        // 4) ���⿡ ���� ������ or flipó���ʿ�

        // 5) shortBeamDuration ��ŭ ���� �����ְ� �������
        yield return new WaitForSeconds(shortBeamDuration);

        // 6) �� ��Ȱ��
        spriteR.enabled = false;
    }
}

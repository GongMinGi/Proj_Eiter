using UnityEngine;

public class RaycastBeamShooter : MonoBehaviour
{

    [SerializeField] private LineRenderer beamLinePrefab;
    [SerializeField] private Transform firepoint; // �� �߻� ������
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask hitLayers;

    public void ShootBeam()
    {
        //(1)�����ɽ�Ʈ�� �浹 ���� Ȯ�� 
        // �߻� ���� ���� �ʿ�
        RaycastHit2D hit = Physics2D.Raycast(firepoint.position, transform.right, maxDistance, hitLayers);

        //(2)LineRender �����
        LineRenderer beamLine = Instantiate(beamLinePrefab, Vector3.zero, Quaternion.identity);

        Vector2 endPos;
        if(hit.collider != null)
        {
            endPos = hit.point; //�����ɽ�Ʈ�� ���� ����
            if(hit.collider.CompareTag("Destroyable"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            // �浹�� ������ maxDistance ���� �� �׸���
            endPos = (Vector2)firepoint.position + (Vector2)transform.right*maxDistance;
        }

        // 4) LineRenderer �� ������/ ���� ����
        beamLine.positionCount = 2;
        beamLine.SetPosition(0, firepoint.position);
        beamLine.SetPosition(1, endPos);

        //5) ���� ��� ������ �� ����
        Destroy(beamLine.gameObject, 0.5f);
        

    }


}

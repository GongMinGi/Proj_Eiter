using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;          // Animator ������Ʈ ����
    private int hashAttackCnt;      // Animator�� AttackCount ������ �ؽ� ��
    private int attackCount = 0;    // ���� AttackCount ��
    private bool isAttacking = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        //Animator�� attackCount ���� �ؽ�ȭ
        hashAttackCnt = Animator.StringToHash("attackCount");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) )
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        if(isAttacking)
            return;
        attackCount++;

        if (attackCount >2)
        {
            attackCount = 1;
        }

        //Animator�� AttackCount �� ����
        anim.SetInteger(hashAttackCnt, attackCount);


        //Trigger �������� ���� ����
        anim.SetTrigger("Attack");
    }

    public void ResetAttackCount()
    {
        //������ ���� �� AttackCount �ʱ�ȭ 
        //attackCount=0;
        //anim.SetInteger(hashAttackCnt, attackCount);
        isAttacking =false;
    }


}

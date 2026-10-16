using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private MonsterInfo monsterInfo;
    private int hp;
    public bool isDead = false;
    private float frontTime;
    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        hp = info.hp;
        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
    }
    public void Wound(int dmg)
    {
        if (isDead)
            return;
        hp -= dmg;
        animator.SetTrigger("Wound");
        if (hp <= 0)
        {
            //播放死亡动画
            Dead();
        }
        else
        {
            //播放音效
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }
    public void Dead()
    {
        isDead = true;
        //agent.isStopped = true;
        agent.enabled = false;
        animator.SetBool("Dead", true);
        GameDataMgr.Instance.PlaySound("Music/Dead");
        //播放死亡音效
        GameLevelMgr.Instance.player.AddMoney(10);

    }
    public void DeadEvent()
    {
        GameLevelMgr.Instance.RemoveMonster(this);
        Destroy(this.gameObject);
        if(GameLevelMgr.Instance.CheckOver())
        {
           GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLevelMgr.Instance.player.money, true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void BornOver()
    {
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        animator.SetBool("Run", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        animator.SetBool("Run",agent.velocity != Vector3.zero);
        if(Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) <= 5 && Time.time-frontTime>=monsterInfo.atkOffset)
        {
            frontTime = Time.time;
            animator.SetTrigger("Atk");
        }
    }

    public void AtkEvent()
    {
        GameDataMgr.Instance.PlaySound("Music/Eat");
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("MainTower"));
        for (int i = 0; i < colliders.Length; i++)
        {
            MainTowerObject tower = colliders[i].GetComponent<MainTowerObject>();
            if (tower != null)
            {
                tower.Wound(monsterInfo.atk);
            }
        }
    }
}

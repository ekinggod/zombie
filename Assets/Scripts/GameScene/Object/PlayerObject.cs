using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private Animator animator;
    private int atk;
    public int money;
    private float roundSpeed = 50;
    public Transform gunPoint;
    public string effStr;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;
        UpdateMoney();
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        this.transform.Rotate(Vector3.up , Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("Roll");
        }
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }
    }

    public void KnifeEvent()
    {
        GameDataMgr.Instance.PlaySound("Music/Knife");
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1f, 1 << LayerMask.NameToLayer("Monster"));
        for(int i = 0; i < colliders.Length; i++)
        {
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster != null&&!monster.isDead)
            {
                monster.Wound(atk);
                break;
            }
        }
    }

    public void ShootEvent()
    {
        GameDataMgr.Instance.PlaySound("Music/Gun");
        RaycastHit[] hits = Physics.RaycastAll(new Ray(gunPoint.position,this.transform.forward),1000f,1 << LayerMask.NameToLayer("Monster"));
        for(int i = 0; i < hits.Length; i++)
        {
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null)
            {
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1f);
                monster.Wound(atk);
                break;
            }
        }
    }

    public void UpdateMoney() 
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    public void AddMoney(int value)
    {
        money += value;
        UpdateMoney();
    }
}

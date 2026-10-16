using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private int hp;
    private int maxHp;
    private bool isDead;
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public void UpdateHp(int hp,int maxHP)
    {
        this.hp = hp;
        this.maxHp = maxHP;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHP);
    }
    public void Wound(int dmg)
    {
        if(isDead) return;
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        UpdateHp(hp, maxHp);
    }
    private void OnDestroy()
    {
        instance = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel  
{

    public Text txtWin;
    public Text txtInfo;
    public Text txtMoney;
    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();       
            GameLevelMgr.Instance.ClickInfo();
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int money,bool isWin)
    {
        txtMoney.text = "¥" + money.ToString();
        if (isWin)
        {
            txtWin.text = "通关";
            txtInfo.text = "获得胜利奖励";
        }
        else
        {
            txtWin.text = "失败";
            txtInfo.text = "获得失败奖励";
        }
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}

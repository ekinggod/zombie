using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;

    public Button btnUnLock;
    public Text txtUnLock;

    public Button btnStart;
    public Button btnBack;

    public Text txtMoney;
    public Text txtName;

    private Transform heroPos;
    private GameObject heroObj;
    private RoleInfo nowRoleData;
    private int nowIndex;

    public override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;

        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();
        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if(nowIndex < 0)
            {
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            }
            ChangeHero();
        });
        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleInfoList.Count)
            {
                nowIndex = 0;
            }
            ChangeHero();
        });
        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData data = GameDataMgr.Instance.playerData;
            if(data.haveMoney >= nowRoleData.lockMoney)
            {
                data.haveMoney -= nowRoleData.lockMoney;
                data.buyHero.Add(nowRoleData.id);
                GameDataMgr.Instance.SavePlayerData();
                txtMoney.text = data.haveMoney.ToString();
                UpdateLockBtn();
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");
            }
        });
        btnStart.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        ChangeHero();
    }

    private void ChangeHero()
    {
        if(heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position,heroPos.rotation);
        Destroy(heroObj.GetComponent<PlayerObject>());
        txtName.text = nowRoleData.tips;
        UpdateLockBtn();
    }

    private void UpdateLockBtn()
    {
        if(nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            btnUnLock.gameObject.SetActive(true);
            txtUnLock.text = "¥" + nowRoleData.lockMoney.ToString();
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    override public void HideMe(UnityAction callback)
    {
        base.HideMe(callback);
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}

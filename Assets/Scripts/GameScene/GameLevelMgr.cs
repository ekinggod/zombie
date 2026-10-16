using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    public PlayerObject player;
    private List<MonsterPoint> points = new List<MonsterPoint>();

    private int nowWaveNum = 0;
    private int maxWaveNum = 0;

    //private int nowMonsterNum = 0;
    private List<MonsterObject> monsterList = new List<MonsterObject>();
    private GameLevelMgr()
    {

    }

    public void InitInfo(SceneInfo sceneInfo)
    {
        UIManager.Instance.ShowPanel<GamePanel>();
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, Quaternion.identity);
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk,sceneInfo.money);
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);
        MainTowerObject.Instance.UpdateHp(sceneInfo.towerHp, sceneInfo.towerHp);
    }

    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
       
    }

    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWaveNum, maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWaveNum, maxWaveNum);
    }



    public bool CheckOver()
    {
        for(int i = 0; i < points.Count; ++i)
        {
            if (!points[i].CheckOver())
            {
                return false;
            }
        }
        if(monsterList.Count > 0)
        {
            return false;
        }

        return true;
    }

    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }

    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }
    public MonsterObject FindMonster(Vector3 pos,int Range)
    {
        for(int i = 0; i < monsterList.Count; i++)
        {
            if (Vector3.Distance(pos, monsterList[i].transform.position) <= Range && !monsterList[i].isDead)
            {
                return monsterList[i];
            }
        }
        return null;
    }
    public List<MonsterObject> FindMonsters(Vector3 pos,int Range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (Vector3.Distance(pos, monsterList[i].transform.position) <= Range && !monsterList[i].isDead)
            {
                list.Add(monsterList[i]);
            } 
        }
        return list;
    }
    public void ClickInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}


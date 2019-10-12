using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
    public List<int> PitcherDataIDList = new List<int>();
    public int CurrentState;
}

public class UserDataManager : Singleton<UserDataManager>
{
    public PlayerSaveData PlayerSaveData;
    public enum STATE
    {
        NONE = 0,
        CLEAR_1st,
        CLEAR_2st,
        CLEAR_3st,
        CLEAR_4st,
    }

    public override void OnAwake()
    {
        PlayerSaveData = LoadData();
        //投手がいない場合はデフォルトキャラを保存しておく
        if(PlayerSaveData.PitcherDataIDList.Count == 0)
        {
            PlayerSaveData.PitcherDataIDList.Add(0);
            PlayerSaveData.PitcherDataIDList.Add(1);
            SaveData();
        }
    }

    public void SaveData()
    {
        // PlayerPrefs.SetInt("Current", PlayerSaveData.CurrentState);
        // for(int i = 0, il = PlayerSaveData.PitcherDataIDList.Count; i < il; i++)
        // {
        //     PlayerPrefs.SetInt(PitcherDataManager.PITCHER_DATA_PATH_KEY + i, PlayerSaveData.PitcherDataIDList[i]);
        // }
    }

    public PlayerSaveData LoadData()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        if(PlayerPrefs.HasKey("Current"))
        {
            saveData.CurrentState = PlayerPrefs.GetInt("Current");
        }
        
        int pitcherID = 0;
        List<int> pitcherIDList = new List<int>();
        // while(PlayerPrefs.HasKey(PitcherDataManager.PITCHER_DATA_PATH_KEY + pitcherID))
        // {
        //     pitcherIDList.Add(PlayerPrefs.GetInt(PitcherDataManager.PITCHER_DATA_PATH_KEY + pitcherID));
        //     pitcherID++;
        // }
        saveData.PitcherDataIDList = pitcherIDList;
        return saveData;
    }

    /// <summary>
    /// 全削除
    /// </summary>
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}

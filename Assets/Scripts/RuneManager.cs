using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    static RuneManager _instance;
    public static RuneManager instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType(typeof(RuneManager)) as RuneManager;
                if (_instance == null){
                    GameObject go = new GameObject("RuneManager");
                    _instance = go.AddComponent<RuneManager>();
                }
            }
            return _instance;
        }
    }

    public int currentRuneId = 0;
    List<float>[] runes;

    public List<float> currentRuneList{
        get{
            return runes[currentRuneId];
        }
    }

    private void Awake() {
        runes = new List<float>[Config.setNum];
        for(int i=0;i<Config.setNum;++i){
            runes[i] = new List<float>();
        }
    }

    public List<float> GetRuneList(int id){
        return runes[id];
    }
}

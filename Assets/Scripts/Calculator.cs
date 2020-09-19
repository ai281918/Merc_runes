using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Calculator : MonoBehaviour
{
    static Calculator _instance;
    public static Calculator instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType(typeof(Calculator)) as Calculator;
                if (_instance == null){
                    GameObject go = new GameObject("Calculator");
                    _instance = go.AddComponent<Calculator>();
                }
            }
            return _instance;
        }
    }

    struct CalUnit{
        public int setId;
        public int id;
        public int value;

        public CalUnit(int setId, int id, int value){
            this.setId = setId;
            this.id = id;
            this.value = value;
        }
    }

    RuneManager runeManager;
    FileManager fileManager;
    int runeSize = 0, targetSize = 0;
    public InputField runeSizeInputField, targetSizeInputField;
    List<CalUnit> food = new List<CalUnit>();
    public TMP_InputField resultInputField;
    bool[] flag = new bool[1000];
    bool[] ans = new bool[1000];
    int ans_max;
    public Text resultSize;
    bool hasAns = false;

    private void Awake() {
        runeManager = RuneManager.instance;
        fileManager = FileManager.instance;
    }

    public void OnRuneSettingChanged(){
        if(runeManager.currentRuneId >= Config.setNum - 2){
            return;
        }

        float t;
        if(!Single.TryParse(runeSizeInputField.text, out t)){
            return;
        }
        runeSize = (int)(t*1000+0.5f);
        if(!Single.TryParse(targetSizeInputField.text, out t)){
            return;
        }
        targetSize = (int)(t*1000+0.5f);

        if(runeSize <= 27000 || runeSize > 33000) return;
        if(targetSize <= 27000 || targetSize > 33000) return;
        if(runeSize > targetSize) return;
        if(runeSize <= 30000 && targetSize > 30000) return;

        float factor = 1f;
        if(runeSize > 30000) factor = 0.5f;

        food.Clear();
        List<float> runes = runeManager.currentRuneList;
        for(int i=0;i<runes.Count;++i){
            int value = (int)(((int)(runes[i] * 1000 + 0.5f) - 26500) * 0.2f * factor + 0.5f);
            food.Add(new CalUnit(runeManager.currentRuneId, i, value));
        }

        runes = runeManager.GetRuneList(Config.setNum-2);
        for(int i=0;i<runes.Count;++i){
            int value = (int)(((int)(runes[i] * 1000 + 0.5f) - 26500) * 0.1f * factor + 0.5f);
            food.Add(new CalUnit(Config.setNum-2, i, value));
        }

        runes = runeManager.GetRuneList(Config.setNum-1);
        for(int i=0;i<runes.Count;++i){
            int value = (int)(((int)(runes[i] * 1000 + 0.5f) - 26500) * 0.1f * factor + 0.5f);
            food.Add(new CalUnit(Config.setNum-1, i, value));
        }

        Calculate();
    }

    void BT(int current, int size){
        if(size > targetSize || hasAns){
            return;
        }
        if(size > ans_max){
            ans_max = size;
            Array.Copy(flag, ans, food.Count);
            if(ans_max == targetSize){
                hasAns = true;
                return;
            }
        }
        if(current >= food.Count){
            return;
        }

        flag[current] = true;
        BT(current+1, size + food[current].value);
        flag[current] = false;
        BT(current+1, size);
    }

    void Calculate(){
        Array.Clear(ans, 0, ans.Length);
        Array.Clear(flag, 0, flag.Length);
        ans_max = runeSize;
        hasAns = false;

        BT(0, runeSize);

        UpdateFoodInputField();
    }

    void UpdateFoodInputField(){
        string s = "";
        int pre = runeManager.currentRuneId;
        for(int i=0;i<food.Count;++i){
            if(ans[i]){
                if(food[i].setId != pre){
                    s += "----------------\n";
                    pre = food[i].setId;
                }
                s += runeManager.GetRuneList(food[i].setId)[food[i].id].ToString() + "\n";
            }
        }
        resultSize.text = (ans_max / 1000f).ToString();
        resultInputField.SetTextWithoutNotify(s);
        resultInputField.ActivateInputField();
    }

    public void Eat(){
        for(int i=food.Count-1;i>=0;--i){
            if(ans[i]){
                runeManager.GetRuneList(food[i].setId).RemoveAt(food[i].id);
            }
        }
        fileManager.SaveAll();
        fileManager.SelectSet(runeManager.currentRuneId);

        food.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class FileManager : MonoBehaviour
{
    static FileManager _instance;
    public static FileManager instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType(typeof(FileManager)) as FileManager;
                if (_instance == null){
                    GameObject go = new GameObject("FileManager");
                    _instance = go.AddComponent<FileManager>();
                }
            }
            return _instance;
        }
    }

    string currentFileName;
    public int id{
        get{
            return runeManager.currentRuneId;
        }
        set{
            runeManager.currentRuneId = value;
            currentFileName = Config.prefix + runeManager.currentRuneId + Config.postfix;
            Load();
        }
    }

    public TMP_InputField inputField;
    RuneManager runeManager;

    private void Awake() {
        runeManager = RuneManager.instance;
        for(int i=0;i<Config.setNum;i++){
            if(!File.Exists("set_" + i + ".txt")){
                File.Create("set_" + i + ".txt");
            }
        }
    }

    private void Start() {
        LoadAll();
        id = runeManager.currentRuneId;
    }

    public void SelectSet(int id){
        this.id = id;
    }

    void Load(){
        List<float> runes = runeManager.currentRuneList;
        runes.Clear();

        StreamReader sr = new StreamReader(currentFileName);
        string s;
        float v;
        while(!sr.EndOfStream){
            s = sr.ReadLine();
            if(Single.TryParse(s, out v)){
                runes.Add(v);
            }
        }
        sr.Close();
        UpdateInputField();
    }

    void LoadAll(){
        for(int i=0;i<Config.setNum;++i){
            string fileName = Config.prefix + i.ToString() + Config.postfix;
            List<float> runes = runeManager.GetRuneList(i);
            StreamReader sr = new StreamReader(fileName);
            string s;
            float v;
            while(!sr.EndOfStream){
                s = sr.ReadLine();
                if(Single.TryParse(s, out v)){
                    runes.Add(v);
                }
            }
            sr.Close();
        }
    }

    public void SaveAll(){
        for(int i=0;i<Config.setNum;++i){
            string fileName = Config.prefix + i.ToString() + Config.postfix;
            List<float> runes = runeManager.GetRuneList(i);
            StreamWriter sw = new StreamWriter(fileName);
            for(int j=0;j<runes.Count;++j){
                sw.WriteLine(runes[j].ToString());
            }
            sw.Close();
        }
    }

    public void OnEditEnd(){
        UpdateRunesFromInputField(); // 去掉奇怪的數值
        Save(); // 儲存
    }

    void UpdateInputField(){
        List<float> runes = runeManager.currentRuneList;
        string s = "";
        for(int i=0;i<runes.Count;++i){
            s += runes[i] + "\n";
        }
        inputField.SetTextWithoutNotify(s);
        inputField.ActivateInputField();
        // inputField.text = s;
    }

    void Save(){
        List<float> runes = runeManager.currentRuneList;
        StreamWriter sw = new StreamWriter(currentFileName);
        for(int i=0;i<runes.Count;++i){
            sw.WriteLine(runes[i].ToString());
        }
        sw.Close();
    }

    void UpdateRunesFromInputField(){
        List<float> runes = runeManager.currentRuneList;
        string[] s = inputField.text.Split('\n');
        runes.Clear();
        float v;
        for(int i=0;i<s.Length;++i){
            if(Single.TryParse(s[i], out v) && v > 27f && v <= 35f){
                runes.Add(v);
            }
        }
    }
}

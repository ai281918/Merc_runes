using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWeb : MonoBehaviour
{
    public void Open(){
        Application.OpenURL("https://twitter.com/kuroriddle/status/1307141891792674816?s=20");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup {

    private PopupType pt;
    private GameObject go = null;
    private Dictionary<string, Transform> childs = new Dictionary<string, Transform>();

    public Popup(PopupType pt) {

        this.pt = pt;
        string s2 = pt.ToString().ToLower();
        string s = Char.ToUpper(s2[0]).ToString();
        for (int i = 1; i < s2.Length; i++) {

            s += s2[i];

        }

        foreach (Transform t in GameObject.FindGameObjectWithTag("PopupCanvas").transform) {

            if (t.name == s) {

                go = t.gameObject;

            }

        }

    }

    public PopupType getType() {
        return pt;
    }

    public void activate() {
        if (go == null) return;

        go.SetActive(true);

    }

    public void unactivate()
    {
        if (go == null) return;

        go.SetActive(false);

    }

}

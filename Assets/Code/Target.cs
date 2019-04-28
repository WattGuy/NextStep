using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target {

    private HeroType ht = HeroType.NONE;
    private OnType ot = OnType.NONE;
    private int target;
    private GameObject go;
    private Text text = null;

    public Target(OnType ot, int target) {

        this.ot = ot;
        this.target = target;
        go = GameObject.Instantiate(Board.target, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("TargetBars").transform);
        foreach (Transform child in go.GetComponentsInChildren<Transform>())
        {

            if (child.name == "Text")
            {
                Text t = child.GetComponent<Text>();
                t.text = "" + target;
                this.text = t;

            }
            else if (child.name == "Image")
            {
                Image i = child.GetComponent<Image>();
                i.sprite = Board.rs.ons[ot];

            }

        }

    }

    public Target(HeroType ht, int target) {

        this.ht = ht;
        this.target = target;
        go = GameObject.Instantiate(Board.target, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("TargetBars").transform);
        foreach (Transform child in go.GetComponentsInChildren<Transform>()) {

            if (child.name == "Text") {
                Text t = child.GetComponent<Text>();
                t.text = "" + target;
                this.text = t;

            }else if (child.name == "Image")
            {
                Image i = child.GetComponent<Image>();
                i.sprite = Board.rs.sprites[new KeyValuePair<HeroType, DLCType>(ht, DLCType.NONE)];

            }

        }

    }

    public void update() {

        if (text != null) {

            text.text = target.ToString();

        }

    }

    public void minus(int i) {

        if (i >= target)
        {

            target = 0;

        }
        else target -= i;

    }

    public int getTarget()
    {
        return target;
    }

    public GameObject getObject() {
        return go;
    }

    public bool isDone() {
        return target <= 0;
    }
	
}

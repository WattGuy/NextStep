using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup {

    private PopupType pt;
    private GameObject go = null;

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
        if (Board.rs == null) Board.rs = new Resources();

        if (pt == PopupType.LEVEL) {

            foreach (Transform child in go.transform) {

                if (child.name == "Level") {

                    child.GetComponent<Text>().text = "УРОВЕНЬ " + ContentManager.id;

                }
                else if (child.name == "Grid")
                {

                    foreach (Transform child2 in child.transform)
                    {
                        GameObject.Destroy(child2.gameObject);
                    }

                    foreach (TargetData td in ContentManager.level.targets) {
                        HeroType? ht = td.getType();
                        if (ht != null)
                        {

                            Sprite s = Board.rs.sprites[new KeyValuePair<HeroType, DLCType>((HeroType)ht, DLCType.NONE)];
                            ContentManager.targetPopup.GetComponent<Image>().sprite = s;
                            ContentManager.targetPopup.GetComponentInChildren<Text>().text = td.target.ToString();
                            GameObject.Instantiate(ContentManager.targetPopup, child);

                        }
                        else {
                            OnType? ot = td.getOType();
                            if (ot == null) continue;

                            Sprite s = Board.rs.ons[(OnType)ot];
                            ContentManager.targetPopup.GetComponent<Image>().sprite = s;
                            ContentManager.targetPopup.GetComponentInChildren<Text>().text = td.target.ToString();
                            GameObject.Instantiate(ContentManager.targetPopup, child);

                        }

                    }

                }
                else if (child.name == "PlayLevel") {

                    if (ContentManager.data.energy >= ContentManager.level.energy)
                    {
                        child.gameObject.SetActive(true);

                        foreach (Transform child2 in child.transform)
                        {

                            if (child2.name == "Number")
                            {

                                child2.GetComponent<Text>().text = ContentManager.level.energy.ToString();

                            }

                        }

                    }
                    else child.gameObject.SetActive(false);

                }
                else if (child.name == "NotEnoughEnergy")
                {

                    if (ContentManager.data.energy < ContentManager.level.energy)
                    {
                        child.gameObject.SetActive(true);

                        foreach (Transform child2 in child.transform)
                        {

                            if (child2.name == "Number")
                            {

                                child2.GetComponent<Text>().text = ContentManager.level.energy.ToString();

                            }

                        }

                    }
                    else child.gameObject.SetActive(false);

                }

            }

        } else if (pt == PopupType.LOSE) {

            foreach (Transform child in go.transform) {

                if (child.name == "Restart" && ContentManager.data.energy >= ContentManager.level.energy)
                {
                    child.gameObject.SetActive(true);

                    foreach (Transform schild in child.transform)
                    {
                        if (schild.name != "Number") continue;

                        schild.GetComponent<Text>().text = ContentManager.level.energy.ToString();
                        break;

                    }

                }
                else if (child.name == "Restart")
                    child.gameObject.SetActive(false);

                if (child.name == "RestartNotEnough" && ContentManager.data.energy < ContentManager.level.energy)
                {
                    child.gameObject.SetActive(true);

                    foreach (Transform schild in child.transform)
                    {
                        if (schild.name != "Number") continue;

                        schild.GetComponent<Text>().text = ContentManager.level.energy.ToString();
                        break;

                    }

                }
                else if (child.name == "RestartNotEnough")
                    child.gameObject.SetActive(false);

            }

        } else if (pt == PopupType.PAUSE) {

            foreach (Transform child in go.transform)
            {
                if (child.name != "Surrender") continue;

                foreach (Transform schild in child.transform) {
                    if (schild.name != "Number") continue;

                    schild.GetComponent<Text>().text = (Board.instance.steps.getSteps() == ContentManager.level.steps ? "0" : "1");
                    break;

                }

            }

        }

        go.SetActive(true);

    }

    public GameObject GetGameObject() {

        return go;

    }

    public void unactivate()
    {
        if (go == null) return;

        go.SetActive(false);

    }

}

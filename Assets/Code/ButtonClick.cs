﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.tag == "ShopItem" && name == "Buy")
        {

            if (!(ContentManager.data != null && ContentManager.data.purchases != null)) return;

            DLCType? dt = getKeyByValue(ShopManager.dict, transform.parent.gameObject);

            if (dt == null) return;

            ShopManager.BuyItem((DLCType) dt);

        }
        else if (transform.parent.tag == "ShopItem" && name == "Takeoff")
        {

            ShopManager.SelectItem(DLCType.NONE);

        }
        else if (transform.parent.tag == "ShopItem" && name == "Puton") {
            DLCType? dt = getKeyByValue(ShopManager.dict, transform.parent.gameObject);

            if (dt == null) return;

            ShopManager.SelectItem((DLCType) dt);

        }
        else if (tag == "ShopIcon") {
            HeroType? ht = TypeUtils.getType(name.ToUpper());

            if (ht == null) return;

            ShopManager.SelectIcon((HeroType)ht);

        }
        else if (tag == "LevelButton") {
            Text t = transform.GetChild(0).GetComponent<Text>();
            int? i = tryParse(t.text);

            if (i == null) return;

            int i2 = (int)i;

            if (i2 > ContentManager.data.last_level + 1)
            {
                return;
            }

            TextAsset ass = ContentManager.levels[i2];
            LevelData d = Saver.loadLevel(ass.text);

            if (d == null) return;

            ContentManager.id = i2;
            ContentManager.level = d;
            SceneManager.LoadScene("game");

        }
        else if ("Settings" == name)
        {



        }
        else if ("About" == name)
        {



        }
        else if ("Store" == name)
        {

            SceneManager.LoadScene("shop");

        } else if ("Menu" == name) {

            SceneManager.LoadScene("menu");

        }
        else if ("Levels" == name || "Quit" == name)
        {

            SceneManager.LoadScene("levels");

        }
        else if ("Restart" == name)
        {

            SceneManager.LoadScene("game");

        }
        else if ("Pause" == name)
        {

            Board.instance.popups[PopupType.PAUSE].activate();
            Board.paused = true;

        }
        else if ("Continue" == name) {

            Board.instance.popups[PopupType.PAUSE].unactivate();
            Board.paused = false;

        }


    }

    private DLCType? getKeyByValue(Dictionary<DLCType, GameObject> dict, GameObject value) {

        foreach (KeyValuePair<DLCType, GameObject> pair in dict) {

            if (pair.Value == value) return pair.Key;

        }

        return null;

    }

    private int? tryParse(string s) {

        try {

            return int.Parse(s);

        }
        catch (Exception e) {}

        return null;

    }

}

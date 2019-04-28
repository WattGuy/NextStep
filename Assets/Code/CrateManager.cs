using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrateManager : MonoBehaviour
{

    public static Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();
    public GameObject crate;

    public static GameObject coins;
    public GameObject _coins;

    public static GameObject energy;
    public GameObject _energy;

    public static GameObject key;
    public GameObject _key;

    public enum LootType {

        COINS_30, COINS_10, COINS_20, ENERGY_1, ENERGY_2, ENERGY_3, COINS_5, COINS_1, KEYS_1, KEYS_2

    }

    void Start() {
        dict.Clear();

        if (ContentManager.data == null) ContentManager.data = Saver.load();

        coins = _coins;
        energy = _energy;
        key = _key;

        for (int i = 0; i < 9; i++) {

            GameObject go = GameObject.Instantiate(crate, transform);
            go.name = i.ToString();

            dict.Add(i, go);

        }

    }

    public static void open(int i) {
        if (!dict.ContainsKey(i)) return;

        ContentManager.data.keys -= 1;
        GameObject go = dict[i];

        foreach (Transform child in go.transform) {

            if (child.name == "Lock") {

                child.gameObject.SetActive(false);

            }

        }

        LootType lt = random();
        string slt = lt.ToString().ToLower();

        if (slt.Contains("coins"))
        {
            int n = Int32.Parse(slt.Split('_')[1]);

            GameObject o = GameObject.Instantiate(coins, go.transform);
            o.GetComponentInChildren<Text>().text = "+" + n;

            ContentManager.data.coins += n;

        }
        else if (slt.Contains("energy")) {
            int n = Int32.Parse(slt.Split('_')[1]);

            GameObject o = GameObject.Instantiate(energy, go.transform);
            o.GetComponentInChildren<Text>().text = "+" + n;

            if (ContentManager.data.energy + n >= ContentManager.data.energy_max) ContentManager.data.energy = ContentManager.data.energy_max;
            else ContentManager.data.energy += n;

        }
        else if (slt.Contains("keys"))
        {
            int n = Int32.Parse(slt.Split('_')[1]);

            GameObject o = GameObject.Instantiate(key, go.transform);
            o.GetComponentInChildren<Text>().text = "+" + n;

            ContentManager.data.keys += n;

        }

        Saver.save(ContentManager.data);
        Bars.update();

    }

    public static LootType random() {
        int i = new System.Random().Next(1, 1001);

        if (i <= 75)
        {

            return LootType.COINS_30;

        }
        else if (i <= 125)
        {

            return LootType.ENERGY_2;

        }
        else if (i <= 150)
        {

            return LootType.ENERGY_3;

        }
        else if (i <= 250)
        {

            return LootType.COINS_10;

        }
        else if (i <= 300)
        {

            return LootType.COINS_20;

        } else if (i <= 450) {

            return LootType.ENERGY_1;

        } else if (i <= 500) {

            return LootType.KEYS_1;
            
        }
        else if (i <= 525)
        {

            return LootType.KEYS_2;

        }
        else
        {

            return LootType.COINS_1;

        }

    }

}

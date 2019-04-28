using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    public static HeroType selected;
    private static GameObject content;
    public Material _outline;
    public static Material outline;
    public GameObject _item;
    public static GameObject item;
    public static Dictionary<DLCType, GameObject> dict = new Dictionary<DLCType, GameObject>();

    void Start() {

        content = this.gameObject;
        item = _item;
        outline = _outline;

        if (Board.rs == null) {

            Board.rs = new Resources();

        }

        if (ContentManager.data == null) {

            ContentManager.data = Saver.load();

        }

        if (ContentManager.data.purchases == null)
        {

            ContentManager.data = new GameData();
            Saver.save(ContentManager.data);

        }

        UpdateIcons();
        SelectIcon(HeroType.BLUE);

    }

    private static int getPrice(DLCType dt) {

        switch (dt)
        {
            case DLCType.VERTICAL:
                return 100;
            case DLCType.HORIZONTAL:
                return 150;
            case DLCType.ADJACENT:
                return 200;
            case DLCType.BOMB:
                return 300;
            case DLCType.NONE:
                return 0;
        }

        return 0;

    }

    public static void BuyItem(DLCType dt2) {

        string s = ShopManager.selected.ToString() + ":" + ((DLCType)dt2).ToString();

        if (ContentManager.data.purchases.Contains(s)) return;
        if (ContentManager.data.coins < getPrice(dt2)) return; 

        ContentManager.data.purchases.Add(s);
        ContentManager.data.coins -= getPrice(dt2);
        Saver.save(ContentManager.data);
        Bars.update();

        foreach (Transform child in dict[dt2].transform) {

            if (child.name == "Buy") {

                child.gameObject.SetActive(false);

            } else if (child.name == "Puton") {

                child.gameObject.SetActive(true);

            }

        }

        foreach (DLCType dt in Enum.GetValues(typeof(DLCType)).Cast<DLCType>()) {

            foreach (Transform child in dict[dt].transform) {

                if (child.name == "Buy" && (ContentManager.data.purchases.Contains(selected.ToString() + ":" + dt.ToString()) || dt == DLCType.NONE || ContentManager.data.coins < getPrice(dt)))
                {

                    child.gameObject.SetActive(false);

                }
                else if (child.name == "Nuy") {

                    child.gameObject.SetActive(true);

                }

                if (child.name == "NotEnoughMoney" && (ContentManager.data.purchases.Contains(selected.ToString() + ":" + dt.ToString()) || dt == DLCType.NONE || ContentManager.data.coins >= getPrice(dt)))
                {

                    child.gameObject.SetActive(false);

                } else if (child.name == "NotEnoughMoney") {

                    child.gameObject.SetActive(true);

                }

            }

        }

    }

    public static void SelectItem(DLCType dt) {
        string s;
        if (dt == DLCType.NONE)
        {

            s = "";

        }
        else s = dt.ToString();

        FieldInfo f = ContentManager.data.GetType().GetField(selected.ToString().ToLower());
        string ds = (string)f.GetValue(ContentManager.data);
        DLCType? odt = TypeUtils.getDType(ds);

        if (odt == null && ds == "")
        {

            odt = DLCType.NONE;

        }

        if (odt != null && dict.ContainsKey((DLCType)odt)) {

            foreach (Transform t in dict[(DLCType)odt].transform) {

                if (t.name == "Takeoff") t.gameObject.SetActive(false);
                else if (t.name == "Puton") t.gameObject.SetActive(true);

            }

        }

        if (dict.ContainsKey(dt))
        {

            foreach (Transform t in dict[dt].transform)
            {

                if (t.name == "Takeoff" && dt != DLCType.NONE) t.gameObject.SetActive(true);
                else if (t.name == "Puton") t.gameObject.SetActive(false);

            }

        }

        f.SetValue(ContentManager.data, s);
        Saver.save(ContentManager.data);
        UpdateIcons();

    }

    public static void UpdateIcons() {

        foreach (Transform child in GameObject.FindGameObjectWithTag("ShopIcons").transform)
        {

            foreach (HeroType ht in Enum.GetValues(typeof(HeroType)).Cast<HeroType>()) {

                if (child.name != TypeUtils.getName(ht).Replace(" ", "")) continue;
                string ds = (string)ContentManager.data.GetType().GetField(ht.ToString().ToLower()).GetValue(ContentManager.data);
                DLCType? main = TypeUtils.getDType(ds);
                if (main == null && ds == "") main = DLCType.NONE;
                if (main == null) break;

                child.gameObject.GetComponent<Image>().sprite = Board.rs.sprites[new KeyValuePair<HeroType, DLCType>(ht, (DLCType) main)];

                break;

            }

        }

    }

    public static void SelectIcon(HeroType ht) {
        if (ht == HeroType.CONCRETE || ht == HeroType.NONE) return;

        foreach (Transform t in content.transform) {

            Destroy(t.gameObject);

        }

        dict.Clear();

        foreach (DLCType dt in Enum.GetValues(typeof(DLCType)).Cast<DLCType>()) {

            GameObject item = GameObject.Instantiate(ShopManager.item, new Vector3(0f, 0f, 1f), Quaternion.identity, content.transform);
            string ds = (string)ContentManager.data.GetType().GetField(ht.ToString().ToLower()).GetValue(ContentManager.data);
            DLCType? main = TypeUtils.getDType(ds);

            foreach (Transform child in item.transform) {

                if (child.name == "Icon") {

                    child.GetComponent<Image>().sprite = Board.rs.sprites[new KeyValuePair<HeroType, DLCType>(ht, dt)];

                }

                if (child.name == "Price") {
                    if (dt == DLCType.NONE) child.gameObject.SetActive(false);
                    else {

                        child.GetComponent<Text>().text = getPrice(dt) + " Монет";

                    }

                }

                if (child.name == "Takeoff" && main != null && main == dt) {

                    child.gameObject.SetActive(true);

                }

                if (child.name == "Buy" && (ContentManager.data.purchases.Contains(ht.ToString() + ":" + dt.ToString()) || dt == DLCType.NONE || ContentManager.data.coins < getPrice(dt))) {

                    child.gameObject.SetActive(false);

                } 

                if (child.name == "NotEnoughMoney" && (ContentManager.data.purchases.Contains(ht.ToString() + ":" + dt.ToString()) || dt == DLCType.NONE || ContentManager.data.coins >= getPrice(dt)))
                {

                    child.gameObject.SetActive(false);

                }

                if (child.name == "Puton" && ((ContentManager.data.purchases.Contains(ht.ToString() + ":" + dt.ToString()) && !(main != null && main == dt)) || (dt == DLCType.NONE && ds != ""))) {

                    child.gameObject.SetActive(true);

                }

            }

            dict.Add(dt, item);

        }

        foreach (Transform child in GameObject.FindGameObjectWithTag("ShopIcons").transform)
        {

            if (child.name == TypeUtils.getName(ht).Replace(" ", ""))
            {

                child.GetComponent<Image>().material = null;

            }
            else
            {

                child.GetComponent<Image>().material = outline;

            }
        }

        selected = ht;

    }


}

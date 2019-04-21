using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Counter {

    private GameObject go1;
    private GameObject go2;
    public int counter = 0;
    private HeroType ht;
    private DLCType type = DLCType.NONE;
    private bool working = false;

    public Counter(HeroType ht) {
        this.ht = ht;
        string s = TypeUtils.getName(ht).Replace(" ", "");

        if (ContentManager.data == null) {

            ContentManager.data = Saver.load();

        }

        string data = (string) ContentManager.data.GetType().GetField(ht.ToString().ToLower()).GetValue(ContentManager.data);
        DLCType? type = getType(data);

        if (type != null) {

            working = true;
            this.type = (DLCType)type;
            Sprite sprite = Dot.typeToSprite(ht, this.type);

            go1 = GameObject.FindGameObjectWithTag(s);
            go2 = GameObject.FindGameObjectWithTag(s + "Bar");

            Image img = go1.GetComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 0.4f);
            img.sprite = sprite;

            Image img2 = go2.GetComponent<Image>();
            img2.color = new Color(1f, 1f, 1f, 1f);
            img2.sprite = sprite;

        }

    }

    public void update(){
        if (!working) return;

        go2.GetComponent<Image>().fillAmount = ((counter % Board.need) / (Board.need / 100f)) / 100f;

    }

    public void check(List<Dot> list) {
        if (!working) return;

        if (counter >= Board.need) {

            foreach (Dot d in list)
            {

                d.setDLCType(type);
                counter -= Board.need;
                return;

            }

        }

    }

    public HeroType getHero() {
        return ht;
    }

    private DLCType? getType(string s)
    {

        try
        {
            return (DLCType)Enum.Parse(typeof(DLCType), s);
        }
        catch (Exception ignored) { }

        return null;

    }

}

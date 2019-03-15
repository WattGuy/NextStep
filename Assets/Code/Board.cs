using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {

    public int _width;
    public int _height;
    public GameObject dot;

    public GameObject line;
    public Camera camera;
    public float depth = 5;

    public GameObject _circle;
    public static GameObject circle;

    public static List<Circle> circles = new List<Circle>();

    public static int blueCounter = 0;
    public static int greenCounter = 0;
    public static int orangeCounter = 0;
    public static int lightgreenCounter = 0;
    public static int pinkCounter = 0;

    public static Sprite blue;
    public static Sprite green;
    public static Sprite lightgreen;
    public static Sprite pink;
    public static Sprite orange;

    public static Sprite concrete;

    public static Sprite blue_horizontal;
    public static Sprite blue_vertical;
    public static Sprite blue_bomb;
    public static Sprite blue_adjacent;

    public static Sprite green_horizontal;
    public static Sprite green_vertical;
    public static Sprite green_bomb;
    public static Sprite green_adjacent;

    public static Sprite orange_horizontal;
    public static Sprite orange_vertical;
    public static Sprite orange_bomb;
    public static Sprite orange_adjacent;

    public static Sprite lightgreen_horizontal;
    public static Sprite lightgreen_vertical;
    public static Sprite lightgreen_bomb;
    public static Sprite lightgreen_adjacent;

    public static Sprite pink_horizontal;
    public static Sprite pink_vertical;
    public static Sprite pink_bomb;
    public static Sprite pink_adjacent;

    public static int width;
    public static int height;

    public float lineWidth = 2;

    public int need = 20;

    private SpriteRenderer sr;

    public static Dot[,] dots;

    public static GameData data;

    public static bool selecting = false;
    public static List<Dot> selected = new List<Dot>();
    public static List<GameObject> lines = new List<GameObject>();
    public static bool staying = false;

    void Start () {
        selected = new List<Dot>();
        selecting = false;
        staying = false;

        circle = _circle;

        blue = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.BLUE));
        pink = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.PINK));
        lightgreen = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.LIGHTGREEN));
        green = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.GREEN));
        orange = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.ORANGE));

        concrete = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.CONCRETE));

        blue_horizontal = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.BLUE, DLCType.HORIZONTAL));
        blue_vertical = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.BLUE, DLCType.VERTICAL));
        blue_bomb = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.BLUE, DLCType.BOMB));
        blue_adjacent = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.BLUE, DLCType.ADJACENT));

        lightgreen_horizontal = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.LIGHTGREEN, DLCType.HORIZONTAL));
        lightgreen_vertical = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.LIGHTGREEN, DLCType.VERTICAL));
        lightgreen_bomb = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.LIGHTGREEN, DLCType.BOMB));
        lightgreen_adjacent = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.LIGHTGREEN, DLCType.ADJACENT));

        green_horizontal = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.GREEN, DLCType.HORIZONTAL));
        green_vertical = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.GREEN, DLCType.VERTICAL));
        green_bomb = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.GREEN, DLCType.BOMB));
        green_adjacent = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.GREEN, DLCType.ADJACENT));

        orange_horizontal = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.ORANGE, DLCType.HORIZONTAL));
        orange_vertical = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.ORANGE, DLCType.VERTICAL));
        orange_bomb = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.ORANGE, DLCType.BOMB));
        orange_adjacent = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.ORANGE, DLCType.ADJACENT));

        pink_horizontal = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.PINK, DLCType.HORIZONTAL));
        pink_vertical = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.PINK, DLCType.VERTICAL));
        pink_bomb = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.PINK, DLCType.BOMB));
        pink_adjacent = Resources.Load<Sprite>(TypeUtils.GetDirectory(HeroType.PINK, DLCType.ADJACENT));

        data = Saver.load();
        activateBars();

        width = _width;
        height = _height;

        sr = dot.GetComponent<SpriteRenderer>() as SpriteRenderer;
        dots = new Dot[width, height];
        Setup();
	}

    private void checkBar(HeroType ht) {

        if (ht == HeroType.BLUE) {
            DLCType? t = getType(data.blue);

            if (t != null) {

                GameObject.FindGameObjectWithTag("BlueBar").GetComponent<Image>().fillAmount =  ((blueCounter % need) / (need / 100f)) / 100f;

            }

        }else if (ht == HeroType.GREEN)
        {
            DLCType? t = getType(data.green);

            if (t != null)
            {

                GameObject.FindGameObjectWithTag("GreenBar").GetComponent<Image>().fillAmount = ((greenCounter % need) / (need / 100f)) / 100f;

            }

        }
        else if (ht == HeroType.LIGHTGREEN)
        {
            DLCType? t = getType(data.lightgreen);

            if (t != null)
            {

                GameObject.FindGameObjectWithTag("LightGreenBar").GetComponent<Image>().fillAmount = ((lightgreenCounter % need) / (need / 100f)) / 100f;

            }

        }
        else if (ht == HeroType.ORANGE)
        {
            DLCType? t = getType(data.orange);

            if (t != null)
            {

                GameObject.FindGameObjectWithTag("OrangeBar").GetComponent<Image>().fillAmount = ((orangeCounter % need) / (need / 100f)) / 100f;

            }


        }
        else if (ht == HeroType.PINK)
        {
            DLCType? t = getType(data.pink);

            if (t != null)
            {

                GameObject.FindGameObjectWithTag("PinkBar").GetComponent<Image>().fillAmount = ((pinkCounter % need) / (need / 100f)) / 100f;

            }


        }

    }

    private DLCType? getType(string s) {

        try
        {
            return (DLCType) Enum.Parse(typeof(DLCType), s);
        }
        catch (Exception ignored) { }

        return null;

    }

    private void activateBars() {

        if (data.blue != "") {
            DLCType? t = getType(data.blue);

            if (t != null) {
                Sprite s = Dot.typeToSprite(HeroType.BLUE, (DLCType)t);

                GameObject go = GameObject.FindGameObjectWithTag("Blue");
                Image img = go.GetComponent<Image>();
                img.color = new Color(1f, 1f, 1f, 0.4f);
                img.sprite = s;

                GameObject go2 = GameObject.FindGameObjectWithTag("BlueBar");
                Image img2 = go2.GetComponent<Image>();
                img2.color = new Color(1f, 1f, 1f, 1f);
                img2.sprite = s;

            }

        }

        if (data.green != "")
        {
            DLCType? t = getType(data.green);

            if (t != null)
            {
                Sprite s = Dot.typeToSprite(HeroType.GREEN, (DLCType)t);

                GameObject go = GameObject.FindGameObjectWithTag("Green");
                Image img = go.GetComponent<Image>();
                img.color = new Color(1f, 1f, 1f, 0.4f);
                img.sprite = s;

                GameObject go2 = GameObject.FindGameObjectWithTag("GreenBar");
                Image img2 = go2.GetComponent<Image>();
                img2.color = new Color(1f, 1f, 1f, 1f);
                img2.sprite = s;

            }

        }

        if (data.lightgreen != "")
        {
            DLCType? t = getType(data.lightgreen);

            if (t != null)
            {
                Sprite s = Dot.typeToSprite(HeroType.LIGHTGREEN, (DLCType)t);

                GameObject go = GameObject.FindGameObjectWithTag("LightGreen");
                Image img = go.GetComponent<Image>();
                img.color = new Color(1f, 1f, 1f, 0.4f);
                img.sprite = s;

                GameObject go2 = GameObject.FindGameObjectWithTag("LightGreenBar");
                Image img2 = go2.GetComponent<Image>();
                img2.color = new Color(1f, 1f, 1f, 1f);
                img2.sprite = s;

            }

        }

        if (data.orange != "")
        {
            DLCType? t = getType(data.orange);

            if (t != null)
            {
                Sprite s = Dot.typeToSprite(HeroType.ORANGE, (DLCType)t);

                GameObject go = GameObject.FindGameObjectWithTag("Orange");
                Image img = go.GetComponent<Image>();
                img.color = new Color(1f, 1f, 1f, 0.4f);
                img.sprite = s;

                GameObject go2 = GameObject.FindGameObjectWithTag("OrangeBar");
                Image img2 = go2.GetComponent<Image>();
                img2.color = new Color(1f, 1f, 1f, 1f);
                img2.sprite = s;

            }

        }

        if (data.pink != "")
        {
            DLCType? t = getType(data.pink);

            if (t != null)
            {
                Sprite s = Dot.typeToSprite(HeroType.PINK, (DLCType)t);

                GameObject go = GameObject.FindGameObjectWithTag("Pink");
                Image img = go.GetComponent<Image>();
                img.color = new Color(1f, 1f, 1f, 0.4f);
                img.sprite = s;

                GameObject go2 = GameObject.FindGameObjectWithTag("PinkBar");
                Image img2 = go2.GetComponent<Image>();
                img2.color = new Color(1f, 1f, 1f, 1f);
                img2.sprite = s;

            }

        }

    }

    private void Setup() {

        for (int x = 0; x < width; x++) {

            for (int y = 0; y < height; y++) {

                HeroType ht;

                if (x == 0 || x == width - 1 || y == 0) {

                    ht = HeroType.CONCRETE;

                } else ht = Dot.random();

                Vector3 temp = new Vector3(Dot.calculateX(x), Dot.calculateY(y), 10f);
                GameObject go = Instantiate(dot, temp, Quaternion.identity, GameObject.FindGameObjectWithTag("Board").transform);
                go.name = "(" + x + ";" + y + ")";
                dots[x, y] = new Dot(x, y, go, ht);

                if (x == 5 && y == 5) {

                    dots[5, 5].setDLCType(DLCType.ADJACENT);

                }

            }

        }

    }

    void FixedUpdate() {

        foreach (Circle c in circles) {

            c.rotate();

        }

    }

    private Dot isSet(int x, int y) {
        Dot d = null;

        try
        {

            d = dots[x, y];

        }
        catch (Exception ignored) { }

        return d;

    }

    private List<Dot> getVerticalDots(Dot b) {
        List<Dot> ds = new List<Dot>();

        for (int i = 1; i <= 2; i++) {
            Dot d = isSet(b.getX(), b.getY() + i);

            if (d != null && d.getType() != HeroType.NONE && d.getType() != HeroType.CONCRETE) {

                ds.Add(d);

            }

        }

        for (int i = 1; i <= 2; i++)
        {
            Dot d = isSet(b.getX(), b.getY() - i);

            if (d != null && d.getType() != HeroType.NONE && d.getType() != HeroType.CONCRETE)
            {

                ds.Add(d);

            }

        }

        return ds;

    }

    private List<Dot> getHorizontalDots(Dot b)
    {
        List<Dot> ds = new List<Dot>();

        for (int i = 1; i <= 2; i++)
        {
            Dot d = isSet(b.getX() + i, b.getY());

            if (d != null && d.getType() != HeroType.NONE && d.getType() != HeroType.CONCRETE)
            {

                ds.Add(d);

            }

        }

        for (int i = 1; i <= 2; i++)
        {
            Dot d = isSet(b.getX() - i, b.getY());

            if (d != null && d.getType() != HeroType.NONE && d.getType() != HeroType.CONCRETE)
            {

                ds.Add(d);

            }

        }

        return ds;

    }

    private List<Dot> getAreaDots(Dot b) {

        List<Dot> ds = new List<Dot>();

        for (int x = -1; x <= 1; x++) {

            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue;

                Dot d = isSet(b.getX() + x, b.getY() + y);

                if (d != null && d.getType() != HeroType.NONE && d.getType() != HeroType.CONCRETE)
                {

                    ds.Add(d);

                }

            }

        }

        return ds;

    }

    public void dlc(Dot d) {

        if (d.getDLCType() == DLCType.VERTICAL)
        {
            List<Dot> ds = getVerticalDots(d);

            foreach (Dot i in ds)
            {

                circles.Add(new Circle(i, d, true));

            }

        }else if (d.getDLCType() == DLCType.HORIZONTAL)
        {
            List<Dot> ds = getHorizontalDots(d);

            foreach (Dot i in ds)
            {

                circles.Add(new Circle(i, d, true));

            }

        }
        else if (d.getDLCType() == DLCType.BOMB)
        {
            List<Dot> ds = getAreaDots(d);

            foreach (Dot i in ds)
            {

                circles.Add(new Circle(i, d, true));

            }

        }
        else if (d.getDLCType() == DLCType.ADJACENT)
        {
            List<Dot> ds = getAreaDots(d);

            foreach (Dot i in ds)
            {

                i.setType(d.getType(), true);
                circles.Add(new Circle(i, d, false));

            }

        }

    }

    void OnMouseDown() {

        if (!selecting)
        {
            Dot d = Dot.has(GetMouseCameraPoint());
            if (d == null || d.getType() == HeroType.NONE || d.getType() == HeroType.CONCRETE) return;

            selected.Clear();
            selected.Add(d);
            d.playPulse();
            dlc(d);
            selecting = true;
            staying = true;
            fading();

            Debug.Log(d.getX() + ":" + d.getY() + " STARTED");

        }

    }

    public void removeAddictedCircles(Dot addict) {
        List<Circle> remove = new List<Circle>();

        foreach (Circle c in circles) {

            if (addict == c.getAddict()) {

                remove.Add(c);
                
            }

        }

        foreach (Circle c in remove) {

            if (addict.getDLCType() == DLCType.ADJACENT) {

                c.getDot().turnBack();

            }

            c.destroy();
            circles.Remove(c);

        }

    }

    void Update() {

        if (selecting) {
            Dot d = Dot.has(GetMouseCameraPoint());
            if (d == null) return;
            Dot last = selected[selected.Count - 1];

            if (d != last && staying){

                staying = false;

            }

            if (selected.Count >= 2 && d == selected[selected.Count - 2] && !staying) {

                selected.Remove(last);
                last.stopPulse();

                drawLines();

                if (selected.Count <= 0) {

                    selecting = false;

                }

                removeAddictedCircles(last);

                fading();

                Debug.Log(d.getX() + ":" + d.getY() + " BACKED");

            } else if (d != last && d.getType() == last.getType() && !selected.Contains(d) && Dot.inRadius(last, d)) {
                Debug.Log(d.getX() + ":" + d.getY() + " ADDED");

                selected.Add(d);
                d.playPulse();
                staying = true;

                dlc(d);

                drawLines();
                fading();

            }

        }

        regen();

    }

    void drawLines() {

        foreach (GameObject go in lines) {

            Destroy(go);

        }
        lines.Clear();

        if (selected.Count < 2) return;
        LineRenderer lr = line.GetComponent<LineRenderer>();

        for (int i = 0; i < selected.Count; i++) {

            if (i + 1 <= selected.Count - 1)
            {
                Vector3 v1 = Dot.getCenter(selected[i].getObject());
                v1.z -= 1f;
                Vector3 v2 = Dot.getCenter(selected[i + 1].getObject());
                v2.z -= 1f;

                lr.SetPosition(0, v1);
                lr.SetPosition(1, v2);
                lr.startWidth = lineWidth;
                lr.endWidth = lineWidth;

                lines.Add(GameObject.Instantiate(line, Vector2.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform));

            }
            else break;

        }

    }

    void OnMouseUp() {

        if (selecting)
        {

            foreach (Dot d in selected)
            {

                d.stopPulse();

            }

            removeCircles();
            circles.Clear();

            if (selected.Count >= 3)
            {

                if (selected[0].getType() == HeroType.BLUE)
                {

                    blueCounter += selected.Count;
                    checkBar(selected[0].getType());

                }
                else if (selected[0].getType() == HeroType.GREEN)
                {

                    greenCounter += selected.Count;
                    checkBar(selected[0].getType());

                }
                else if (selected[0].getType() == HeroType.LIGHTGREEN)
                {

                    lightgreenCounter += selected.Count;
                    checkBar(selected[0].getType());

                }
                else if (selected[0].getType() == HeroType.ORANGE)
                {

                    orangeCounter += selected.Count;
                    checkBar(selected[0].getType());

                }
                else if (selected[0].getType() == HeroType.PINK)
                {

                    pinkCounter += selected.Count;
                    checkBar(selected[0].getType());

                }

                foreach (Dot d in selected)
                {

                    d.stopPulse();
                    d.setType(HeroType.NONE, false);

                }

            }

            selected.Clear();
            selecting = false;

            drawLines();
            fading();

            Debug.Log("FINISH");

        }

    }

    private Vector2 GetMouseCameraPoint()
    {

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * depth;

    }

    private float every = 0.15f;
    private float time = 0f;

    private bool hasNone() {

        foreach (Dot d in dots) {

            if (d.getType() == HeroType.NONE)
            {

                return true;

            }

        }

        return false;

    }

    private void regen() {
        time += Time.deltaTime;

        if (time < every)
        {

           return;

        }
        else {

            time = 0f;

        }

        for (int x = 0; x < width; x++)
        {
            bool b = false;
            int wy = 0;

            for (int y = 0; y < height; y++)
            {
                Dot d = dots[x, y];

                if (d.getType() == HeroType.NONE)
                {

                    Debug.Log("" + d.getX() + ":" + d.getY() + " none");

                    b = true;
                    wy = y;

                }
                else if (b)
                {

                    dots[x, wy].setType(d.getType(), false);
                    dots[x, wy].setDLCType(d.getDLCType());
                    d.setType(HeroType.NONE, false);
                    wy += 1;

                }

            }

        }

        for (int x = 0; x < width; x++)
        {
            Dot d = dots[x, height - 1];

            if (d.getType() == HeroType.NONE)
            {

                HeroType r = Dot.random();
                d.setType(r, false);
                Debug.Log("random is " + d.getType());

                if (r == HeroType.BLUE && blueCounter >= need)
                {
                    DLCType? t = getType(data.blue);

                    if (t != null)
                    {

                        d.setDLCType((DLCType)t);
                        blueCounter -= need;

                    }

                }
                else if (r == HeroType.GREEN && greenCounter >= need)
                {
                    DLCType? t = getType(data.green);

                    if (t != null)
                    {

                        d.setDLCType((DLCType)t);
                        greenCounter -= need;

                    }

                }
                else if (r == HeroType.LIGHTGREEN && lightgreenCounter >= need)
                {
                    DLCType? t = getType(data.lightgreen);

                    if (t != null)
                    {

                        d.setDLCType((DLCType)t);
                        lightgreenCounter -= need;

                    }

                }
                else if (r == HeroType.ORANGE && orangeCounter >= need)
                {
                    DLCType? t = getType(data.orange);

                    if (t != null)
                    {

                        d.setDLCType((DLCType)t);
                        orangeCounter -= need;

                    }

                }
                else if (r == HeroType.PINK && pinkCounter >= need)
                {
                    DLCType? t = getType(data.pink);

                    if (t != null)
                    {

                        d.setDLCType((DLCType)t);
                        pinkCounter -= need;

                    }

                }

            }

        }

    }

    private void removeCircles() {

        foreach (Circle c in circles) {

            if (selected.Count >= 3) {
                Dot d = c.getDot();

                if (!selected.Contains(d) && (c.getAddict().getDLCType() == DLCType.HORIZONTAL || c.getAddict().getDLCType() == DLCType.BOMB || c.getAddict().getDLCType() == DLCType.VERTICAL)) {

                    if (d.getType() == HeroType.BLUE)
                    {

                        blueCounter += 1;
                        checkBar(HeroType.BLUE);

                    }
                    else if (d.getType() == HeroType.ORANGE)
                    {

                        orangeCounter += 1;
                        checkBar(HeroType.ORANGE);

                    }
                    else if (d.getType() == HeroType.GREEN)
                    {

                        greenCounter += 1;
                        checkBar(HeroType.GREEN);

                    }
                    else if (d.getType() == HeroType.LIGHTGREEN)
                    {

                        lightgreenCounter += 1;
                        checkBar(HeroType.LIGHTGREEN);

                    }
                    else if (d.getType() == HeroType.PINK)
                    {

                        pinkCounter += 1;
                        checkBar(HeroType.PINK);

                    }

                }

                if (d.getType() != HeroType.CONCRETE && (c.getAddict().getDLCType() == DLCType.HORIZONTAL || c.getAddict().getDLCType() == DLCType.BOMB || c.getAddict().getDLCType() == DLCType.VERTICAL)) {

                    d.setType(HeroType.NONE, false);

                }
            } else if (c.getAddict().getDLCType() == DLCType.ADJACENT) {

                c.getDot().turnBack();

            }

            c.destroy();

        }

    }

    private void fading() {

        HeroType? type = null;
        
        if (selecting) {

            type = selected[selected.Count - 1].getType();

        }

        foreach (Dot d in dots) {

            if ((type != null && d.getType() == type) || (type == null))
            {

                d.setNormal();

            }
            else d.setFaded();

        }

    }

}

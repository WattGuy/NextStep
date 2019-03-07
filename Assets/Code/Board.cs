using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public int _width;
    public int _height;
    public GameObject dot;

    public Sprite _cocos;
    public Sprite _apple;
    public Sprite _milk;
    public Sprite _star;
    public Sprite _cookie;

    public GameObject line;
    public Camera camera;
    public float depth = 5;

    public static Sprite cocos;
    public static Sprite apple;
    public static Sprite milk;
    public static Sprite star;
    public static Sprite cookie;

    public static int width;
    public static int height;

    public float lineWidth = 2;

    private SpriteRenderer sr;

    public static Dot[,] dots;

    public static bool selecting = false;
    public static List<Dot> selected = new List<Dot>();
    public static List<GameObject> lines = new List<GameObject>();
    public static bool staying = false;

    void Start () {
        selected = new List<Dot>();
        selecting = false;
        staying = false;

        cocos = _cocos;
        apple = _apple;
        milk = _milk;
        star = _star;
        cookie = _cookie;

        width = _width;
        height = _height;

        sr = dot.GetComponent<SpriteRenderer>() as SpriteRenderer;
        dots = new Dot[width, height];
        Setup();
	}

    private void Setup() {

        for (int x = 0; x < width; x++) {

            for (int y = 0; y < height; y++) {

                HeroType ht = Dot.random();
                sr.sprite = Dot.typeToSprite(ht);

                Vector3 temp = new Vector3(Dot.calculateX(x), Dot.calculateY(y), 10f);
                GameObject go = Instantiate(dot, temp, Quaternion.identity);
                dots[x, y] = new Dot(x, y, go, ht);

            }

        }

    }

    void OnMouseDown() {

        if (!selecting)
        {
            Dot d = Dot.has(GetMouseCameraPoint());
            if (d == null) return;

            selected.Clear();
            selected.Add(d);
            d.playPulse();
            selecting = true;
            staying = true;

            Debug.Log(d.getX() + ":" + d.getY() + " STARTED");

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

                Debug.Log(d.getX() + ":" + d.getY() + " BACKED");

            } else if (d != last && d.getType() == last.getType() && !selected.Contains(d) && Dot.inRadius(last, d)) {
                Debug.Log(d.getX() + ":" + d.getY() + " ADDED");

                selected.Add(d);
                d.playPulse();
                staying = true;

                drawLines();

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

            if (selected.Count >= 3) {

                foreach (Dot d in selected)
                {

                    d.stopPulse();
                    d.setType(HeroType.NONE);

                }

            }

            selected.Clear();
            selecting = false;

            drawLines();

            Debug.Log("FINISH");

        }

    }

    private Vector2 GetMouseCameraPoint()
    {

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * depth;

    }

    private void regen() {

        for (int x = 0; x < width; x++) {
            bool b = false;
            int wy = 0;

            for (int y = 0; y < height; y++) {
                Dot d = dots[x, y];

                if (d.getType() == HeroType.NONE) {

                    b = true;
                    wy = y;

                } else if (b) {

                    dots[x, wy].setType(d.getType());
                    wy += 1;

                }

            }

        }

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                Dot d = dots[x, y];

                if (d.getType() == HeroType.NONE)
                {

                    d.setType(Dot.random());

                }

            }

        }

    }

}

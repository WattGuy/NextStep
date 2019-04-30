using UnityEngine;

public class Dot
{

    private int x;
    private int y;
    private HeroType ht;
    private GameObject go;
    private SpriteRenderer sr;
    private BoxCollider2D collider;
    private Animation animation;
    private HeroType? cacheht = null;
    private DLCType? cachedt = null;
    private DLCType dt = DLCType.NONE;
    private OnType ot = OnType.NONE;
    private int oi = 0;
    private Vector3? point = null;

    public Dot(int x, int y, GameObject go, HeroType ht)
    {

        this.x = x;
        this.y = y;
        this.go = go;
        this.sr = go.GetComponent<SpriteRenderer>();
        this.collider = go.GetComponent<BoxCollider2D>();
        this.animation = go.GetComponent<Animation>();
        setType(ht, false);

    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public void setY(int y) {
        this.y = y;
    }

    public Vector3? getPoint()
    {
        return point;
    }

    public void setPoint(Vector3? v) {
        this.point = v;
    }

    public void destroy() {
        GameObject.Destroy(go);
    }

    public GameObject getObject()
    {

        return go;

    }

    public BoxCollider2D getCollider()
    {

        return collider;

    }

    public void setDLCType(DLCType dt) {

        this.dt = dt;
        sr.sprite = typeToSprite(ht, dt);

    }

    public DLCType getDLCType()
    {
        return dt;
    }

    public void setFaded() {

        sr.color = new Color(255f, 255f, 255f, 0.5f);

    }

    public void setNormal()
    {

        sr.color = new Color(255f, 255f, 255f, 1f);

    }

    public Animation getAnimation()
    {

        return animation;

    }

    public void playPulse()
    {

        animation.clip = animation.GetClip("Pulse");
        animation.Play();

    }

    public void stopPulse()
    {

        animation.Stop();
        go.transform.eulerAngles = new Vector3(0f, 0f, 0f);

    }

    public int getOnInt() {

        return oi;

    }

    public void setOnInt(int oi) {

       this.oi = oi;

    }

    public void setOType(OnType ot) {

        foreach (Transform child in go.transform) {

            if (child.name == ot.ToString().ToLower()) {

                child.gameObject.SetActive(true);

            } else if (child.name == this.ot.ToString().ToLower()) {

                child.gameObject.SetActive(false);

            }

        }

        this.oi = (int) ot;
        this.ot = ot;

    }

    public OnType getOType() {
        return ot;
    }

    public HeroType getType()
    {
        return ht;
    }

    public void turnBack() {

        if (cacheht != null) {

            setType((HeroType) cacheht, false);
            cacheht = null;

        }

        if (cachedt != null)
        {
            setDLCType((DLCType) cachedt);
            cachedt = null;

        }

    }

    public void setType(HeroType ht, bool adjacent)
    {

        sr.sprite = Dot.typeToSprite(ht, dt);
        if (adjacent)
        {
            this.cacheht = this.ht;
            this.cachedt = this.dt;
        }
        this.ht = ht;
        this.dt = DLCType.NONE;

    }

    public void select(bool b) {

        if (b)
        {

            go.GetComponent<SpriteRenderer>().material = Board.outline;

        }
        else {

            go.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Sprites/Default"));

        }

    }

    public static Sprite typeToSprite(HeroType ht, DLCType dt)
    {
        return Board.rs.sprites[new System.Collections.Generic.KeyValuePair<HeroType, DLCType>(ht, dt)];
    }

    public static Dot has(Vector2 click)
    {
        bool b = false;
        Dot d = null;

        for (int x = 0; x < Board.width; x++)
        {

            for (int y = 0; y < Board.height; y++)
            {

                if (Board.dots[x, y].getCollider().bounds.Contains(new Vector3(click.x, click.y, 10f)))
                {

                    d = Board.dots[x, y];
                    b = true;
                    break;

                }

            }

            if (b)
            {

                break;

            }

        }

        return d;

    }

    public static bool inRadius(Dot last, Dot d)
    {
        int x = Mathf.Abs(d.getX() - last.getX());
        int y = Mathf.Abs(d.getY() - last.getY());

        if (x <= 1 && y <= 1)
        {

            return true;

        }
        else return false;

    }

    public static float calculateX(int x)
    {

        return x + (x * 1.58f) - 9f;

    }

    public static float calculateY(int y)
    {

        return y + (y * 2.5f) - 16.75f;

    }

    public static Vector3 getCenter(GameObject go)
    {
        SpriteRenderer r = go.GetComponent<SpriteRenderer>();

        return r.bounds.center;

    }

    public static HeroType random()
    {

        int r = Random.Range(1, 100);
        if (r <= 20)
        {

            return HeroType.BLUE;

        }
        else if (r <= 40)
        {

            return HeroType.GREEN;

        }
        else if (r <= 60)
        {

            return HeroType.LIGHTGREEN;

        }
        else if (r <= 80)
        {

            return HeroType.PINK;

        }
        else
        {

            return HeroType.ORANGE;

        }

    }

}
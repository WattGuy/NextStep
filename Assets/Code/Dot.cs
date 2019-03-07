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

    public Dot(int x, int y, GameObject go, HeroType ht)
    {

        this.x = x;
        this.y = y;
        this.go = go;
        this.sr = go.GetComponent<SpriteRenderer>();
        this.collider = go.GetComponent<BoxCollider2D>();
        this.animation = go.GetComponent<Animation>();
        this.ht = ht;

    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public GameObject getObject()
    {

        return go;

    }

    public BoxCollider2D getCollider()
    {

        return collider;

    }

    public Animation getAnimation()
    {

        return animation;

    }

    public void playPulse()
    {

        animation.Play();

    }

    public void stopPulse()
    {

        animation.Stop();
        go.transform.localScale = new Vector3(1.2f, 1.2f, 1f);

    }

    public HeroType getType()
    {
        return ht;
    }

    public void setType(HeroType ht)
    {

        sr.sprite = Dot.typeToSprite(ht);
        this.ht = ht;
    }

    public static Sprite typeToSprite(HeroType ht)
    {

        if (ht == HeroType.APPLE)
        {

            return Board.apple;

        }
        else if (ht == HeroType.COCOS)
        {

            return Board.cocos;

        }
        else if (ht == HeroType.COOKIE)
        {

            return Board.cookie;

        }
        else if (ht == HeroType.MILK)
        {

            return Board.milk;

        }
        else if (ht == HeroType.STAR)
        {

            return Board.star;

        }
        else
        {

            return null;

        }

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

        return y + (y * 2.5f) - 13.75f;

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

            return HeroType.APPLE;

        }
        else if (r <= 40)
        {

            return HeroType.COCOS;

        }
        else if (r <= 60)
        {

            return HeroType.MILK;

        }
        else if (r <= 80)
        {

            return HeroType.COOKIE;

        }
        else
        {

            return HeroType.STAR;

        }

    }

}
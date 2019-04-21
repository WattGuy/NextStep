using System;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<HeroType, Counter> counters = null;
    public Dictionary<HeroType, Target> targets = null;
    public Dictionary<PopupType, Popup> popups = null;

    public static int width;
    public static int height;

    public GameObject _target;
    public static GameObject target;

    //public GameObject explosion;

    public static float y_difference;

    public float lineWidth = 2;

    public static int need = 20;

    private SpriteRenderer sr;

    private GridLayoutGroup glg;

    public static List<Dot> queue = new List<Dot>();
    public static int counter = 0;
    public static Vector3[,] positions;
    public static Dot[,] dots;

    public static bool selecting = false;
    public static List<Dot> selected = new List<Dot>();
    public static List<GameObject> lines = new List<GameObject>();
    public static bool staying = false;
    public List<Dot> selection = new List<Dot>();

    public static Board instance = null;
    public static bool paused = false;

    public Material _outline;
    public static Material outline;

    public static Resources rs = null;

    public int default_steps = 30;
    public StepsCounter steps = null;

    public static List<Dot> otstoi = new List<Dot>();

    void Start () {
        instance = this;
        paused = false;
        selected = new List<Dot>();
        selecting = false;
        staying = false;

        circle = _circle;
        target = _target;
        outline = _outline;

        if (rs == null) rs = new Resources();

        if (counters == null) {
            counters = new Dictionary<HeroType, Counter>();

            foreach (HeroType ht in Enum.GetValues(typeof(HeroType)).Cast<HeroType>())
            {

                if (ht != HeroType.NONE && ht != HeroType.CONCRETE)
                {

                    counters.Add(ht, new Counter(ht));

                }

            }

        }

        width = _width;
        height = _height;

        if (popups == null) {
            popups = new Dictionary<PopupType, Popup>();

            foreach (PopupType pt in Enum.GetValues(typeof(PopupType)).Cast<PopupType>())
            {

                popups.Add(pt, new Popup(pt));

            }

        }

        if (ContentManager.level != null)
        {
            targets = new Dictionary<HeroType, Target>();
            List<HeroType> was = new List<HeroType>();

            foreach (TargetData t in ContentManager.level.targets)
            {
                HeroType? ht = t.getType();
                if (ht == null) continue;

                if (was.Contains((HeroType)ht)) continue;
                targets.Add((HeroType)ht, new Target((HeroType)ht, t.target));
                was.Add((HeroType)ht);

            }

            steps = new StepsCounter(ContentManager.level.steps);

        }
        else {

            steps = new StepsCounter(default_steps);

        }

        glg = GameObject.FindGameObjectWithTag("Board").GetComponent<GridLayoutGroup>() as GridLayoutGroup;

        sr = dot.GetComponent<SpriteRenderer>() as SpriteRenderer;
        dots = new Dot[width, height];
        positions = new Vector3[width, height + 1];
        Setup();
    }

    private bool allTargetsIsCompleted() {

        if (targets != null)
        {

            foreach (Target t in targets.Values)
            {

                if (!t.isDone())
                {

                    return false;

                }

            }

        }
        else return false;

        return true;

    }

    private bool isLocked() {

        return allTargetsIsCompleted() || steps.isLocked() || paused;

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

            }

        }

        glg.CalculateLayoutInputHorizontal();
        glg.CalculateLayoutInputVertical();
        glg.SetLayoutHorizontal();
        glg.SetLayoutVertical();

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {

                positions[x, y] = new Vector3(dots[x, y].getObject().transform.position.x, dots[x, y].getObject().transform.position.y, dots[x, y].getObject().transform.position.z);

            }

        }

        y_difference = positions[0, height - 1].y - positions[0, height - 2].y;

        if (ContentManager.level == null) return;
        else {

            foreach (DotData d in ContentManager.level.dots) {
                string[] s = d.position.Split(':');
                if (s.Length < 2) continue;

                int? x = tryParse(s[0]);
                if (x == null) continue;

                int? y = tryParse(s[1]);
                if (y == null) continue;

                if (x > width - 1 || y > height - 1) continue;
                HeroType? ht = d.getType();
                if (ht == null) continue;

                dots[(int)x, (int)y].setType((HeroType) ht, false);

                DLCType? dt = d.getDType();
                if (dt == null) continue;

                dots[(int)x, (int)y].setDLCType((DLCType)dt);

            }

        }

    }

    private int? tryParse(string s)
    {

        try
        {

            return int.Parse(s);

        }
        catch (Exception e) { }

        return null;

    }

    void updateSelection() {
        List<Dot> remove = new List<Dot>();

        foreach (Dot d in selection)
        {

            if (!selected.Contains(d)) {

                d.select(false);
                remove.Add(d);

            }

        }

        foreach (Dot d in remove) {

            selection.Remove(d);

        }

        foreach (Dot d in selected)
        {

            if (!selection.Contains(d)) {

                d.select(true);
                selection.Add(d);

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

    public static List<Dot> activated = new List<Dot>();

    private bool hasUnactivated() {

        foreach (Circle c in circles) {

            if (c.getAddict().getDLCType() != DLCType.ADJACENT && c.getDot().getDLCType() != DLCType.NONE && c.getDot().getDLCType() != DLCType.ADJACENT && !activated.Contains(c.getDot())) {

                return true;

            }

        }

        return false;

    }

    public void updateCircles() {
        activated.Clear();
        List<Circle> remove = new List<Circle>();

        foreach (Circle c in circles) {

            if (c.getAddict().getDLCType() != DLCType.ADJACENT) {

                c.destroy();
                remove.Add(c);

            }

        }

        foreach(Circle c in remove){

            circles.Remove(c);

        }

        foreach (Dot d in selected) {

            if (d.getDLCType() != DLCType.NONE && d.getDLCType() != DLCType.ADJACENT) {

                dlc(d, selected[selected.Count - 1]);
                activated.Add(d);

            }

        }

        while (hasUnactivated()) {

            foreach (Circle c in circles)
            {

                if (c.getAddict().getDLCType() != DLCType.ADJACENT && c.getDot().getDLCType() != DLCType.NONE && c.getDot().getDLCType() != DLCType.ADJACENT && !activated.Contains(c.getDot()))
                {

                    dlc(c.getDot());
                    activated.Add(c.getDot());
                    break;

                }

            }

        }

    }

    public void dlc(Dot addict) {

        dlc(addict, null);

    }

    public void dlc(Dot addict, Dot d) {

        if (addict.getDLCType() == DLCType.VERTICAL)
        {
            List<Dot> ds;

            if (d != null) {

                ds = getVerticalDots(d);

            } else ds = getVerticalDots(addict);

            foreach (Dot i in ds)
            {

                circles.Add(new Circle(i, addict, true));

            }

        }else if (addict.getDLCType() == DLCType.HORIZONTAL)
        {
            List<Dot> ds;
            if (d != null)
            {

                ds = getHorizontalDots(d);

            }
            else ds = getHorizontalDots(addict);

            foreach (Dot i in ds)
            {

                circles.Add(new Circle(i, addict, true));

            }

        }
        else if (addict.getDLCType() == DLCType.BOMB)
        {
            List<Dot> ds;
            if (d != null)
            {

                ds = getAreaDots(d);

            }
            else ds = getAreaDots(addict);

            foreach (Dot i in ds)
            {

                circles.Add(new Circle(i, addict, true));

            }

        }
        else if (addict.getDLCType() == DLCType.ADJACENT)
        {
            List<Dot> ds;
            if (d != null)
            {

                ds = getAreaDots(d);

            }
            else ds = getAreaDots(addict);

            foreach (Dot i in ds)
            {

                i.setType(addict.getType(), true);
                circles.Add(new Circle(i, addict, false));

            }

        }

    }

    void OnMouseDown() {

        if (!selecting && !isLocked())
        {
            Dot d = Dot.has(GetMouseCameraPoint());
            if (d == null || d.getType() == HeroType.NONE || d.getType() == HeroType.CONCRETE) return;

            selected.Clear();
            selected.Add(d);
            d.playPulse();
            dlc(d);
            updateCircles();
            selecting = true;
            staying = true;
            fading();

            updateSelection();

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

            if (d != null) {

                Dot last = selected[selected.Count - 1];

                if (d != last && staying)
                {

                    staying = false;

                }

                if (selected.Count >= 2 && d == selected[selected.Count - 2] && !staying)
                {

                    selected.Remove(last);
                    last.stopPulse();

                    drawLines();

                    if (selected.Count <= 0)
                    {

                        selecting = false;

                    }

                    if (last.getDLCType() == DLCType.ADJACENT) {

                        removeAddictedCircles(last);

                    }else updateCircles();

                    fading();

                    updateSelection();

                    Debug.Log(d.getX() + ":" + d.getY() + " BACKED");

                }
                else if (d != last && d.getType() == last.getType() && !selected.Contains(d) && Dot.inRadius(last, d))
                {
                    Debug.Log(d.getX() + ":" + d.getY() + " ADDED");

                    selected.Add(d);
                    d.playPulse();
                    staying = true;

                    dlc(d);
                    updateCircles();

                    drawLines();
                    fading();
                    updateSelection();

                }

            }

        }

        regen();

        if (queue.Count > 0)
        {
            List<Dot> remove = new List<Dot>();

            foreach (Dot d in queue)
            {
                Vector3? down = d.getPoint();

                if (down == null || d.getObject() == null) {

                    remove.Add(d);
                    continue;

                }

                if (Vector3.Distance(d.getObject().transform.position, (Vector3)down).ToString("0.00") == "0.00") {

                    remove.Add(d);
                    continue;

                }

                d.getObject().transform.position = Vector3.MoveTowards(d.getObject().transform.position, (Vector3) down, 20f * Time.deltaTime);

            }

            foreach (Dot d in remove) {

                queue.Remove(d);

            }

        }

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

    private void checkForComplete() {

        if (allTargetsIsCompleted()) {

            GameData d;
            if (ContentManager.data == null)
            {

                d = new GameData();

            }
            else d = ContentManager.data;

            if (ContentManager.id != 0 && ContentManager.id > d.last_level) {

                d.last_level = ContentManager.id;
                Saver.save(d);

            }

            popups[PopupType.WIN].activate();

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

                if (counters != null && counters.ContainsKey(selected[0].getType())) {

                    Counter c = counters[selected[0].getType()];
                    c.counter += selected.Count;
                    c.update();

                }

                if (targets != null && targets.ContainsKey(selected[0].getType())) {

                    Target t = targets[selected[0].getType()];
                    t.minus(selected.Count);
                    t.update();
                    checkForComplete();

                }

                foreach (Dot d in selected)
                {

                    d.stopPulse();
                    d.setType(HeroType.NONE, false);

                }

                steps.minus(1);
                steps.update();
                checkForLose();

            }

            selected.Clear();
            selecting = false;

            drawLines();
            fading();
            updateSelection();
            Dictionary<HeroType, List<Dot>> dict = getDots();

            foreach (Counter c in counters.Values)
            {

                if (dict.ContainsKey(c.getHero()))
                {

                    c.check(dict[c.getHero()]);

                }
                else
                {

                    c.check(new List<Dot>());

                }

            }

            Debug.Log("FINISH");

        }

    }

    private Vector2 GetMouseCameraPoint()
    {

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * depth;

    }

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

        for (int x = 0; x < width; x++)
        {
            bool b = false;
            int wy = 0;
            Vector3? point = null;

            for (int y = 0; y < height; y++)
            {
                Dot d = dots[x, y];

                if (!otstoi.Contains(d))
                {

                    if (!b && d.getType() == HeroType.NONE)
                    {

                        b = true;
                        wy = y;

                    }
                    else if (b && d.getType() != HeroType.NONE)
                    {
                        Dot d2 = dots[x, wy];

                        d.setY(wy);
                        d.getObject().name = "(" + d.getX() + ";" + d.getY() + ")";
                        d.setPoint(positions[x, wy]);
                        dots[x, y] = d2;
                        d2.setY(y);
                        d2.getObject().transform.position = positions[x, y];
                        d2.getObject().name = "(" + d2.getX() + ";" + d2.getY() + ")";
                        dots[x, wy] = d;

                        queue.Add(d);

                        wy += 1;

                    }

                }

            }

        }

        bool b2 = false;
        for (int x = 0; x < width; x++)
        {
            float i = 1;

            for (int y = 0; y < height; y++) {
                Dot d = dots[x, y];
                if (d.getType() != HeroType.NONE) continue;

                HeroType r = Dot.random();
                d.setType(r, false);
                d.getObject().transform.position = new Vector3(positions[x, height - 1].x, positions[x, height - 1].y + (y_difference * i), positions[x, height - 1].z);
                d.setPoint(positions[x, y]);
                i++;
                queue.Add(d);
                b2 = true;

            }

        }

        if (!b2) return; 
        Dictionary<HeroType, List<Dot>> dict = getDots();

        foreach (Counter c in counters.Values)
        {

            if (dict.ContainsKey(c.getHero()))
            {

                c.check(dict[c.getHero()]);

            }
            else
            {

                c.check(new List<Dot>());

            }

        }

    }

    private void removeCircles() {

        foreach (Circle c in circles) {

            if (selected.Count >= 3) {
                Dot d = c.getDot();

                if (!selected.Contains(d) && (c.getAddict().getDLCType() == DLCType.HORIZONTAL || c.getAddict().getDLCType() == DLCType.BOMB || c.getAddict().getDLCType() == DLCType.VERTICAL)) {

                    if (counters != null && counters.ContainsKey(d.getType())) {

                        Counter counter = counters[d.getType()];
                        counter.counter++;
                        counter.update();

                    }

                    if (targets != null && targets.ContainsKey(d.getType()))
                    {

                        Target t = targets[d.getType()];
                        t.minus(1);
                        t.update();
                        checkForComplete();

                    }

                }

                d.setType(HeroType.NONE, false);
            } else if (c.getAddict().getDLCType() == DLCType.ADJACENT) {

                c.getDot().turnBack();

            }

            c.destroy();

        }

    }

    private void checkForLose() {

        if (steps.isLocked()) {

            popups[PopupType.LOSE].activate();

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

    private Dictionary<HeroType, List<Dot>> getDots() {
        Dictionary<HeroType, List<Dot>> dict = new Dictionary<HeroType, List<Dot>>();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = height - 1; y > 0; y--) {
                Dot d = dots[x, y];

                if (d.getType() == HeroType.NONE || d.getType() == HeroType.CONCRETE || d.getDLCType() != DLCType.NONE) continue;

                List<Dot> l;
                if (dict.ContainsKey(d.getType())) l = dict[d.getType()];
                else {

                    l = new List<Dot>();
                    dict.Add(d.getType(), l);

                }

                l.Add(d);

            }

        }

        return dict;

    }

}

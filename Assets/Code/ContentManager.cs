using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    [Header("Controllers")]
    public int panCount;
    public int panOffset;
    public float snapSpeed;
    public float scaleOffset;
    public float scaleSpeed;
    [Header("Other Objects")]
    public GameObject panPrefab;
    public ScrollRect scrollRect;

    private GameObject[] instPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;

    private RectTransform contentRect;
    private Vector2 contentVector;
    private int priority;
    private bool good = false;

    private int selectedPanID;
    private bool isScrolling;
    public int plusy = 50;
    public GameObject button;

    public static GameData data = null;
    public static int id = 0;
    public static Popup levelPopup = null;
    public GameObject _targetPopup;
    public static GameObject targetPopup;

    private int getNumber(int i) {

        return i / 15;

    }

    private void Start()
    {
        if (levels.Count > 0)
        {

            levels.Clear();

        }
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];
        levelPopup = new Popup(PopupType.LEVEL);
        targetPopup = _targetPopup;

        if (Board.rs == null) Board.rs = new Resources();

        if (data == null) data = Saver.load();
        priority = getNumber(data.last_level + 1);

        int i = 0;
        int hi = 1;

        while (true)
        {
            string s = "" + hi;

            if (s.Length == 1)
            {

                s = "0" + s;

            }

            TextAsset asset = getText(s);

            if (asset != null)
            {
                int h = getNumber(i);
                if (hi % 15 == 1) {

                    instPans[h] = Instantiate(panPrefab, transform, false);
                    if (i == 0) { instPans[h].transform.localPosition = new Vector2(instPans[h].transform.localPosition.x, instPans[h].transform.localPosition.y + plusy); }
                    else{
                        instPans[h].transform.localPosition = new Vector2(instPans[h - 1].transform.localPosition.x + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset,
                            instPans[h].transform.localPosition.y + plusy);
                        pansPos[h] = -instPans[h].transform.localPosition;
                    }

                }

                levels.Add(hi, asset);
                GameObject go = Instantiate(button, Vector3.zero, Quaternion.identity, instPans[h].transform);
                foreach (Transform child in go.transform)
                {

                    if (child.GetComponent<Text>() != null)
                    {

                        Text t = child.GetComponent<Text>();
                        if (hi > data.last_level + 1)
                        {
                            child.gameObject.SetActive(false);
                        }

                        t.text = s;

                    }
                    else {

                        if (hi > data.last_level + 1) child.gameObject.SetActive(true);

                    }

                    

                }

            }
            else break;

            i++;
            hi++;
        }

        contentRect.anchoredPosition = new Vector2(pansPos[priority].x, contentRect.anchoredPosition.y);

    }

    private void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling)
            scrollRect.inertia = false;
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
   
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }

    public static Dictionary<int, TextAsset> levels = new Dictionary<int, TextAsset>();

    public static LevelData level = null;

    private TextAsset getText(string s)
    {

        try
        {

            return UnityEngine.Resources.Load<TextAsset>("Levels/" + s);

        }
        catch (Exception e) { }

        return null;

    }

}

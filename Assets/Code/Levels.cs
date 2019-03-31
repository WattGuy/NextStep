using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour {

    public GameObject button;

    public static Dictionary<int, TextAsset> levels = new Dictionary<int, TextAsset>();

    public static LevelData level = null;

    void Start() {

        if (levels.Count > 0) {

            levels.Clear();

        }

        int i = 1;

        while (true) {
            if (i > 9) break;
            string s = "" + i;

            if (s.Length == 1)
            {

                s = "0" + s;

            }

            TextAsset asset = getText(s);

            if (asset != null)
            {

                levels.Add(i, asset);
                GameObject go = Instantiate(button, Vector3.zero, Quaternion.identity, transform);
                foreach (Transform child in go.transform) {

                    Text t = child.GetComponent<Text>();
                    t.text = s;

                }

            }
            else break;

            i++;
        }

    }

    private TextAsset getText(string s) {

        try
        {

            return UnityEngine.Resources.Load<TextAsset>("Levels/" + s);

        }
        catch (Exception e) { }

        return null;

    }

}

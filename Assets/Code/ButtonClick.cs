using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {

        if (tag == "LevelButton") {
            Text t = transform.GetChild(0).GetComponent<Text>();
            int? i = tryParse(t.text);

            if (i == null) return;

            TextAsset ass = Levels.levels[(int)i];
            LevelData d = Saver.loadLevel(ass.text);

            if (d == null) return;

            Levels.level = d;
            SceneManager.LoadScene("game");

        }
        else if ("Settings" == name)
        {



        }
        else if ("About" == name)
        {



        }
        else if ("Store" == name)
        {



        } else if ("Menu" == name) {

            SceneManager.LoadScene("menu");

        }
        else if ("Levels" == name || "Quit" == name)
        {

            SceneManager.LoadScene("levels");

        }
        else if ("Restart" == name)
        {

            SceneManager.LoadScene("game");

        }
        else if ("Pause" == name)
        {

            Board.instance.popups[PopupType.PAUSE].activate();
            Board.paused = true;

        }
        else if ("Continue" == name) {

            Board.instance.popups[PopupType.PAUSE].unactivate();
            Board.paused = false;

        }


    }

    private int? tryParse(string s) {

        try {

            return int.Parse(s);

        }
        catch (Exception e) {}

        return null;

    }

}

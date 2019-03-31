using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsCounter {

    private int left;
    private Text text = null;

    public StepsCounter(int steps) {

        this.left = steps;
        text = GameObject.FindGameObjectWithTag("StepsCounter").GetComponent<Text>();
        update();

    }

    public void update() {

        text.text = left.ToString();

    }

    public void minus(int i) {

        if (i >= left)
        {

            left = 0;

        }
        else left -= i;

    }

    public bool isLocked() {

        return left <= 0;

    }

    public int getSteps() {
        return left;
    }

}

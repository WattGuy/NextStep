using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonColorText : UnityEngine.UI.Button
{

    Text text;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        Color color;
        switch (state)
        {
            case Selectable.SelectionState.Normal:
                color = this.colors.normalColor;
                break;
            case Selectable.SelectionState.Highlighted:
                color = this.colors.highlightedColor;
                break;
            case Selectable.SelectionState.Pressed:
                color = this.colors.pressedColor;
                break;
            case Selectable.SelectionState.Disabled:
                color = this.colors.disabledColor;
                break;
            default:
                color = Color.white;
                break;
        }
        if (base.gameObject.activeInHierarchy)
        {
            switch (this.transition)
            {
                case Selectable.Transition.ColorTint:
                    ColorTween(color * this.colors.colorMultiplier, instant);
                    break;
            }
        }
    }

    private void ColorTween(Color targetColor, bool instant)
    {

        base.image.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);

        foreach (Transform child in gameObject.transform) {

            if (child.GetComponent<Text>() != null) {
                Text text = child.GetComponent<Text>();

                text.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);

            } else if (child.GetComponent<Image>() != null) {

                Image text = child.GetComponent<Image>();

                text.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);

            }

        }

    }
}
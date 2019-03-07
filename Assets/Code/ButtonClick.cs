using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour, IPointerClickHandler{

    public static Dot last;
    public static bool selecting = false;
    public GameObject prefab;
    public Camera cam;

    public static Vector2 first;

    void OnMouseDown() {

        if ("Board" == name) {

            Vector3 wp = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            first = touchPos;



        }

    }

    void Update() {



    }

    void OnMouseUp() {

        if ("Board" == name)
        {

            //selecting = false;
            //Debug.Log("BOARD UP");

        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if ("Play" == name) {

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

    }

}

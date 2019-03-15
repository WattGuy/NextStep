using UnityEngine;

public class Circle {

    private Dot d;
    private Dot addict;
    private GameObject go;
    private float z = 0f;

    public Circle(Dot d, Dot addict, bool on) {

        this.d = d;
        this.addict = addict;

        if (on) go = GameObject.Instantiate(Board.circle, Dot.getCenter(d.getObject()), Quaternion.identity, d.getObject().transform);

    }

    public void destroy() {

        if (go != null) {

            Object.Destroy(go);
            go = null;

        } 

    }

    public void rotate() {

        if (go != null) {

            z += 10f;
            go.transform.Rotate(0, 0, Time.deltaTime * 50, Space.Self);

        }

    }

    public Dot getAddict() {
        return addict;
    }

    public Dot getDot() {
        return d;
    }
	
}

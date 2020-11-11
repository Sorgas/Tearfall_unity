using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {
    struct ButtonData {
        public string name;
        public KeyCode hotKey;
        public int method;
    }

    public GameObject toCreate;
    public GameObject canvas;
    public int qwer;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("qwer");
    }

    // Update is called once per frame
    void Update() {
        GameObject createdObject = Instantiate(toCreate, new Vector3(Random.Range(0f, 1000), Random.Range(0f, 1000), 0), new Quaternion());
        createdObject.transform.SetParent(canvas.transform);
        Debug.Log("qwer");
    }
}

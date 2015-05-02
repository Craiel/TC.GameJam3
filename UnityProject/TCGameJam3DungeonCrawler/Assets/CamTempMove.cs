using UnityEngine;
using System.Collections;

public class CamTempMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
	    var mult = 0.5f;
        if (Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(xAxisValue * mult, 0.0f, zAxisValue * mult));
        }
	}
}

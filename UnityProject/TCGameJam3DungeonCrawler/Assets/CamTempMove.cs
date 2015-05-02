using UnityEngine;
using System.Collections;

public class CamTempMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float xAxisValue = Input.GetAxis("Horizontal");
        float yAxisValue = Input.GetAxis("Vertical");
        float zAxisValue = Input.GetAxis("zAxis");
	    var mult = 0.5f;
        if (Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(xAxisValue * mult, yAxisValue * mult, zAxisValue * mult));
        }
	}
}

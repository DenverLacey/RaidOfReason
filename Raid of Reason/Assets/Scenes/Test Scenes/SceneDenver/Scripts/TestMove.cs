using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float movX = Input.GetAxis("Horizontal");
		float movZ = Input.GetAxis("Vertical");

		Vector3 translation = new Vector3(movX, 0, movZ) * 5.0f;

		transform.Translate(translation * Time.deltaTime);
    }
}

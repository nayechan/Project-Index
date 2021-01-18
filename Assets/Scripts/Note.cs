using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static float speed = 12.5f;

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.Translate(0, speed * -Time.deltaTime, 0);
        Debug.Log("Note Update called");
    }
}

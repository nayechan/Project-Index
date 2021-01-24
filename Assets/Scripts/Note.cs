using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float beat;
    private int key;

    public Note()
	{

	}

    public Note(float beat, int key)
	{

	}

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        //transform.Translate(0, speed * -Time.deltaTime, 0);
    }
}

//TODO 노트 컨트롤러 클래스 추가, 그 클래스에 노트 움직이는 메소드도 옮기기
using System.Collections;
using UnityEngine;

public class HologramScript : MonoBehaviour {

    private const string Url = "10.34.85.21:8080/data.txt";
    private const int PingsPerSecond = 2;

    private float data = 1.0f; // Starting data

    // Use this for initialization
    void Start () {
        InvokeRepeating("QueryServer", 0.0f, 1.0f / PingsPerSecond);
        iTween.Init(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        //print(data);
        Pulse(gameObject, data);
    }

    void QueryServer()
    {
        WWW www = new WWW(Url);
        StartCoroutine(WaitForAPIResponse(www));
    }

    // Handles the server responses
    IEnumerator WaitForAPIResponse(WWW www)
    {
        yield return www;
        string text = www.text;
        if (text != null && text != "") data = float.Parse(text);
    }

    void Pulse(GameObject gameObject, float time)
    {
        //Hashtable hash = new Hashtable();
        //hash.Add("amount", new Vector3(0.05f, 0.05f, 0.05f));
        //hash.Add("time", time);
        iTween.PunchScale(gameObject, new Vector3(0.05f, 0.05f, 0.05f), time);
    }
}

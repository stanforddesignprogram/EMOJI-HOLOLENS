using System.Collections;
using UnityEngine;

public class HologramScript : MonoBehaviour {

    [System.Serializable]
    private class UpdateData
    {
        public float pulseDuration;
        public float opacity;
        public float fadeDuration;
    }

    private const string Url = "10.34.85.21:8080/data.txt";
    private const int PingsPerSecond = 2;

    [SerializeField]
    private UpdateData updateData;


    // Use this for initialization
    void Start () {
        InvokeRepeating("QueryServer", 0.0f, 1.0f / PingsPerSecond);
        iTween.Init(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        Pulse(updateData.pulseDuration);
        Fade(updateData.opacity, updateData.fadeDuration);
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
        if (text != null && text != "")
           updateData = JsonUtility.FromJson<UpdateData>(text);
    }

    void Pulse(float duration)
    {
        //Hashtable hash = new Hashtable();
        //hash.Add("amount", new Vector3(0.05f, 0.05f, 0.05f));
        //hash.Add("time", time);
        iTween.PunchScale(gameObject, new Vector3(0.05f, 0.05f, 0.05f), duration);
    }

    void Fade(float alpha, float duration)
    {
        iTween.FadeUpdate(gameObject, alpha, duration);
    }
}

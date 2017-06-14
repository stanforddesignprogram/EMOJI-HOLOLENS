using UnityEngine;

public class DebugTextScript : MonoBehaviour
{
    public Transform headTransform;
    public Vector3 headOffset;
    private TextMesh textMesh;

    void Start()
    {
        textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = "Anchors have been reset. Please close this application and restart it!";
    }

    void Update()
    {
        // Put the object in a static position in the scene relative to the user.  
        // Generally this wouldn't be a good idea for an application, but this is handy for
        // debugging.

        transform.position = headTransform.position +
            headTransform.forward * headOffset.z +
            headTransform.right * headOffset.x +
            headTransform.up * headOffset.y;

        transform.LookAt(headTransform);
        transform.Rotate(0, 180, 0);
    }
}

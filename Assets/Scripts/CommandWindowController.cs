using HoloToolkit.Unity;
using UnityEngine;

public class CommandWindowController : MonoBehaviour
{
    public GameObject debugText;
    private WorldAnchorManager anchorManager;

    // Use this for initialization
    void Start()
    {
        debugText.SetActive(false);
        anchorManager = WorldAnchorManager.Instance;
        if (anchorManager == null)
        {
            Debug.LogError("This script expects that you have a WorldAnchorManager component in your scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("u")) // Virtual Input -> Hololens for some reason doesn't recognize KeyCode.U
        {
            anchorManager.RemoveAllAnchors();
            debugText.SetActive(true);
        }
    }
}

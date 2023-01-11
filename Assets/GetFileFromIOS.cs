using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFileFromIOS : MonoBehaviour
{
	public Text fileURL;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void getFileURLFromApp()
	{
        SwiftForUnity.getStepData(3);      //This will invoke The picker inIOS
        //fileURL.text = Path;
	}

    public void OnRecieveFileURL(string url) {
        fileURL.text = url;
    }
}

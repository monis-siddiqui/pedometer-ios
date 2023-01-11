using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepDataHandler : MonoBehaviour
{
    public Text[] stepCountArray;
    public int[] stepCountData;

    // Start is called before the first frame update
    void Start()
    {
        stepCountData = new int[7];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fillStepsData() {
        Debug.Log("Called");
        if (Application.platform == RuntimePlatform.Android)
        {
           string stepCount = AndroidBridge.getStepCount();
            stepCountArray[0].text = stepCount;
        }
        else { 
            for (int i = 0; i < stepCountArray.Length; i++) {
            SwiftForUnity.getStepData(i);
        }
        }
    }

    public void OnRecieveStepCountData(string data) {
        
        string[] token = data.Split('-');
        int index = int.Parse( token[0]);
        int stepCount = int.Parse(token[1]);
        if (index < 7){
            stepCountData[index] = stepCount;
            stepCountArray[index].text = "" + stepCount;
        }

    }
}

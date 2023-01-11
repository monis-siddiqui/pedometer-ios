using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidStepCounter : MonoBehaviour
{
    private AndroidJavaObject plugin;
    public Text stepCount;

    void Start()
    {
#if UNITY_ANDROID
                plugin = new AndroidJavaClass("jp.kshoji.unity.sensor.UnitySensorPlugin").CallStatic<AndroidJavaObject>("getInstance");
                if (plugin != null) {
                    plugin.Call("startSensorListening", "stepcounter");
        }
#endif
    }

    void OnApplicationQuit()
    {
#if UNITY_ANDROID
		if (plugin != null) {
			plugin.Call("terminate");
			plugin = null;
		}
#endif
    }

    private void Update()
    {
#if UNITY_ANDROID
	if (plugin != null) {
		float[] sensorValue = plugin.Call<float[]>("getSensorValues", "stepcounter");
            stepCount.text = string.Join(",", new List<float>(sensorValue).ConvertAll(i => i.ToString()).ToArray());
        }
#endif
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AndroidBridge : MonoBehaviour
{
    public Text text;
    private static AndroidBridge _instance;
    public static AndroidBridge Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("AndroidBridge");
                _instance = obj.AddComponent<AndroidBridge>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    const string pluginName = "com.unityplugin.stepcountplugin.AndroidBridge";

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluinInstance;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(pluginName);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluinInstance == null)
            {
                _pluinInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            return _pluinInstance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ////text.text = getStepCount();
        //if (!StepCounter.current.enabled) {
        //    Debug.Log("Wasnt Enabled, Enabling now");
        //    InputSystem.EnableDevice(StepCounter.current);
        //}
        //// = StepCounter.current.stepCounter;
        //text.text = StepCounter.current.stepCounter.ReadValue()+ "";

        //Create Notification Channel on App
        if (Application.platform == RuntimePlatform.Android)
        {
            var plugin = new AndroidJavaClass(pluginName);
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            object[] args = new object[1];
            args[0] = context;
            //stepCount = PluginInstance.Call<string>("_getStepCount", args);
             string reply =PluginInstance.Call<string>("createNotificationChannel", args);
            Debug.Log("Reply from JNI: "+reply );
        }
    }

    // Update is called once per frame
    void Update()
    {
       // text.text = getStepCount();
    }

    public static string getStepCount() {
        string stepCount="Not Supported";
        if (Application.platform == RuntimePlatform.Android)
            {
                var plugin = new AndroidJavaClass(pluginName);
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                object[] args = new object[1];
                args[0] = context;
                //args[1] = activity;
            //stepCount = PluginInstance.Call<string>("_getStepCount", args);
                stepCount = PluginInstance.Call<string>("startService", args);
        }
        return stepCount;
    }


    public void OnRecieveData(string data) {
        Debug.Log("Value Changed");
        text.text = data;
    }
}

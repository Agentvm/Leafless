using System.Collections;
using UnityEngine;

public class OutputLogOnScreen : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue ();


    void OnEnable ()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable ()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog (string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n " + myLog;
        myLogQueue.Enqueue (newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue (newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
        if (myLogQueue.Count > 12)
            myLogQueue.Dequeue ();
    }

    void OnGUI ()
    {
        GUILayout.Label (myLog);
    }
}

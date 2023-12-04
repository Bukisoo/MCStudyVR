using UnityEngine;
using TMPro;
using System.Collections.Generic; // Needed for Queue


public class VRDebugConsole : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private Queue<string> logMessages = new Queue<string>();
    private const int maxLogCount = 10;

private string logString; // Define logString here
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

void HandleLog(string logString, string stackTrace, LogType type)
{
    // Check if the log message contains "MissingReferenceException:"
    if (!logString.Contains("MissingReferenceException:"))
    {
        this.logString = logString + "\n" + this.logString;

        if (this.logString.Length > 600) // Example limit
        {
            this.logString = this.logString.Substring(0, 600);
        }

        if (debugText != null)
        {
            debugText.text = this.logString;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI reference is missing");
        }
    }
}

}

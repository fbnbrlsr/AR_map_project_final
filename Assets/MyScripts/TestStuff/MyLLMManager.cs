using System.IO;
using UnityEngine;

public class MyLLMManager
{
    
    private string modelPath;
    public MyLLMManager()
    {   
        //Debug.Log("MyLLMManager started");
    }

    private void RunPythonScript()
    {
        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
        start.FileName = "/Users/fabian-mac/Desktop/Job_ETH/VisionOS_Unity_Projects/Mapbox_active_project/Assets/MyScripts/TestStuff/llmServer.py";
        start.UseShellExecute = true;
        start.RedirectStandardOutput = false;
        System.Diagnostics.Process.Start(start);
    }

    public string HandlePrompt(string prompt)
    {   
        // TODO: implement LLM inference given the prompt
        return "(LLM inference not implemented) " + prompt;
    }



}


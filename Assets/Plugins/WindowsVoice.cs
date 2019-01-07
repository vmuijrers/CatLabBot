using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
public class WindowsVoice : MonoBehaviour {
  [DllImport("WindowsVoice")]
  public static extern void initSpeech();
  [DllImport("WindowsVoice")]
  public static extern void destroySpeech();
  [DllImport("WindowsVoice")]
  public static extern void addToSpeechQueue(string s);
  [DllImport("WindowsVoice")]
  public static extern void clearSpeechQueue();
  [DllImport("WindowsVoice")]
  public static extern void statusMessage(StringBuilder str, int length);
  public static WindowsVoice instance = null;
    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
            Debug.Log("Initializing speech");
            initSpeech();
        }
    }
    public void Speak(string msg, float delay = 0f) {
    if ( delay == 0f )
        addToSpeechQueue(msg);
    else
        StartCoroutine(instance.ExecuteLater(delay, () => Speak(msg)));
    }
    void OnDestroy()
    {
    if (instance == this)
    {
        Debug.Log("Destroying speech");
        destroySpeech();
        instance = null;
    }
    }
    public string GetStatusMessage()
    {
        StringBuilder sb = new StringBuilder(40);
        statusMessage(sb, 40);
        return sb.ToString();
    }

    public IEnumerator ExecuteLater(float delay, System.Action action)
    {

        yield return new WaitForSeconds(delay);
        if(action != null)
        {
            action();
        }
    }
}
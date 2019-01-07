using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Windows.Speech;

using System.IO;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;
public class TextToSpeech : MonoBehaviour {
    public Text textComponent;
    public float timeBetweenNames;
    private string[] allNames;
    private int currentIndex = 0;
    private WindowsVoice voice;
    private TextAsset textAsset;
    public TextMeshProUGUI textMeshPro;

    public string filePath;
    private bool skip = false;
    private float currentTime = 0;
    private Dictionary<string, AudioClip> allClips = new Dictionary<string, AudioClip>();
    private AudioSource aSource;
    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    //private Queue<string> audioQueue = new Queue<string>();
    private int seed = 1234;
    // Use this for initialization
    void Start() {
        Random.InitState(seed);
        aSource = GetComponent<AudioSource>();
        AudioClip[] allAudio = Resources.LoadAll<AudioClip>("AudioFiles");
        foreach (AudioClip ac in allAudio)
        {
            allClips.Add(ac.name, ac);
        }
        filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "NameList.txt");
        textAsset = Resources.Load<TextAsset>("NameList");
        voice = WindowsVoice.instance;
        allNames = textAsset.text.Split('\n');
        System.Random rnd = new System.Random(seed);
        allNames = allNames.OrderBy(x => rnd.Next()).ToArray();
        StartCoroutine(SayNames());
    }

    void Update()
    {
        SpeakAudio();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skip = true;
        }
    }

    IEnumerator SayNames()
    {
        string intro = "Hello, Welcome to CatLab";
        SayAndShowText(intro);
        yield return new WaitForSeconds(4f);
        intro = "Let's all introduce ourselves!";
        SayAndShowText(intro);
        yield return new WaitForSeconds(4f);
        intro = "I am CatLabBot!";
        SayAndShowText(intro);
        yield return new WaitForSeconds(3f);
        intro = "I like cats and talking!";
        SayAndShowText(intro);
        yield return new WaitForSeconds(3f);
        intro = "The next introduction is from";
        SayAndShowText(intro);
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < allNames.Length; i++)
        {
            SayAndShowText(allNames[i]);
            while(currentTime < timeBetweenNames && !skip)
            {
                yield return null;
                currentTime += Time.deltaTime;
            }
            currentTime = 0;
            skip = false;
            string response = Util.Choose<string>("interesting!", "awesome!", "great!", "fantastic!", "cool!");
            intro = "Thank you! That was " + response;
            SayAndShowText(intro);
            yield return new WaitForSeconds(3f);
            intro = "The next introduction is from";
            SayAndShowText(intro);
            yield return new WaitForSeconds(4f);
        }
        intro = "Thank you all for the wonderful introductions,\n have fun and goodbye!";
        SayAndShowText(intro);
        yield return new WaitForSeconds(6f);
        intro = "You can all start speeddating now with your fellow students!";
        SayAndShowText(intro);

    }

    void SayAndShowText(string text)
    {
        AddAudio(text);
        textComponent.text = text;
        textMeshPro.text = text;
    }

    void SpeakAudio()
    {
        if (!aSource.isPlaying && audioQueue.Count > 0)
        {
            //string sentence = audioQueue.Dequeue();
            //voice.Speak(sentence);

            AudioClip currentClip = audioQueue.Dequeue();
            aSource.clip = currentClip;
            aSource.Play();
        }
    }
    void AddAudio(string sentence)
    {
        
        string replacement = Regex.Replace(sentence, @"\t|\n|\r", "");

        //audioQueue.Enqueue(replacement);
        if (allClips.ContainsKey(replacement))
        {
            audioQueue.Enqueue(allClips[replacement]);
        }
        else
        {
            Debug.Log("Could not find audioFile: " + sentence);
        }

    }
}

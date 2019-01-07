using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedRobotHead : MonoBehaviour {

    public float blinkDuration = 0.6f;
    public float delayBlinkBetweenEyes = 0.1f;
    public Transform leftEye, rightEye, eyeCenter, head;
    public AnimationCurve blinkCurveOpen;
    public AnimationCurve blinkCurveClose;
    private Vector3 centerTargetPos, originalHeadPosition;

    public float minEyeSize = 0.25f;
    public float maxEyeSize = 0.4f;

    public float hoverSpeed = 1f;
    public float headAmplitude = 0.1f;
    public float headRotationSpeed = 180f;
    private Vector3 lookPoint = new Vector3(0,0,-1);

    public Material mouthmaterial;


    public LineRenderer lRender;
    public int numVerticesMouth = 10;

    //Mouth Settings
    private float XDistBetweenVertices = 0.5f;
    public float YDistBetweenVertices = 0.5f;
    private bool blinking = false;
    public AudioSource aSource;
    private Coroutine mouthRoutine;
    // Use this for initialization
    void Start () {
        

        XDistBetweenVertices = 0.8f / (numVerticesMouth - 1);
        originalHeadPosition = head.transform.position;
        centerTargetPos = eyeCenter.transform.localPosition;
        InvokeRepeating("UpdateScaleEyes", 1f, 4f);
        InvokeRepeating("Blink", 5f, 6f);
        InvokeRepeating("LookAtRandomPoint", 1f, 5f);
        InitLineRender();
        //InvokeRepeating("BlinkMouth", 1f, 1.5f);
        StartCoroutine(AnimateMouth());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Blink();
        }

        eyeCenter.transform.localPosition = Vector3.Lerp(eyeCenter.transform.localPosition, centerTargetPos, Time.deltaTime);
        head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.LookRotation(lookPoint), headRotationSpeed * Time.deltaTime);
        HoverHead();
    }

    void UpdateScaleEyes()
    {
        if (!blinking)
        {
            centerTargetPos = new Vector3(Random.Range(-0.28f, 0.28f), Random.Range(0.1f, 0.4f), 1);
            float rnd = Random.Range(minEyeSize, maxEyeSize);
            StartCoroutine(UpdatScaleEye(leftEye, rnd));
            StartCoroutine(UpdatScaleEye(rightEye, (minEyeSize + maxEyeSize) - rnd));
        }

    }

    void HoverHead()
    {
        head.transform.position = originalHeadPosition + new Vector3(0, headAmplitude * Mathf.Sin(hoverSpeed*Time.time), 0);
    }

    void LookAtRandomPoint()
    {
        lookPoint = originalHeadPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-1, 0));

    }

    void BlinkMouth()
    {
        StartCoroutine(
            GenericLerp(
            (t) =>
            {
                mouthmaterial.color = Color.Lerp(Color.white, Color.green, Random.value);
            }
            , 0.5f));
    }


    IEnumerator GenericLerp(System.Action<float> action, float duration)
    {
        float t = 0;
        float dTime =Time.deltaTime* ( 1f / duration);
        while(t < 1f)
        {
            yield return null;
            if(action != null)
            {
                action(t);
            }
            t += dTime;
        }
    }


    IEnumerator UpdatScaleEye(Transform eye, float targetScale)
    {

        Vector3 targetScaleVec = new Vector3(targetScale, targetScale, targetScale);
        float t = 0;
        while (t < 1f)
        {
            eye.transform.localScale = Vector3.Lerp(eye.transform.localScale, targetScaleVec, Time.deltaTime);
            yield return null;
            t += Time.deltaTime;
        }
        


    }


    void Blink()
    {
        StartCoroutine(
            Sequence(
                DoAction(() => { blinking = true; }),
                DoAction(() => { StartCoroutine(BlinkEye(leftEye, blinkDuration)); }),
                Wait(0.3f),
                DoAction(() => { StartCoroutine(BlinkEye(rightEye, blinkDuration)); }),
                Wait(2 * blinkDuration + 0.3f),
                DoAction(() => { blinking = false; })
            )
        );



      

    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator BlinkEye(Transform eye, float time)
    {

        Vector3 lScale = eye.transform.localScale;
        Vector3 targetScale = new Vector3(lScale.x, 0.025f, 0.01f);
        float dTime = 1f / time;
        float t = 0;
        while(t< 1f)
        {
            eye.transform.localScale = Vector3.Lerp(lScale, targetScale, blinkCurveClose.Evaluate(t));
            yield return null;
            t += Time.deltaTime * dTime;
        }
        t = 0;
        while (t < 1f)
        {
            eye.transform.localScale = Vector3.Lerp(targetScale, lScale, blinkCurveOpen.Evaluate(t));
            yield return null;
            t += Time.deltaTime * dTime;
        }

    }

    public IEnumerator Sequence(params IEnumerator[] input)
    {
        foreach(IEnumerator ie in input)
        {
            while (ie.MoveNext())
            {
                yield return ie.Current;
            }
        }
    }

    public IEnumerator DoAction(System.Action action, float delay = 0) {

        yield return new WaitForSeconds(delay);
        if(action != null)
        {
            action();
        }
    }
    public void InitLineRender()
    {
        lRender.positionCount = numVerticesMouth;
        for (int i = 0; i < numVerticesMouth; i++)
        {
            lRender.SetPosition(i, new Vector3(i * XDistBetweenVertices, 0, 0));
        }

    }

    IEnumerator AnimateMouth()
    {
        float dTime = 0.1f;
        while (true)
        {
            if (aSource.isPlaying)
            {
                for (int i = 2; i < numVerticesMouth - 2; i++)
                {
                    float num = Util.Choose<float>(YDistBetweenVertices, -YDistBetweenVertices);
                    lRender.SetPosition(i, new Vector3(i * XDistBetweenVertices, num, 0));
                }
                yield return new WaitForSeconds(dTime);
            }
            else
            {
                for (int i = 2; i < numVerticesMouth - 2; i++)
                {
                    lRender.SetPosition(i, new Vector3(i * XDistBetweenVertices, 0, 0));
                }
            }
            yield return null;
        }
    }


}

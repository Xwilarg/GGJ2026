using GGJ2026.Manager;
using GGJ2026.Player;
using System;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class ArrangementSwitchingQuantized : MonoBehaviour
{
    public double bpm = 72.0F;
    public int signatureHi = 4;
    public int signatureLo = 4;

    public AudioSource[] arrangementA;
    public AudioSource[] solosA;
    public AudioSource[] arrangementB;
    public AudioSource[] solosB;
    public AudioSource[] arrangementC;
    public AudioSource[] solosC;
    public AudioSource[] arrangementD;
    public AudioSource[] solosD;
    public AudioClip sourceClip;
    public AudioMixer MainMixer;
    private string ArrangementAGroupVolume = "ArrangementAGroupVolume";
    private string ArrangementBGroupVolume = "ArrangementBGroupVolume";
    private string ArrangementCGroupVolume = "ArrangementCGroupVolume";
    private string ArrangementDGroupVolume = "ArrangementDGroupVolume";
    public float arrangementAVolume;
    public float arrangementBVolume;
    public float arrangementCVolume;
    public float arrangementDVolume;
    private float volumeCheckA;
    private float volumeCheckB;
    private float volumeCheckC;
    private float volumeCheckD;
    private int toggle = 0;
    public MaskManager maskManager;
    //private MaskType activeMask;

    private double nextTick = 0.0F;
    private double sampleRate = 0.0F;
    private int accent;
    private bool running = false;
    private double beatLength;
    private double barLength;
    private int barCount;
    private double nextBar;
    private double nextBeat;
    private double timeToNextBar;
    private double timeToNextBeat;

    private double clipDuration;
    private double clipDurationInBars;
    private double startTime;
    private double nextStartTime;
    private bool queueNextTrack;

    void Start()
    {
        accent = signatureHi;
        beatLength = 60d / bpm;
        barLength = beatLength * signatureLo;
        barCount = 1;
        double startTick = AudioSettings.dspTime + 0.5;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;

        clipDuration = (double)sourceClip.samples / sourceClip.frequency;
        clipDurationInBars = clipDuration / barLength;
        //Debug.Log("Clip Duration in Bars is " + clipDurationInBars);

        startTime = AudioSettings.dspTime + 0.5f;
        nextStartTime = startTime + clipDuration;
        for (int i = 0; i < arrangementA.Length / 2; i++)
        {
            arrangementA[i].PlayScheduled(startTime);
            //Debug.Log("Play scheduled for track " + i);
        }
        for (int i = 0; i < arrangementB.Length / 2; i++)
        {
            arrangementB[i].PlayScheduled(startTime);
            //Debug.Log("Play scheduled for track " + i);
        }
        for (int i = 0; i < arrangementC.Length / 2; i++)
        {
            arrangementC[i].PlayScheduled(startTime);
            //Debug.Log("Play scheduled for track " + i);
        }
        for (int i = 0; i < arrangementD.Length / 2; i++)
        {
            arrangementD[i].PlayScheduled(startTime);
            //Debug.Log("Play scheduled for track " + i);
        }

        arrangementAVolume = 0;
        arrangementBVolume = 1;
        arrangementCVolume = 0;
        arrangementDVolume = 0;
        MainMixer.SetFloat(ArrangementAGroupVolume, -80.0f);
        MainMixer.SetFloat(ArrangementBGroupVolume, 0.0f);
        MainMixer.SetFloat(ArrangementCGroupVolume, -80.0f);
        MainMixer.SetFloat(ArrangementDGroupVolume, -80.0f);
        toggle = 1 - toggle;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;

        int n = 0;
        while (n < dataLen)
        {
            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                nextBeat = nextTick / sampleRate;
                if (++accent > signatureHi)
                {
                    accent = 1;
                    //Debug.Log("Bar " + barCount);
                    barCount++;
                    nextBar = AudioSettings.dspTime + barLength;
                    if (barCount > (int)clipDurationInBars)
                    {
                        nextStartTime = AudioSettings.dspTime + barLength;
                        queueNextTrack = true;
                        barCount = 1;
                    }
                }
                //Debug.Log("Tick: " + accent + "/" + signatureHi);
            }
            n++;
        }
    }

    private void Update()
    {
        if (queueNextTrack)
        {
            // Schedule the next audio sources to play
            switch (toggle)
            {
                case 0:
                    //schedule base layers
                    for (int i = 0; i < arrangementA.Length / 2; i++)
                    {
                        arrangementA[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    //schedule additional layers and set volume
                    for (int i = 0; i < arrangementB.Length / 2; i++)
                    {
                        arrangementB[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    for (int i = 0; i < arrangementC.Length / 2; i++)
                    {
                        arrangementC[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    for (int i = 0; i < arrangementD.Length / 2; i++)
                    {
                        arrangementD[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    break;
                case 1:
                    //schedule base layers
                    for (int i = arrangementA.Length / 2; i < arrangementA.Length; i++)
                    {
                        arrangementA[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    //schedule additional layers and set volume
                    for (int i = arrangementB.Length / 2; i < arrangementB.Length; i++)
                    {
                        arrangementB[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    for (int i = arrangementC.Length / 2; i < arrangementC.Length; i++)
                    {
                        arrangementC[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    for (int i = arrangementD.Length / 2; i < arrangementD.Length; i++)
                    {
                        arrangementD[i].PlayScheduled(nextStartTime);
                        //Debug.Log("Play scheduled for track " + i);
                    }
                    break;
            }
            //randomly determine whether or not the next set of queued audio sources will include a solo
            float soloChance = UnityEngine.Random.Range(0.0f, 1.0f);
            if (soloChance < 0.4f)
            {
                switch (maskManager.CurrentMask)
                {
                    case MaskType.None:
                        int selectedSoloA = UnityEngine.Random.Range(0, solosA.Length);
                        if (solosA[selectedSoloA].isPlaying)
                        {
                            int forbiddenValue = selectedSoloA;
                            while (forbiddenValue == selectedSoloA)
                            {
                                selectedSoloA = UnityEngine.Random.Range(0, solosA.Length);
                            }
                        }
                        solosA[selectedSoloA].PlayScheduled(nextStartTime);
                        break;
                    case MaskType.Joy:
                        int selectedSoloB = UnityEngine.Random.Range(0, solosB.Length);
                        if (solosB[selectedSoloB].isPlaying)
                        {
                            int forbiddenValue = selectedSoloB;
                            while (forbiddenValue == selectedSoloB)
                            {
                                selectedSoloB = UnityEngine.Random.Range(0, solosB.Length);
                            }
                        }
                        solosB[selectedSoloB].PlayScheduled(nextStartTime);
                        break;
                    case MaskType.Sorrow:
                        int selectedSoloC = UnityEngine.Random.Range(0, solosC.Length);
                        if (solosC[selectedSoloC].isPlaying)
                        {
                            int forbiddenValue = selectedSoloC;
                            while (forbiddenValue == selectedSoloC)
                            {
                                selectedSoloC = UnityEngine.Random.Range(0, solosC.Length);
                            }
                        }
                        solosC[selectedSoloC].PlayScheduled(nextStartTime);
                        break;
                    case MaskType.Wrath:
                        int selectedSoloD = UnityEngine.Random.Range(0, solosD.Length);
                        if (solosC[selectedSoloD].isPlaying)
                        {
                            int forbiddenValue = selectedSoloD;
                            while (forbiddenValue == selectedSoloD)
                            {
                                selectedSoloD = UnityEngine.Random.Range(0, solosD.Length);
                            }
                        }
                        solosD[selectedSoloD].PlayScheduled(nextStartTime);
                        break;
                    default: //in the case where input falls outside the expected range, do nothing
                        break;
                }
            }
            toggle = 1 - toggle;
            queueNextTrack = false;
        }
        /*
        //quantized arrangement switching to next bar using o key
        if (Input.GetKeyDown("o"))
        {

            timeToNextBar = nextBar - AudioSettings.dspTime;

            if (arrangementBVolume > 0)
            {
                arrangementAVolume = 1;
                arrangementBVolume = 0;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.5f, arrangementAVolume, (float)timeToNextBar));
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.5f, arrangementBVolume, (float)timeToNextBar));
            }
            else
            {
                arrangementAVolume = 0;
                arrangementBVolume = 1;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.5f, arrangementAVolume, (float)timeToNextBar));
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.5f, arrangementBVolume, (float)timeToNextBar));
            }
        }

        //quantized arrangement switching to next beat using p key
        if (Input.GetKeyDown("p"))
        {

            timeToNextBeat = nextBeat - AudioSettings.dspTime;

            if (arrangementBVolume > 0)
            {
                arrangementAVolume = 1;
                arrangementBVolume = 0;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.0f, arrangementAVolume, (float)timeToNextBeat));
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.0f, arrangementBVolume, (float)timeToNextBeat));
            }
            else
            {
                arrangementAVolume = 0;
                arrangementBVolume = 1;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 0.5f, arrangementAVolume, (float)timeToNextBeat));
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 0.5f, arrangementBVolume, (float)timeToNextBeat));
            }
        }
        */
    }

    public void SetMusicArrangementByMask(MaskType inputMask)
    {
        timeToNextBeat = nextBeat - AudioSettings.dspTime;

        MainMixer.GetFloat(ArrangementAGroupVolume, out volumeCheckA);
        MainMixer.GetFloat(ArrangementBGroupVolume, out volumeCheckB);
        MainMixer.GetFloat(ArrangementCGroupVolume, out volumeCheckC);
        MainMixer.GetFloat(ArrangementDGroupVolume, out volumeCheckD);

        switch (inputMask)
        {
            case MaskType.None:
                arrangementAVolume = 1;
                arrangementBVolume = 0;
                arrangementCVolume = 0;
                arrangementDVolume = 0;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.0f, arrangementAVolume, (float)timeToNextBeat));
                if (volumeCheckB > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.0f, arrangementBVolume, (float)timeToNextBeat));
                }
                if (volumeCheckC > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementCGroupVolume, 1.0f, arrangementCVolume, (float)timeToNextBeat));
                }
                if (volumeCheckD > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementDGroupVolume, 1.0f, arrangementDVolume, (float)timeToNextBeat));
                }
                break;
            case MaskType.Joy:
                arrangementAVolume = 0;
                arrangementBVolume = 1;
                arrangementCVolume = 0;
                arrangementDVolume = 0;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.0f, arrangementBVolume, (float)timeToNextBeat));
                if (volumeCheckA > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.0f, arrangementAVolume, (float)timeToNextBeat));
                }
                if (volumeCheckC > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementCGroupVolume, 1.0f, arrangementCVolume, (float)timeToNextBeat));
                }
                if (volumeCheckD > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementDGroupVolume, 1.0f, arrangementDVolume, (float)timeToNextBeat));
                }
                break;
            case MaskType.Sorrow:
                arrangementAVolume = 0;
                arrangementBVolume = 0;
                arrangementCVolume = 1;
                arrangementDVolume = 0;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementCGroupVolume, 1.0f, arrangementCVolume, (float)timeToNextBeat));
                if (volumeCheckA > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.0f, arrangementAVolume, (float)timeToNextBeat));
                }
                if (volumeCheckB > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.0f, arrangementBVolume, (float)timeToNextBeat));
                }
                if (volumeCheckD > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementDGroupVolume, 1.0f, arrangementDVolume, (float)timeToNextBeat));
                }
                break;
            case MaskType.Wrath:
                arrangementAVolume = 0;
                arrangementBVolume = 0;
                arrangementCVolume = 0;
                arrangementDVolume = 1;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementDGroupVolume, 1.0f, arrangementDVolume, (float)timeToNextBeat));
                if (volumeCheckA > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.0f, arrangementAVolume, (float)timeToNextBeat));
                }
                if (volumeCheckB > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.0f, arrangementBVolume, (float)timeToNextBeat));
                }
                if (volumeCheckC > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementCGroupVolume, 1.0f, arrangementCVolume, (float)timeToNextBeat));
                }
                break;
            default: //use MaskType.None case as default case
                arrangementAVolume = 1;
                arrangementBVolume = 0;
                arrangementCVolume = 0;
                arrangementDVolume = 0;
                StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementAGroupVolume, 1.0f, arrangementAVolume, (float)timeToNextBeat));
                if (volumeCheckB > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementBGroupVolume, 1.0f, arrangementBVolume, (float)timeToNextBeat));
                }
                if (volumeCheckC > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementCGroupVolume, 1.0f, arrangementCVolume, (float)timeToNextBeat));
                }
                if (volumeCheckD > -80.0f)
                {
                    StartCoroutine(FadeMixerGroupQuantized.StartFadeQuantized(MainMixer, ArrangementDGroupVolume, 1.0f, arrangementDVolume, (float)timeToNextBeat));
                }
                break;
        }
    }
}

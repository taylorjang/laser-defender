using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthKeeper : MonoBehaviour {

    public static float health = 0f;
    private Text myText;

    void Start()
    {
        myText = GetComponent<Text>();
        Reset();
    }

    public void Health(float points)
    {
        health = points;
        myText = GetComponent<Text>();
        myText.text = health.ToString();
    }

    public static void Reset()
    {
        health = 0;
    }
}

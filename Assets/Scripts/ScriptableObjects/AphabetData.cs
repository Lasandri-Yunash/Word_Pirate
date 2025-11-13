using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu]

public class AphabetData : ScriptableObject
{
    [System.Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite image;

    }

    public List<LetterData> AlphaPlain = new List<LetterData>();
    public List<LetterData> AlphaNormal = new List<LetterData>();
    public List<LetterData> AlphaHightlighted = new List<LetterData>();
    public List<LetterData> AlphaWrong = new List<LetterData>();
}

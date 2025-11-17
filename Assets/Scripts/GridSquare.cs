using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public int SquareIndex {  get; set; }

    private AphabetData.LetterData _normalletterData;
    private AphabetData.LetterData _selectedletterData;
    private AphabetData.LetterData _correctedletterData;


    private SpriteRenderer _displayedImage;


    private void Start()
    {
        _displayedImage = GetComponent<SpriteRenderer>();

    }

    public void SetSprite(AphabetData.LetterData normalLetterData, AphabetData.LetterData selectedLetterdata, AphabetData.LetterData correctedLetterData)
    {
        _normalletterData = normalLetterData;
        _selectedletterData = selectedLetterdata;
        _correctedletterData = correctedLetterData;

        GetComponent<SpriteRenderer>().sprite = normalLetterData.image;
    }



}

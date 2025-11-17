using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{

    public GameData currentGameDta;
    public GameObject gridSquarePrefab;
    public AphabetData alphabetData;

    public float squareOffset = 8.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();

    private void Start()
    {
        SpawnGridSquares();
        SetSquarePosition();
    }

    private void SetSquarePosition()
    {
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTranform = _squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (squareRect.width * squareTranform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTranform.localScale.y + squareOffset) * 0.01f

        };

        var startPosition = GetFirstSquarePosition();

        int coloumnNumber = 0;
        int rowNumber = 0;

        foreach(var square in _squareList)
        {
            if(rowNumber +1 > currentGameDta.setectedboardData.Rows)
            {
                coloumnNumber++;
                rowNumber = 0;

            }

            var positioNX = startPosition.x + offset.x * coloumnNumber;
            var positioNY = startPosition.y - offset.y * rowNumber;

            square.GetComponent<Transform>().position = new Vector2(positioNX,positioNY);
            rowNumber++;
        }

    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);


        squareSize.x = squareRect.width * squareTransform.localScale.x; ;
        squareSize.y = squareRect.height * squareTransform.localScale.y; ;

        var midWidthPosition = (((currentGameDta.setectedboardData.Columns -1) *squareSize.x) / 2) * 0.01f;
        var midHightPosition = (((currentGameDta.setectedboardData.Rows -1) * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midHightPosition;

        return startPosition;
    }
    private void SpawnGridSquares()
    {

        if(currentGameDta != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));

            foreach(var squares in currentGameDta.setectedboardData.Board)
            {
                foreach (var squareLetter in squares.Row)
                {
                    var normalLetterData = alphabetData.AlphaNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.AlphaHightlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.AlphaWrong.Find(data => data.letter == squareLetter);

                    if(normalLetterData.image ==null || selectedLetterData.image == null)
                    {
                        Debug.LogError("All feild in your array should have some Letters. Presss fill up with random button in board data to add random Letters : letter : " + squareLetter);
#if UNITY_EDITOR

                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
#endif
                    }
                    else
                    {
                        _squareList.Add(Instantiate(gridSquarePrefab));
                        _squareList[_squareList.Count -1].GetComponent<GridSquare>().SetSprite(normalLetterData, correctLetterData,selectedLetterData);
                        _squareList[_squareList.Count - 1].transform.SetParent(this.transform);
                        _squareList[_squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f,0f,0f);
                        _squareList[_squareList.Count - 1].transform.localScale = squareScale;


                    }


                }
            }

        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjesment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjesment;
            finalScale.y -= adjesment;

            if(finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjesment;
                finalScale.y = adjesment;

                return finalScale;

            }
        }

        return finalScale;

    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameDta.setectedboardData.Columns * squareSize.x) / 2) * 0.01f;
        var midHightPosition = ((currentGameDta.setectedboardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 :midWidthPosition;
        startPosition.y = midHightPosition;

        return startPosition.x < GetHalfScreenwidth() * -1 || startPosition.y > topPosition;


    }


    private float GetHalfScreenwidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width /2 ;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Face
{
    public static string TestWord = "DUMPY";

    // Properties
    public int Width = 5;
    public int Height = 5;
    public CubeClosestFace.CubeFace CubeFace;
    public GameObject PanelFace;
    public GameObject[,] FaceMatrix;
    public string Word = "";
    public List<GameObject> LetterGameObjects = new List<GameObject>();
    public int GuessIndex = 0;
    public int LetterIndex = 0;

    public Face(CubeClosestFace.CubeFace face, GameObject panelFace)
    {
        if (face == CubeClosestFace.CubeFace.None) { return; }

        CubeFace = face;
        PanelFace = panelFace;

        var childrenFace = GameObjectExtensions.GetChildren(PanelFace);
        FaceMatrix = ListExtensions.ToMatrix<GameObject>(childrenFace, Width, Height);
    }

    public void WriteToWord(string letter)
    {
        if (Word == null) { Word = ""; }
        if (Word.Length >= 5) { return; }

        Word += letter;
        LetterIndex++;
    }

    public void RemoveFromWord()
    {
        if (Word == null) { Word = ""; }
        if (Word.Length <= 0) { return; }

        Word = Word.Remove(Word.Length - 1, 1);
        LetterIndex--;
    }

    public void CheckWord()
    {
        // Check Word

        for (int i = 0; i < Width; i++)
        {
            string letter = Word[i].ToString();
            var realIndexes = StringExtensions.GetAllIndexes(TestWord, letter).ToList();
            var guessIndexes = StringExtensions.GetAllIndexes(Word, letter).ToList();

            if (realIndexes.Count == 0) { continue; }

        }
    }

    public void NextRow()
    {
        Word = "";
        LetterIndex = 0;
        GuessIndex++;
    }

    public void ChangePanelColor(int row, int col, Color color)
    {
        var image = FaceMatrix[row, col].GetComponent<Image>(); // GuessIndex, LetterIndex
        image.color = color;
    }
}


public class MainCube : MonoBehaviour
{

    // Static Properties
    public static MainCube Instance;

    // Fields
    private CubeClosestFace closestFace;

    private Face currentFace;

    private GameObject canvasFaceFront;
    private GameObject panelFaceFront;
    private Face faceFront;



    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        closestFace = CubeClosestFace.Instance;

        canvasFaceFront = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Front.ToString()}");
        panelFaceFront = canvasFaceFront.transform.GetChild(0).gameObject;
        faceFront = new Face(CubeClosestFace.CubeFace.Front, panelFaceFront);

        SetCurrentFace();
    }

    void Update()
    {
    }

    public void SetCurrentFace()
    {
        switch (closestFace.CurrentFace)
        {
            case CubeClosestFace.CubeFace.Front:
                currentFace = faceFront;
                break;
            default:
                currentFace = null;
                break;
        }
    }

    public void WriteToCurrentFace(string letter)
    {
        if (currentFace == null) { return; }

        DisplayLetter(letter);
        currentFace.WriteToWord(letter);
    }

    public void RemoveFromCurrentFace()
    {
        if (currentFace == null) { return; }

        currentFace.RemoveFromWord();
        RemoveLetter();
    }

    private void DisplayLetter(string letter)
    {
        if (currentFace.LetterIndex >= 5) { return; }

        GameObject currentPanel = currentFace.FaceMatrix[currentFace.GuessIndex, currentFace.LetterIndex];
        var text = currentPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        text.text = letter;
    }

    private void RemoveLetter()
    {
        if (currentFace.LetterIndex < 0) { return; }

        GameObject currentPanel = currentFace.FaceMatrix[currentFace.GuessIndex, currentFace.LetterIndex];
        var text = currentPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }

    public void GuessWord()
    {
        if (currentFace.Word.Length != 5) { return; }
        if (currentFace.GuessIndex >= 5) { return; }

        if (currentFace.GuessIndex == 4)
        {
            Debug.Log("Done");
            return;
        }

        currentFace.CheckWord();
        currentFace.NextRow();
    }
}

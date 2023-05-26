using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Face
{
    public static string TestWord = "ABCDD";

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
    public bool Done;
    public bool Correct;
    public Dictionary<string, Color> KeyboardColors = new Dictionary<string, Color>();

    // Fields
    private List<int> guessIndexes = new List<int>();


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
        if (Done) return;
        if (Word == null) Word = "";
        if (Word.Length >= 5) return;

        Word += letter;
        LetterIndex++;
    }

    public void RemoveFromWord()
    {
        if (Done) return;
        if (Word == null) Word = "";
        if (Word.Length <= 0) return;

        Word = Word.Remove(Word.Length - 1, 1);
        LetterIndex--;
    }

    public void CheckGuess()
    {
        if (Done) return;

        guessIndexes.Clear();

        for (int i = 0; i < Width; i++)
        {
            string letter = Word[i].ToString();
            var hiddenWordIndexes = StringExtensions.GetAllIndexes(TestWord, letter).ToList();
            var userWordIndexes = StringExtensions.GetAllIndexes(Word, letter).ToList();

            if (hiddenWordIndexes.Count == 0)
            {
                ApplyColor(i, letter, Color.gray);
                continue;
            }

            if (hiddenWordIndexes.Count != userWordIndexes.Count)
                ApplyColor(i, letter, Color.yellow);
            else
            {
                if (Enumerable.SequenceEqual(hiddenWordIndexes, userWordIndexes))
                {
                    foreach (int index in userWordIndexes)
                    {
                        guessIndexes.Add(index);
                        ApplyColor(i, letter, Color.green);
                    }
                }
                else
                {
                    foreach (int index in userWordIndexes)
                        ApplyColor(i, letter, Color.yellow);
                }
            }
        }

        var guessIndexesFiltered = guessIndexes.Distinct().ToList();
        guessIndexesFiltered.Sort();
        bool result = ListExtensions.IsOrderedSequence(guessIndexesFiltered, Width);

        if (GuessIndex == 4) Done = true;

        if (result)
        {
            Correct = true;
            Done = true;
        }
    }

    public void ApplyColor(int index, string letter, Color color)
    {
        ChangePanelColor(GuessIndex, index, color);
        UIKeyboardButtonHandler.Instance.ChangeButtonColor(letter, color);

        try
        {
            KeyboardColors.Add(letter, color);
        }
        catch (global::System.Exception)
        {
            return;
        }
    }

    public void NextRow()
    {
        if (Done) return;

        Word = "";
        LetterIndex = 0;
        GuessIndex++;
    }

    public void ChangePanelColor(int row, int col, Color color)
    {
        var image = FaceMatrix[row, col].GetComponent<Image>(); // [GuessIndex, LetterIndex]
        image.color = color;
    }

}


public class MainCube : MonoBehaviour
{

    // Static Properties
    public static MainCube Instance;

    // Fields
    private CubeClosestFace closestFace;
    private UIKeyboardButtonHandler buttonHandler;

    private Face currentFace;

    private GameObject canvasFaceFront;
    private GameObject panelFaceFront;
    private Face faceFront;

    private GameObject canvasFaceBack;
    private GameObject panelFaceBack;
    private Face faceBack;

    private GameObject canvasFaceTop;
    private GameObject panelFaceTop;
    private Face faceTop;

    private GameObject canvasFaceBottom;
    private GameObject panelFaceBottom;
    private Face faceBottom;

    private GameObject canvasFaceLeft;
    private GameObject panelFaceLeft;
    private Face faceLeft;

    private GameObject canvasFaceRight;
    private GameObject panelFaceRight;
    private Face faceRight;



    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        closestFace = CubeClosestFace.Instance;
        buttonHandler = UIKeyboardButtonHandler.Instance;

        canvasFaceFront = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Front.ToString()}");
        panelFaceFront = canvasFaceFront.transform.GetChild(0).gameObject;
        faceFront = new Face(CubeClosestFace.CubeFace.Front, panelFaceFront);

        canvasFaceBack = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Back.ToString()}");
        panelFaceBack = canvasFaceBack.transform.GetChild(0).gameObject;
        faceBack = new Face(CubeClosestFace.CubeFace.Back, panelFaceBack);

        canvasFaceTop = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Top.ToString()}");
        panelFaceTop = canvasFaceTop.transform.GetChild(0).gameObject;
        faceTop = new Face(CubeClosestFace.CubeFace.Top, panelFaceTop);

        canvasFaceBottom = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Bottom.ToString()}");
        panelFaceBottom = canvasFaceBottom.transform.GetChild(0).gameObject;
        faceBottom = new Face(CubeClosestFace.CubeFace.Bottom, panelFaceBottom);

        canvasFaceLeft = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Left.ToString()}");
        panelFaceLeft = canvasFaceLeft.transform.GetChild(0).gameObject;
        faceLeft = new Face(CubeClosestFace.CubeFace.Left, panelFaceLeft);

        canvasFaceRight = GameObject.Find($"Canvas Face {CubeClosestFace.CubeFace.Right.ToString()}");
        panelFaceRight = canvasFaceRight.transform.GetChild(0).gameObject;
        faceRight = new Face(CubeClosestFace.CubeFace.Right, panelFaceRight);

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
                ColorKeyboard();
                break;
            case CubeClosestFace.CubeFace.Back:
                currentFace = faceBack;
                ColorKeyboard();
                break;
            case CubeClosestFace.CubeFace.Top:
                currentFace = faceTop;
                ColorKeyboard();
                break;
            case CubeClosestFace.CubeFace.Bottom:
                currentFace = faceBottom;
                ColorKeyboard();
                break;
            case CubeClosestFace.CubeFace.Left:
                currentFace = faceLeft;
                ColorKeyboard();
                break;
            case CubeClosestFace.CubeFace.Right:
                currentFace = faceRight;
                ColorKeyboard();
                break;
            default:
                currentFace = null;
                ColorKeyboard();
                break;
        }
    }

    public void ColorKeyboard()
    {
        if (currentFace == null)
        {
            buttonHandler.ClearKeyboard();
            return;
        }

        buttonHandler.ClearKeyboard();

        foreach (var item in currentFace.KeyboardColors)
        {
            buttonHandler.ChangeButtonColor(item.Key, item.Value);
        }
    }

    public void WriteToCurrentFace(string letter)
    {
        if (currentFace == null) return;
        if (currentFace.Done) return;

        DisplayLetter(letter);
        currentFace.WriteToWord(letter);
    }

    public void RemoveFromCurrentFace()
    {
        if (currentFace == null) return;
        if (currentFace.Done) return;

        currentFace.RemoveFromWord();
        RemoveLetter();
    }

    private void DisplayLetter(string letter)
    {
        if (currentFace.Done) return;
        if (currentFace.LetterIndex >= currentFace.Width) return;

        GameObject currentPanel = currentFace.FaceMatrix[currentFace.GuessIndex, currentFace.LetterIndex];
        var text = currentPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        text.text = letter;
    }

    private void RemoveLetter()
    {
        if (currentFace.Done) return;
        if (currentFace.LetterIndex < 0) return;

        GameObject currentPanel = currentFace.FaceMatrix[currentFace.GuessIndex, currentFace.LetterIndex];
        var text = currentPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }

    public void GuessWord()
    {
        if (currentFace.Done) return;
        if (currentFace.Word.Length != currentFace.Width) return;
        if (currentFace.GuessIndex >= currentFace.Width) return;

        currentFace.CheckGuess();

        if (currentFace.Done)
        {
            Debug.Log("Done");

            if (currentFace.Correct)
                Debug.Log("Correct");
            else
                Debug.Log("Incorrect");

            return;
        }

        currentFace.NextRow();
    }
}

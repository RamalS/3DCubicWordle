using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum CubeFace
{
    None,
    Front,
    Back,
    Top,
    Bottom,
    Left,
    Right
}

public class Face
{
    public string HiddenWord = "ABCDD";

    // Properties
    public int Width = 5;
    public int Height = 5;
    public CubeFace CubeFace;
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


    public Face(CubeFace face, GameObject panelFace)
    {
        if (face == CubeFace.None) { return; }

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
            var hiddenWordIndexes = StringExtensions.GetAllIndexes(HiddenWord, letter).ToList();
            var userWordIndexes = StringExtensions.GetAllIndexes(Word, letter).ToList();

            if (hiddenWordIndexes.Count == 0)
            {
                ApplyColor(i, letter, Color.gray);
                continue;
            }

            for (int j = 0; j < userWordIndexes.Count; j++)
            {
                if (hiddenWordIndexes.Contains(userWordIndexes[j]))
                {
                    guessIndexes.Add(userWordIndexes[j]);
                    ApplyColor(userWordIndexes[j], letter, Color.green);
                }
                else
                {
                    ApplyColor(userWordIndexes[j], letter, Color.yellow);
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
            if (KeyboardColors.ContainsKey(letter))
            {
                if (color == Color.green)
                {
                    KeyboardColors[letter] = color;
                }
            }
            else
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

public class UIFace
{
    public GameObject CanvasFace;
    public GameObject PanelFace;
    public Face Face;

    public UIFace(GameObject canvasFace, GameObject panelFace, Face face)
    {
        CanvasFace = canvasFace;
        PanelFace = panelFace;
        Face = face;
    }
}


public class MainCube : MonoBehaviour
{

    // Static Properties
    public static MainCube Instance;

    // Properties
    public int FacesCorrect;
    public int FacesDone;

    // Serialize Fields
    [SerializeField] TMP_Text TextScore;
    [SerializeField] TextAsset WordleWords;

    // Fields
    private CubeClosestFace closestFace;
    private UIKeyboardButtonHandler buttonHandler;
    private UIManager uiManager;
    private List<UIFace> uiFaces = new List<UIFace>();
    private Face currentFace;


    private void InitFaces()
    {
        foreach (CubeFace cubeFace in Enum.GetValues(typeof(CubeFace)))
        {
            if (cubeFace == CubeFace.None) continue;

            GameObject canvasFace = GameObject.Find($"Canvas Face {cubeFace.ToString()}");
            GameObject panelFace = canvasFace.transform.GetChild(0).gameObject;
            Face face = new Face(cubeFace, panelFace);
            uiFaces.Add(new UIFace(canvasFace, panelFace, face));
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        closestFace = CubeClosestFace.Instance;
        buttonHandler = UIKeyboardButtonHandler.Instance;
        uiManager = UIManager.Instance;

        InitFaces();

        SetCurrentFace();

        TextScore.SetText($"Score: {FacesCorrect}");

        string[] words = WordleWords.text.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );





        int f = 0;
    }


    public void SetCurrentFace()
    {
        if (closestFace.CurrentFace == CubeFace.None) return;

        currentFace = uiFaces[(int)closestFace.CurrentFace - 1].Face;
        ColorKeyboard();
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
        if (currentFace.Word.Length != currentFace.Width) return;
        if (currentFace.GuessIndex >= currentFace.Width) return;

        currentFace.CheckGuess();

        if (currentFace.Done)
        {
            Debug.Log("Done");
            FacesDone++;

            if (FacesDone >= 6)
            {
                // GAME OVER
                Debug.Log("Game Over");

                uiManager.ActivateGameOver(FacesCorrect);
            }

            if (currentFace.Correct)
            {
                FacesCorrect++;
                TextScore.SetText($"Score: {FacesCorrect}");
            }
            else
                Debug.Log("Incorrect");

            return;
        }

        currentFace.NextRow();
    }
}

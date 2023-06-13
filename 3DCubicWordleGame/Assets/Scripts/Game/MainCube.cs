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
    public string HiddenWord;

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


    public Face(CubeFace face, GameObject panelFace, string word)
    {
        if (face == CubeFace.None) return; 

        HiddenWord = word.ToUpper();

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
    public Face currentFace;

    // Serialize Fields
    [SerializeField] TMP_Text TextScore;
    [SerializeField] TextAsset WordleWords;
    [SerializeField] TMP_Text HintFront;
    [SerializeField] TMP_Text HintBack;
    [SerializeField] TMP_Text HintTop;
    [SerializeField] TMP_Text HintBottom;
    [SerializeField] TMP_Text HintLeft;
    [SerializeField] TMP_Text HintRight;
    


    // Fields
    private CubeClosestFace closestFace;
    private UIKeyboardButtonHandler buttonHandler;
    private UIManager uiManager;
    private List<UIFace> uiFaces = new List<UIFace>();
    
    private Dictionary<char, List<string>> startsWith = new Dictionary<char, List<string>>();
    private Dictionary<char, List<string>> endsWith = new Dictionary<char, List<string>>();
    private List<string> faceWords;
    private List<string> validWords;


    private void InitFaces()
    {
        foreach (CubeFace cubeFace in Enum.GetValues(typeof(CubeFace)))
        {
            if (cubeFace == CubeFace.None) continue;

            GameObject canvasFace = GameObject.Find($"Canvas Face {cubeFace.ToString()}");
            GameObject panelFace = canvasFace.transform.GetChild(0).gameObject;
            Face face = new Face(cubeFace, panelFace, faceWords[(int)cubeFace - 1]);
            uiFaces.Add(new UIFace(canvasFace, panelFace, face));
        }
    }

    private List<string> StartsWith(List<string> words, char letter)
    {
        List<string> wordsStartingWith = new List<string>();

        foreach (var word in words)
        {
            if (word.Length <= 0) continue;

            if (word.First() == letter)
                wordsStartingWith.Add(word); 
        }

        return wordsStartingWith;
    }

    private List<string> EndsWith(List<string> words, char letter)
    {
        List<string> wordsEndingWith = new List<string>();

        foreach (var word in words)
        {
            if (word.Length <= 0) continue;

            if (word.Last() == letter)
                wordsEndingWith.Add(word);
        }

        return wordsEndingWith;
    }

    private List<string> StartsWithEndsWith(List<string> words, char startLetter, char endLetter) 
    {
        List<string> wordsStartingWith = StartsWith(words, startLetter);
        List<string> wordsStartingEndingWith = EndsWith(wordsStartingWith, endLetter);
        return wordsStartingEndingWith;
    }

    private void InitWords()
    {
        List<string> words = WordleWords.text.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        ).ToList();
        words = words.Where(s => !string.IsNullOrEmpty(s)).ToList();
        validWords = words;

        while (PickWords(words) != true)
        {

        }

    }

    private bool PickWords(List<string> words)
    {
        var random = new System.Random();

        string frontWord = words[random.Next(words.Count - 1)];

        List<string> wordsStartingWithFront = StartsWith(words, frontWord.Last());
        if (wordsStartingWithFront.Count == 0) return false;
        string rightWord = wordsStartingWithFront[random.Next(wordsStartingWithFront.Count - 1)];

        List<string> wordsStartingWithRight = StartsWith(words, rightWord.Last());
        if (wordsStartingWithRight.Count == 0) return false;
        string backWord = wordsStartingWithRight[random.Next(wordsStartingWithRight.Count - 1)];


        List<string> wordsStartingWithBackEndingWithFront = StartsWithEndsWith(words, backWord.Last(), frontWord.First());
        if (wordsStartingWithBackEndingWithFront.Count == 0) return false;
        string leftWord = wordsStartingWithBackEndingWithFront[random.Next(wordsStartingWithBackEndingWithFront.Count - 1)];

        List<string> wordsStartingWithBackEndingWithBack = StartsWithEndsWith(words, backWord.Last(), backWord.First());
        if (wordsStartingWithBackEndingWithBack.Count == 0) return false;
        string topWord = wordsStartingWithBackEndingWithBack[random.Next(wordsStartingWithBackEndingWithBack.Count - 1)];

        List<string> wordsStartingWithFrontEndingWithFront = StartsWithEndsWith(words, frontWord.First(), frontWord.Last());
        if (wordsStartingWithFrontEndingWithFront.Count == 0) return false;
        string bottomWord = wordsStartingWithFrontEndingWithFront[random.Next(wordsStartingWithFrontEndingWithFront.Count - 1)];

        faceWords = new List<string> { frontWord, backWord, topWord, bottomWord, leftWord, rightWord };

        HintFront.SetText($"Front: {frontWord}");
        HintBack.SetText($"Back: {backWord}");
        HintTop.SetText($"Top: {topWord}");
        HintBottom.SetText($"Bottom: {bottomWord}");
        HintLeft.SetText($"Left: {leftWord}");
        HintRight.SetText($"Right: {rightWord}");

        return true;
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

        InitWords();

        InitFaces();

        SetCurrentFace();

        TextScore.SetText($"Score: {FacesCorrect}");

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
        bool found = false;
        foreach (var word in validWords)
        {
            if (currentFace.Word.ToLower() == word)
            { found = true; break; }
        }
        if (!found) return;

        currentFace.CheckGuess();

        if (currentFace.Done)
        {
            Debug.Log("Done");
            FacesDone++;

            

            if (currentFace.Correct)
            {
                FacesCorrect++;
                TextScore.SetText($"Score: {FacesCorrect}");
            }
            else
                Debug.Log("Incorrect");

            if (FacesDone >= 6)
            {
                // GAME OVER
                Debug.Log("Game Over");

                uiManager.ActivateGameOver(FacesCorrect);
            }

            return;
        }

        currentFace.NextRow();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class to handle the user interface : buttons events and shown texts.
/// </summary>
public class UIManager : MonoBehaviour
{

    static string Ai = "ai";
    static string Human = "human";


    [SerializeField]
    Text _leftPlayerTypeText;

    [SerializeField]
    Text _rightPlayerTypeText;

    [SerializeField]
    Text _difficultyText;

    [SerializeField]
    Transform _mainMenu;

    [SerializeField]
    Transform _pauseMenu;

    ControllerAI.Difficulty _difficulty = ControllerAI.Difficulty.Normal;

    string _leftPlayerType = Ai;
    string _rightPlayerType = Ai;

    void Start()
    {
        UpdatePlayersTypesTexts();
        UpdateDifficultyText();
    }

    /// <summary>
    /// Set difficulty to easy
    /// </summary>
    public void AssignEasyDifficulty()
    {
        _difficulty = ControllerAI.Difficulty.Easy;
        UpdateDifficultyText();
    }

    /// <summary>
    /// Set difficulty to hard
    /// </summary>
    public void AssignHardDifficulty()
    {
        _difficulty = ControllerAI.Difficulty.Hard;
        UpdateDifficultyText();
    }

    /// <summary>
    /// Assign AI to left player
    /// </summary>
    public void AssignLeftPlayerAI()
    {
        _leftPlayerType = Ai;
        UpdatePlayersTypesTexts();
    }

    /// <summary>
    /// Assign Human to left player
    /// </summary>
    public void AssignLeftPlayerHuman()
    {
        _leftPlayerType = Human;
        UpdatePlayersTypesTexts();
    }

    /// <summary>
    /// Set difficulty to normal
    /// </summary>
    public void AssignNormalDifficulty()
    {
        _difficulty = ControllerAI.Difficulty.Normal;
        UpdateDifficultyText();
    }

    /// <summary>
    /// Assign AI to right player
    /// </summary>
    public void AssignRightPlayerAI()
    {
        _rightPlayerType = Ai;
        UpdatePlayersTypesTexts();
    }

    /// <summary>
    /// Assign Human to right player
    /// </summary>
    public void AssignRightPlayerHuman()
    {
        _rightPlayerType = Human;
        UpdatePlayersTypesTexts();
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    /// <summary>
    /// Continue playing
    /// </summary>
    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    /// <summary>
    /// Stop the current game and return to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    /// <summary>
    /// Start a new game using chosen parameters in menu
    /// </summary>
    public void StartNewGame()
    {
        // Default controllers
        Transform ball = GameManager.Instance.Ball.transform;
        float racketsDistance = GameManager.Instance.RacketsDistance;

        ControllerBase leftPlayerController = new ControllerAI(0, ControllerAI.Difficulty.Normal, racketsDistance, GameManager.Instance.LeftRacketInitialPosition, ball);
        ControllerBase rightPlayerController = new ControllerAI(1, ControllerAI.Difficulty.Normal, racketsDistance, GameManager.Instance.RightRacketInitialPosition, ball);

        float playerRacketSpeed = GameManager.Instance.PlayerRacketSpeed;

        int playerId = 0;

        // Create racket controller for the left player using chosen parameters
        if(_leftPlayerType == Human)
        {
            leftPlayerController = new ControllerHuman(playerId, ControllerBase.ControllerType.Human, playerRacketSpeed, GameManager.Instance.LeftRacketInitialPosition);
        }
        else if(_leftPlayerType == Ai)
        {
            leftPlayerController = new ControllerAI(playerId, _difficulty, racketsDistance, GameManager.Instance.LeftRacketInitialPosition, ball);
        }
        else
        {
            Debug.LogError("Error starting new game. Unknown controller type found in Main Menu: " + _leftPlayerType);
        }

        // Increases player id to match controls
        ++playerId;

        // Create racket controller for the right player using chosen parameters
        if (_rightPlayerType == Human)
        {
            rightPlayerController = new ControllerHuman(playerId, ControllerBase.ControllerType.Human, playerRacketSpeed, GameManager.Instance.RightRacketInitialPosition);
        }
        else if (_rightPlayerType == Ai)
        {
            rightPlayerController = new ControllerAI(playerId, _difficulty, racketsDistance, GameManager.Instance.RightRacketInitialPosition, ball);
        }
        else
        {
            Debug.LogError("Error starting new game. Unknown controller type found in Main Menu: " + _rightPlayerType);
        }

        // Start new game
        GameManager.Instance.StartNewGame(leftPlayerController, rightPlayerController);

        // Hide menus
        _mainMenu.gameObject.SetActive(false);
        _pauseMenu.gameObject.SetActive(false);
    }



    /// <summary>
    /// Updates players types interface texts
    /// </summary>
    void UpdatePlayersTypesTexts()
    {
        _leftPlayerTypeText.text = _leftPlayerType.ToUpper();
        _rightPlayerTypeText.text = _rightPlayerType.ToUpper();
    }

    /// <summary>
    /// Updates difficulty interface text
    /// </summary>
    void UpdateDifficultyText()
    {
        _difficultyText.text = _difficulty.ToString().ToUpper();
    }
}

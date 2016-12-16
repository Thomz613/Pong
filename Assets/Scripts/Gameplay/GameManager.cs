using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Singleton used as a simple engine to manage the game
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    static int _playersIdsCounter = 0;      // Ids used to match players, scores, and goals. These ids are different from the ones used in the ControllerBase derived classes.


    [SerializeField]
    BallManager _ball;

    [SerializeField]
    Transform _middlePoint;

    [SerializeField]
    RacketManager _leftRacket;

    [SerializeField]
    RacketManager _rightRacket;

    [SerializeField]
    GoalManager _leftGoal;

    [SerializeField]
    GoalManager _rightGoal;

    [SerializeField]
    Text _leftPlayerScoreText;

    [SerializeField]
    Text _rightPlayerScoreText;


    [SerializeField]
    float _serviceHalfMaxAngle = 45f;

    [SerializeField]
    float _playerRacketSpeed = 20f;

    [SerializeField]
    float _ballSpeed = 30f;


    [SerializeField]
    float _timeBetweenRounds = 5f;


    [SerializeField]
    Transform _mainMenu;

    [SerializeField]
    Transform _pauseMenu;



    Player _leftPlayer;
    Player _rightPlayer;

    float _racketsDistance;


    public static GameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    public BallManager Ball
    {
        get { return _ball; }
        private set { _ball = value; }
    }

    public float PlayerRacketSpeed
    {
        get { return _playerRacketSpeed; }
        private set { _playerRacketSpeed = value; }
    }

    public float RacketsDistance
    {
        get { return _racketsDistance; }
        private set { _racketsDistance = value; }
    }

    void Awake()
    {
        // Singleton behavior
        if (!_instance)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Compute the distance between the two rackets
        ComputeRacketsDistance();
        // No sound should be heared while in the main menu
        SoundManager.Instance.Mute();
        // Show the menu
        _mainMenu.gameObject.SetActive(true);
        // Start an AI game in background
        StartMockGame();
    }

    void Update()
    {
        UpdateInput();

        UpdateBall();
        UpdateRackets();
    }

    /// <summary>
    /// Create players and assign their controllers to the rackets
    /// </summary>
    /// <param name="leftPlayer">The left player controller</param>
    /// <param name="rightPlayer">The right player controller</param>
    void AssignPlayers(ControllerBase leftPlayer, ControllerBase rightPlayer)
    {
        // Generate new Ids
        int leftPlayerId = GetNewPlayerId();
        int rightPlayerId = GetNewPlayerId();

        // Create players
        _leftPlayer = new Player(leftPlayerId, leftPlayer);
        _rightPlayer = new Player(rightPlayerId, rightPlayer);

        // Assign controllers to rackets
        _leftRacket.SetController(_leftPlayer.Controller);
        _rightRacket.SetController(_rightPlayer.Controller);
    }

    /// <summary>
    /// Compute the horizontal distance between the two rackets and sets _racketsDistance.
    /// </summary>
    void ComputeRacketsDistance()
    {
        float distance = _leftRacket.transform.position.x - _rightRacket.transform.position.x;
        _racketsDistance = Mathf.Abs(distance);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Serve in a random clamped direction
    /// </summary>
    IEnumerator FirstServe()
    {
        // Set speed
        _ball.SetBall(_ballSpeed);
        // Set ball position
        _ball.transform.position = _middlePoint.position;

        // Random direction
        Vector3 randomDirection = BallManager.RandomDirection(_serviceHalfMaxAngle, _ball.transform);
        
        // Random side
        if(Random.Range(0,2) == 1)
        {
            randomDirection.x = -randomDirection.x;
        }

        // Wait and serve
        yield return new WaitForSeconds(_timeBetweenRounds);
        _ball.Serve(_middlePoint.position, randomDirection);
    }

    /// <summary>
    /// Gives an unnused id for a new player
    /// </summary>
    /// <returns></returns>
    int GetNewPlayerId()
    {
        return _playersIdsCounter++;
    }

    /// <summary>
    /// Init Game parameters : players, goals and scores
    /// </summary>
    /// <param name="leftPlayerController"></param>
    /// <param name="rightPlayerController"></param>
    void InitGame(ControllerBase leftPlayerController, ControllerBase rightPlayerController)
    {
        AssignPlayers(leftPlayerController, rightPlayerController);
        SetGoals();
        SetPlayersScoreTexts();
    }

    /// <summary>
    /// Show Pause menu and pauses game progression
    /// </summary>
    public void PauseGame()
    {
        // Only pause game if no other menu is shown
        if (!_mainMenu.gameObject.activeSelf && !_pauseMenu.gameObject.activeSelf)
        {
            Time.timeScale = 0f;
            _pauseMenu.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Player scored event. Score is updated and new round will start after a few seconds.
    /// Left goald Id mathces left player Id. If left Id is triggered, it's the right player who scored
    /// </summary>
    /// <param name="id">The id of the goal triggered</param>
    public void PlayerScored(int id)
    {
        // Update player score
        if (id == _leftPlayer.Id)
        {
            _rightPlayer.AddPoint();
        }
        else if (id == _rightPlayer.Id)
        {
            _leftPlayer.AddPoint();
        }
        else
        {
            Debug.LogError("Unknown player id found when setting score " + id);
        }

        // Update user interface
        UpdatePlayerScoreText(id);
        // Start a new round
        StartCoroutine(StartNewRound(id));
    }

    /// <summary>
    /// Close pause menu and continue playing
    /// </summary>
    public void ResumeGame()
    {
        // ResumeGame should only be called when the pause menu is open
        if (_pauseMenu.gameObject.activeSelf && !_mainMenu.gameObject.activeSelf)
        {
            _pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Stop the current game and shown the main menu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        // Close pause menu and show main menu
        _mainMenu.gameObject.SetActive(true);
        _pauseMenu.gameObject.SetActive(false);

        Time.timeScale = 1f;

        // The main menu will be shown and a mock game will start. Mute the sound
        SoundManager.Instance.Mute();
        StartMockGame();
    }

    /// <summary>
    /// Set the goals ids. Goals ids matches players'
    /// </summary>
    void SetGoals()
    {
        _leftGoal.SetId(_leftPlayer.Id);
        _rightGoal.SetId(_rightPlayer.Id);
    }

    /// <summary>
    /// Sets the score texts to the players' scores
    /// </summary>
    void SetPlayersScoreTexts()
    {
        _leftPlayerScoreText.text = _leftPlayer.Score.ToString();
        _rightPlayerScoreText.text = _rightPlayer.Score.ToString();
    }

    /// <summary>
    /// Start a mock game with 2 AIs
    /// </summary>
    void StartMockGame()
    {
        // Create controllers
        int aiId = -1;
        ControllerAI leftPlayerController = new ControllerAI(aiId, ControllerAI.Difficulty.Normal, _racketsDistance, _ball.transform);
        ControllerAI rightPlayerController = new ControllerAI(aiId, ControllerAI.Difficulty.Normal, _racketsDistance, _ball.transform);

        // Assign controllers to players and start the mock game
        InitGame(leftPlayerController, rightPlayerController);
        StartCoroutine(FirstServe());
    }

    /// <summary>
    /// Start a new game. Creates new players from given controllers, init everything then serve.
    /// </summary>
    /// <param name="leftPlayerController">The left player controller used</param>
    /// <param name="rightPlayerController">The right player controller used</param>
    public void StartNewGame(ControllerBase leftPlayerController, ControllerBase rightPlayerController)
    {
        InitGame(leftPlayerController, rightPlayerController);

        // The game will start unmute the sound
        SoundManager.Instance.UnMunte();
        StartCoroutine(FirstServe());
    }

    /// <summary>
    /// Start a new round afeter a brief delay
    /// </summary>
    /// <param name="id">The id of the player who will serve</param>
    /// <returns></returns>
    IEnumerator StartNewRound(int id)
    {
        // Wait before service
        yield return new WaitForSeconds(_timeBetweenRounds);

        Vector3 randomDirection = BallManager.RandomDirection(_serviceHalfMaxAngle, _ball.transform);
        // The player who won the last round "will serve". The ball direction should not be towars its goal
        if(id == _leftPlayer.Id)
        {
            randomDirection.x = -randomDirection.x;
        }
        else if(id != _leftPlayer.Id && id != _rightPlayer.Id)
        {
            Debug.LogError("Unknown player id found when serving " + id);
        }

        _ball.Serve(_middlePoint.position, randomDirection);
    }

    /// <summary>
    /// Updates the ball position using its manager
    /// </summary>
    void UpdateBall()
    {
        _ball.UpdateBallPosition();
    }
    
    /// <summary>
    /// Handles the keyboard
    /// </summary>
    void UpdateInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Updates a player's score text.
    /// The id used is the id of the opponent's goal.
    /// </summary>
    /// <param name="id">The id of the goal triggered</param>
    void UpdatePlayerScoreText(int id)
    {
        if (id == _leftPlayer.Id)
        {
            _rightPlayerScoreText.text = _rightPlayer.Score.ToString();
        }
        else if (id == _rightPlayer.Id)
        {
            _leftPlayerScoreText.text = _leftPlayer.Score.ToString();
        }
        else
        {
            Debug.LogError("Unknown player id found when setting score text " + id);
        }
    }
    /// <summary>
    /// Updates the rackets positions using their controllers
    /// </summary>
    void UpdateRackets()
    {
        _leftPlayer.Controller.ControlRacket(_leftRacket.transform);
        _rightPlayer.Controller.ControlRacket(_rightRacket.transform);
    }


}

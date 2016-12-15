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



    Player _leftPlayer;
    Player _rightPlayer;


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
        //// TODO: Debug
        //ControllerBase leftController = new ControllerHuman(0, ControllerBase.ControllerType.Human, _playerRacketSpeed);
        //ControllerBase rightController = new ControllerAI(1, ControllerAI.Difficulty.Normal, _ball.transform);

        //InitGame(leftController, rightController);
        //FirstServe();

        StartMockGame();
    }

    void Update()
    {
        UpdateBall();
        UpdateRackets();
    }

    void AssignPlayers(ControllerBase leftPlayer, ControllerBase rightPlayer)
    {
        int leftPlayerId = GetNewPlayerId();
        int rightPlayerId = GetNewPlayerId();

        _leftPlayer = new Player(leftPlayerId, leftPlayer);
        _rightPlayer = new Player(rightPlayerId, rightPlayer);

        _leftRacket.SetController(_leftPlayer.Controller);
        _rightRacket.SetController(_rightPlayer.Controller);
    }

    void FirstServe()
    {
        _ball.SetBall(_ballSpeed);

        // Random direction
        Vector3 randomDirection = BallManager.RandomDirection(_serviceHalfMaxAngle, _ball.transform);
        
        // Random side
        if(Random.Range(0,2) == 1)
        {
            randomDirection.x = -randomDirection.x;
        }

        _ball.Serve(_middlePoint.position, randomDirection);
    }

    int GetNewPlayerId()
    {
        return _playersIdsCounter++;
    }

    void InitGame(ControllerBase leftPlayerController, ControllerBase rightPlayerController)
    {
        AssignPlayers(leftPlayerController, rightPlayerController);
        SetGoals();
        SetPlayersScoreTexts();
    }

    /// <summary>
    /// Player scored event. Score is updated and new round will start after a few seconds.
    /// Left goald Id mathces left player Id. If left Id is triggered, it's the right player who scored
    /// </summary>
    /// <param name="id">The id of the goal triggered</param>
    public void PlayerScored(int id)
    {
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

        UpdatePlayerScoreText(id);
        StartCoroutine(StartNewRound(id));
    }

    /// <summary>
    /// Set the goals ids. Goals ids matches players'
    /// </summary>
    void SetGoals()
    {
        // TODO: Debug
        _leftGoal.SetId(_leftPlayer.Id);
        _rightGoal.SetId(_rightPlayer.Id);
    }

    void SetPlayersScoreTexts()
    {
        _leftPlayerScoreText.text = _leftPlayer.Score.ToString();
        _rightPlayerScoreText.text = _rightPlayer.Score.ToString();
    }

    void StartMockGame()
    {
        int aiId = -1;
        ControllerAI leftPlayerController = new ControllerAI(aiId, ControllerAI.Difficulty.Normal, _ball.transform);
        ControllerAI rightPlayerController = new ControllerAI(aiId, ControllerAI.Difficulty.Normal, _ball.transform);

        InitGame(leftPlayerController, rightPlayerController);
        FirstServe();
    }

    public void StartNewGame(ControllerBase leftPlayerController, ControllerBase rightPlayerController)
    {
        InitGame(leftPlayerController, rightPlayerController);
        FirstServe();
    }

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

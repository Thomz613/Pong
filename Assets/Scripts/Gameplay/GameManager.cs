using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton used as a simple engine to manage the game
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;


    [SerializeField]
    RacketManager _leftRacket;

    [SerializeField]
    RacketManager _rightRacket;

    [SerializeField]
    Transform _middlePoint;

    [SerializeField]
    BallManager _ball;

    // TODO: Debug
    ControllerBase _leftController;
    ControllerBase _rightController;

    public static GameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
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
        // TODO: Debug
        _leftController = new ControllerHuman(0, ControllerBase.ControllerType.Human, 20);
        _rightController = new ControllerAI(1, ControllerAI.Difficulty.Normal, _ball.transform);

        _leftRacket.SetController(_leftController);
        _rightRacket.SetController(_rightController);

        _ball.SetBall(30f);
        _ball.Serve(_middlePoint.position, BallManager.RandomDirection(45, _ball.transform));
    }

    void Update()
    {
        UpdateBall();
        UpdateRackets();
    }


    /// <summary>
    /// Updates the rackets positions using their controllers
    /// </summary>
    void UpdateRackets()
    {
        _leftController.ControlRacket(_leftRacket.transform);
        _rightController.ControlRacket(_rightRacket.transform);
    }

    /// <summary>
    /// Updates the ball position using its manager
    /// </summary>
    void UpdateBall()
    {
        _ball.UpdateBallPosition();
    }
}

using UnityEngine;
using System.Collections;

/// <summary>
/// Player model used to store its relative infos
/// </summary>
public class Player
{
    ControllerBase _controller;
    uint _score;

    int _id;

    public ControllerBase Controller
    {
        get { return _controller; }
        private set { _controller = value; }
    }

    public uint Score
    {
        get { return _score; }
        private set { _score = value; }
    }

    public int Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    public Player(int id, ControllerBase controller)
    {
        _id = id;
        _controller = controller;
        _score = 0;
    }

    /// <summary>
    /// Add a point to the player's score
    /// </summary>
    public void AddPoint()
    {
        ++_score;
    }
}

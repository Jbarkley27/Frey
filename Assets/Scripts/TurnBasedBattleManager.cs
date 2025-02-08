using UnityEngine;
using TMPro;
using DG.Tweening;

public class TurnBasedBattleManager: MonoBehaviour
{
    public static TurnBasedBattleManager instance;

    [SerializeField] private float _timeBetweenTurns = 1.0f;
    public enum TimeState
    {
        Stopped,
        Moving
    }
    public TimeState _currentTimeState = TimeState.Moving;
    public int _turnCount = 0;
    public TMP_Text _turnCountText;
    public TMP_Text _timeStateText;


   private void Awake()
    {
        // if there is already an instance of this object
        if (instance != null)
        {
            // log an error message
            Debug.LogError("Found an Turn Based Manager object, destroying new one");
            // destroy the new object
            Destroy(gameObject);
            // return from this method
            return;
        }
        // set the instance to this object
        instance = this;
        // don't destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _turnCount = 0;
        Invoke("SlowDownTime", 1);
        Debug.Log("Starting Battle");
        _turnCountText.DOText("Turn #: " + _turnCount, 0.4f, true, ScrambleMode.Uppercase);
    }



    // TIME HANDLING ---------------------------------------------------------
    private void SlowDownTime()
    {
        Debug.Log("Slowing down time");

        // set the current time state to pause
        _currentTimeState = TimeState.Stopped;

        // update the time state text
        _timeStateText.DOText("Time State: " + _currentTimeState.ToString(), 0.4f, true, ScrambleMode.Uppercase);

        // increment the turn count
        _turnCount++;
        _turnCountText.DOText("Turn #: " + _turnCount, 0.4f, true, ScrambleMode.Uppercase);

        // freeze movements of all combatants and environment
        EnemyManager.instance.CalculateAllEnemyIntentions();

        // wait for user input to resume time...
    }


    public void ResumeTime()
    {
        // set the current time state to normal
        Debug.Log("Resuming time");

        _currentTimeState = TimeState.Moving;

        // unfreeze movements of all combatants and environment
        EnemyManager.instance.ActivateAllEnemyIntentions();

        // update the time state text
        _timeStateText.DOText("Time State: " + _currentTimeState.ToString(), 0.4f, true, ScrambleMode.Uppercase);


        // invoke slow down time after 1 second
        Invoke("SlowDownTime", _timeBetweenTurns);
    }



    public bool IsTimeStopped()
    {
        return _currentTimeState == TimeState.Stopped;
    }
}
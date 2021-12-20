using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhacManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<WhacButton> _whacButtons;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _bestScoreText;
    [SerializeField] TextMeshProUGUI _timeText;

    [Header("Game Configuration")]
    [SerializeField] float _gameTime = 60;
    [SerializeField] float _roundDelay = 1;
    [SerializeField] int _minActiveButtons = 1;
    [SerializeField] int _maxActiveButtons = 3;

    int _activeButtons = 0;
    int _score = 0;
    int _bestScore = 0;
    bool _gameEnded = false;
    float _gameTimeLeft;
    Coroutine _timeTickCoroutine;

    private void OnEnable()
    {
        _whacButtons.ForEach(whacButton => {
            whacButton.OnHit.AddListener(_RegisterHit);
        });
        StartGame();
    }

    private void OnDisable()
    {
        _whacButtons.ForEach(whacButton => {
            whacButton.OnHit.RemoveListener(_RegisterHit);
        });
    }
    public void StartGame()
    {
        _gameEnded = false;
        _StartTimer();
        _ResetTable();
        _ResetScore();
        StartCoroutine(_ActivateButtons());
    }


    public void EndGame()
    {
        _gameEnded = true;
    }

    IEnumerator _ActivateButtons()
    {
        if (!_gameEnded)
        {
            yield return new WaitForSeconds(_roundDelay);

            int amountButtons = Random.Range(_minActiveButtons, _maxActiveButtons + 1);
            List<int> indexes = _RollIndex(amountButtons);

            indexes.ForEach(index => {
                _whacButtons[index].Activate();
                _activeButtons++;
            });
        }
    }

    List<int> _RollIndex(int amount)
    {
        List<int> indexes = new List<int>();
        int rolledIndex;

        while (indexes.Count < amount)
        {
            rolledIndex = Random.Range(0, _whacButtons.Count);
            if (!indexes.Contains(rolledIndex))
            {
                indexes.Add(rolledIndex);
            }
        }

        return indexes;
    }

    void _ResetTable()
    {
        _whacButtons.ForEach(whacButton => {
            whacButton.Deactivate();
        });
    }
    void _RegisterHit()
    {
        Debug.Log($"Button whacked");
        _score++;
        _activeButtons--;
        _PrintScore();
        _CheckBestScore();
        if (_activeButtons == 0)
        {
            StartCoroutine(_ActivateButtons());
        }
    }
    #region Score
    void _PrintScore()
    {
        _scoreText.text = _score.ToString();
    }

    void _CheckBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            _bestScoreText.text = _bestScore.ToString();
        }
    }

    void _ResetScore()
    {
        _score = 0;
        _activeButtons = 0;
        _PrintScore();
    }
    #endregion

    #region Timer
    void _StartTimer()
    {
        _ResetTimer();
        _timeTickCoroutine = StartCoroutine(_TimeTick());
    }

    IEnumerator _TimeTick()
    {
        float tick = 1f;
        WaitForSeconds wait = new WaitForSeconds(tick);
        float startTime = Time.time;

        while (!_gameEnded)
        {
            _gameTimeLeft = _gameTimeLeft - tick;
            _PrintTime();

            if (_gameTimeLeft <= 0)
            {
                EndGame();
            }

            yield return wait;
        }
    }
    void _ResetTimer()
    {
        if (_timeTickCoroutine != null)
            StopCoroutine(_timeTickCoroutine);

        _gameTimeLeft = _gameTime;
        _PrintTime();
    }

    void _PrintTime()
    {
        _timeText.text = _gameTimeLeft.ToString("0");
    }
    #endregion

}

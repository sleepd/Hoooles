using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] Text _scoreText2;
    [SerializeField] bool _timerEnabled = true;
    [SerializeField] GameObject restartUI;

    private int _score;
    private float _tickElapsed;
    private bool _gameoverTriggered;

    void Start()
    {
        UpdateScoreText();

        if (restartUI != null)
        {
            restartUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!_timerEnabled)
        {
            return;
        }

        _tickElapsed += Time.deltaTime;
        const float tickInterval = 0.1f;

        if (_tickElapsed >= tickInterval)
        {
            int ticksPassed = Mathf.FloorToInt(_tickElapsed / tickInterval);
            _score += ticksPassed;
            _tickElapsed -= ticksPassed * tickInterval;
            UpdateScoreText();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Gameover()
    {
        if (_gameoverTriggered)
        {
            return;
        }

        _gameoverTriggered = true;
        SetTimerEnabled(false);
        StartCoroutine(ShowRestartDelayed());
    }

    public void SetTimerEnabled(bool enabled)
    {
        _timerEnabled = enabled;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator ShowRestartDelayed()
    {
        yield return new WaitForSeconds(2f);

        if (restartUI != null)
        {
            restartUI.SetActive(true);
        }
    }

    private void UpdateScoreText()
    {
        if (_scoreText2 != null)
        {
            _scoreText2.text = $"Score: {_score}";
        }
    }
}

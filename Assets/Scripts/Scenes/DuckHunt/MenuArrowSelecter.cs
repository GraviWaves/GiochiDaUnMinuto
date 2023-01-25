using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuArrowSelecter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image[] arrows;
    [SerializeField] private Text topScore;

    [Space(15), Header("Parameters")]
    [SerializeField] private float menuEnableDelay;

    [Space(15), Header("Audio clips")]
    [SerializeField] private AudioClip BgmMenuTitleIntro;

    private int currentSelectedArrow;
    private bool menuIsEnabled;

    const int MOVE_INDEX_UP = -1;
    const int MOVE_INDEX_DOWN = 1;


    private void Awake()
    {
        if(arrows == null || arrows.Length == 0)
        {
            return;
        }

        currentSelectedArrow = 0;
        menuIsEnabled = false;
        topScore.text = GetTopScore();

        SetArrowsVisibility(-1);
        StartCoroutine(EnableMenu());
        AudioManager.Instance.PlayBgmOneShot(BgmMenuTitleIntro);
    }

    private void Update()
    {
        if (arrows == null || arrows.Length == 0 || !menuIsEnabled)
        {
            return;
        }

        bool upMovement = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool downMovement = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        bool startGame = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);

        if (!upMovement && !downMovement && !startGame)
        {
            return;
        }
        else if (startGame)
        {
            menuIsEnabled = false;
            LoadingManager.Instance.LoadScene(Enums.SceneName.DuckHunt_Game);
        }
        else if (upMovement)
        {
            currentSelectedArrow = SetCurrentSelection(MOVE_INDEX_UP);
        }
        else
        {
            currentSelectedArrow = SetCurrentSelection(MOVE_INDEX_DOWN);
        }

        SetArrowsVisibility(currentSelectedArrow);
    }

    private string GetTopScore()
    {
        return $"TOP SCORE = {PlayerPrefs.GetString(Definition.DuckHuntTopScoreKey, "")}";
    }

    private void SetArrowsVisibility(int arrowIndex)
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if(i == arrowIndex)
            {
                SetArrowVisibility(arrows[i], true);
                continue;
            }
            
            SetArrowVisibility(arrows[i], false);
        }
    }

    private void SetArrowVisibility(Image arrow, bool isVisible)
    {
        arrow.enabled = isVisible;
    }

    private int SetCurrentSelection(int movementIndex)
    {
        return Mathf.Clamp(currentSelectedArrow + movementIndex, 0, 1);
    }

    private IEnumerator EnableMenu()
    {
        yield return new WaitForSeconds(menuEnableDelay);

        currentSelectedArrow = 0;
        SetArrowVisibility(arrows[currentSelectedArrow], true);
        menuIsEnabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Track the Animation Component
    //track Animation Clips for Fade in/out
    //Functions that can receive animation events
    //Function to play fade in/out animations

    [SerializeField] private Animation _mainMenyAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;

    public Event.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStageChanged);
    }
    public void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
        Debug.LogWarning("Fadeout Complete.");
    }

    public void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
        Debug.LogWarning("Fadein Complete.");
        UIManager.Instance.SetDummyCameraActive(true);
    }

    void HandleGameStageChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        _mainMenyAnimator.Stop();
        _mainMenyAnimator.clip = _fadeInAnimation;
        _mainMenyAnimator.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);

        _mainMenyAnimator.Stop();
        _mainMenyAnimator.clip = _fadeOutAnimation;
        _mainMenyAnimator.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class MusicManager : Singleton<MusicManager>
{
    private FMOD.Studio.EventInstance FMODEventInstance;

    [SerializeField] FMODUnity.EventReference fmodEvent;

    [SerializeField]
    [Range(0f, 2f)]
    private float dangerLevel;

    [SerializeField]
    [Range(0, 1)]
    int playerState, combatEngaged, mainMenuTheme, loadingTheme, explorationTheme;

    protected override void Awake()
    {
        base.Awake();

        PlayerState.OnSwitchToHunter += ToHunter;
        PlayerState.OnSwitchToPrey += ToPrey;

        PlayerState.OnBattleEngaged += EngageBattle;
        PlayerState.OnBattleDisengaged += DisengageBattle;

        ResetBools();
    }

    private void OnDestroy()
    {
        PlayerState.OnSwitchToHunter -= ToHunter;
        PlayerState.OnSwitchToPrey -= ToPrey;

        PlayerState.OnBattleEngaged -= EngageBattle;
        PlayerState.OnBattleDisengaged -= DisengageBattle;
    }
    
    void Start()
    {
        FMODEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        FMODEventInstance.start();
    }

#if UNITY_EDITOR
    [SerializeField] bool testMode;
    void Update()
    {
        if (!testMode) return;

        FMODEventInstance.setParameterByName("IsHunter", playerState);
        FMODEventInstance.setParameterByName("IsCombat", combatEngaged);
        FMODEventInstance.setParameterByName("DangerProximity", dangerLevel);
        FMODEventInstance.setParameterByName("IsMainMenuTheme", mainMenuTheme);
        FMODEventInstance.setParameterByName("IsCaricamentoTheme", loadingTheme);
        FMODEventInstance.setParameterByName("IsEsplorazioneTheme", explorationTheme);
    }
#endif

    #region Events

    // IsHunter
    void ToHunter() { FMODEventInstance.setParameterByName("IsHunter", 1f); }
    void ToPrey() { FMODEventInstance.setParameterByName("IsHunter", 0f); }

    // IsCombat
    void EngageBattle() { FMODEventInstance.setParameterByName("IsCombat", 1f); }
    void DisengageBattle() { FMODEventInstance.setParameterByName("IsCombat", 0f); }

    #endregion

    public static void SetVictory(bool state)
    {
        _instance.FMODEventInstance.setParameterByName(GetThemeID(Theme.Victory), state ? 1f : 0f);
    }

    public static void SetDangerProximity(float interpolation)
    {
        if (PlayerState.IsInCombat)
        {
            _instance.FMODEventInstance.setParameterByName("DangerProximity", 0f);
            return;
        }
        _instance.FMODEventInstance.setParameterByName("DangerProximity", interpolation * 2f);
    }

    public enum Theme { MainMenu, LoadingScreen, Exploration, Victory }
    public static void SetMusicalTheme(Theme theme)
    {
        ResetTriggers();
        ApplyTheme(theme);

        
        void ResetTriggers()
        {
            _instance.FMODEventInstance.setParameterByName(GetThemeID(Theme.MainMenu), 0f);
            _instance.FMODEventInstance.setParameterByName(GetThemeID(Theme.LoadingScreen), 0f);
            _instance.FMODEventInstance.setParameterByName(GetThemeID(Theme.Exploration), 0f);
            _instance.FMODEventInstance.setParameterByName(GetThemeID(Theme.Victory), 0f);
        }

        void ApplyTheme(Theme theme)
        {
            _instance.FMODEventInstance.setParameterByName(GetThemeID(theme), 1f);
        }
    }

    static string GetThemeID(Theme theme)
    {
        switch (theme)
        {
            case Theme.MainMenu: return "IsMainMenuTheme";
            case Theme.LoadingScreen: return "IsCaricamentoTheme";
            case Theme.Exploration: return "IsEsplorazioneTheme";
            case Theme.Victory: return "Victory";
        }
        throw new System.NotImplementedException();
    }

    private void ResetBools()
    {
        FMODEventInstance.setParameterByName("IsCombat", 0f);
        FMODEventInstance.setParameterByName("IsHunter", 0f);
        FMODEventInstance.setParameterByName("Victory", 0f);
    }
}

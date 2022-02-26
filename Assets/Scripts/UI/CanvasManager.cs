using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Patterns;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] GameObject _controlsPanel;
    [SerializeField] Text _tarotText;

    public static Vector3 TarotTextScale { get { return _instance._tarotText.transform.localScale; } set { _instance._tarotText.transform.localScale = value; } }

    protected override void Awake()
    {
        base.Awake();
        ShowControlsPanel(_isShowingControls);
        _tarotText.text = "";
    }

    bool _isShowingControls = false;
    public static bool IsShowingControls { get { return _instance._isShowingControls; } }
    public static void ShowControlsPanel(bool state)
    {
        _instance._isShowingControls = state;
        _instance._controlsPanel.SetActive(state);
    }

    public static void SetTarotText(TarotId tarotId)
    {
        _instance._tarotText.text = tarotCaptions[tarotId];
    }

    public enum TarotId { Unassigned, DevilPrey, DevilHunter, FoolPrey, FoolHunter, LoversPrey, LoversHunter, MoonPrey, MoonHunter, SunPrey, SunHunter }

    private static Dictionary<TarotId, string> tarotCaptions = new Dictionary<TarotId, string>()
    {
        { TarotId.Unassigned,       "" },

        { TarotId.DevilPrey,       "Even in evil, find the opportunity to do the right thing.\nYou have control of your Shadow." },
        { TarotId.DevilHunter,     "Release your fears and troubles to unleash your potential." },

        { TarotId.FoolPrey,        "A leap of faith:\nErase all shadows" },
        { TarotId.FoolHunter,      "A leap of faith:\nErase all shadows" },

        { TarotId.LoversPrey,      "Your values, your beliefs, your philosophy.\nYou know what you stand for now..." },
        { TarotId.LoversHunter,    "To what extent do you Honor and accept Who you are?\nUnderstand what it Is in the other One which you also have." },

        { TarotId.MoonPrey,        "Remember what you have forgotten.\nBe considerate on this journey to your secrets." },
        { TarotId.MoonHunter,      "Be careful of your fears, obsessions and what you don't understand." },

        { TarotId.SunPrey,         "Clear your mind, so you know your way." },
        { TarotId.SunHunter,       "Look for what lies behind your victories." }
    };
}

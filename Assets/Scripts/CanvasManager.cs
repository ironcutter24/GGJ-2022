using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Patterns;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] Text _tarotText;

    public static Vector3 TarotTextScale { get { return _instance._tarotText.transform.localScale; } set { _instance._tarotText.transform.localScale = value; } }

    protected override void Awake()
    {
        base.Awake();
        _tarotText.text = "";
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
        { TarotId.DevilHunter,     "Offer your fears and troubles to free yourself from the shackles of unhealthy behavior." },

        { TarotId.FoolPrey,        "A leap of faith:\nDefeat all foes" },
        { TarotId.FoolHunter,      "A leap of faith:\nDefeat all foes" },

        { TarotId.LoversPrey,      "Your values, your beliefs, your philosophy.\nYou know what you stand for now..." },
        { TarotId.LoversHunter,    "To what extent do you honor and accept who you are?\nYou have to understand there is in the other,\nwhich you also have." },

        { TarotId.MoonPrey,        "Remember what you have forgotten.\nBe considerate on this journey to your secrets." },
        { TarotId.MoonHunter,      "Be careful of what you don't understand, fears and obsessions." },

        { TarotId.SunPrey,         "Know your way, get to clarity." },
        { TarotId.SunHunter,       "Appreciate who you are.\nDon't let pleasure take your eyes off the truth." }
    };
}

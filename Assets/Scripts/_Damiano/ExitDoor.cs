using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Patterns;

public class ExitDoor : Singleton<ExitDoor>
{
    [Header("Components")]
    [SerializeField] MeshRenderer meshRend;
    [SerializeField] BoxCollider invisibleWall;

    [Header("Scene specific")]
    [SerializeField] GameObject tarotPrefab;
    [SerializeField] CanvasManager.TarotId captionPrey;
    [SerializeField] CanvasManager.TarotId captionHunter;
    [SerializeField] string nextScene;

    bool isTarotInverted = false;

    GameObject _tarot;

    private void Start()
    {
        MusicManager.SetVictory(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _isOpen)
        {
            StartCoroutine(_ShowTarot());
        }
    }

    public void StartTarotAnimation()
    {
        StartCoroutine(_ShowTarot());
    }

    bool _isOpen = false;
    public void Open()
    {
        meshRend.enabled = false;
        invisibleWall.enabled = false;
        _isOpen = true;

        MusicManager.SetVictory(true);
    }

    IEnumerator _ShowTarot()
    {
        _tarot = Instantiate(tarotPrefab, Camera.main.transform);
        _tarot.transform.localPosition = new Vector3(0f, .02f, .2f);

        if(PlayerState.Instance != null)
        {
            Controller3D.DisableController();
            isTarotInverted = PlayerState.HasBeenMostlyHunter;
        }

        CanvasManager.SetTarotText(isTarotInverted ? captionHunter : captionPrey);

        float duration = 1.6f;
        float speed = 1 / duration;
        float interpolation = 0f;

        while (interpolation < 1f)
        {
            UpdateTransition(interpolation);
            interpolation += speed * Time.deltaTime;
            yield return null;
        }
        interpolation = 1f;
        UpdateTransition(interpolation);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        Destroy(_tarot.gameObject);
        CanvasManager.SetTarotText(CanvasManager.TarotId.Unassigned);

        GameManager.LoadScene(nextScene);
    }

    void UpdateTransition(float interpolation)
    {
        _tarot.transform.localScale = Vector3.one * interpolation * 2f;

        float deltaRotation = interpolation * 540f;
        _tarot.transform.localRotation = Quaternion.Euler(0f, _tarot.transform.localRotation.y - deltaRotation, isTarotInverted ? 180f : 0f);

        CanvasManager.TarotTextScale = Vector3.one * Mathf.Clamp(interpolation - .5f, 0f, 1f) * 2;
    }
}

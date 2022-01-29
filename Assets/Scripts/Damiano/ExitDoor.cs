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
    [SerializeField] Text textbox;

    [Header("Scene specific")]
    [SerializeField] GameObject tarotPrefab;
    [SerializeField] string nextScene;

    bool isTarotInverted;

    GameObject _tarot;

    private void Start()
    {
        textbox.enabled = false;
        //StartCoroutine(_ShowTarot());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(_ShowTarot());
        }
    }

    public void Open()
    {
        meshRend.enabled = false;
        invisibleWall.enabled = false;
    }

    IEnumerator _ShowTarot()
    {
        _tarot = Instantiate(tarotPrefab, Camera.main.transform);
        _tarot.transform.localPosition = new Vector3(0f, .02f, .2f);

        isTarotInverted = PlayerState.HasBeenMostlyHunter;

        textbox.text = _tarot.GetComponent<TarotData>().GetCaption(isTarotInverted);
        textbox.enabled = true;

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

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        // Change scene
    }

    void UpdateTransition(float interpolation)
    {
        _tarot.transform.localScale = Vector3.one * interpolation * 2f;

        float deltaRotation = interpolation * 540f;
        _tarot.transform.localRotation = Quaternion.Euler(0f, _tarot.transform.localRotation.y - deltaRotation, isTarotInverted ? 180f : 0f);

        textbox.transform.localScale = Vector3.one * Mathf.Clamp(interpolation - .5f, 0f, 1f) * 2;
    }
}

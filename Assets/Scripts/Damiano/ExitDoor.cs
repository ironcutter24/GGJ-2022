using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class ExitDoor : Singleton<ExitDoor>
{
    [Header("Components")]
    [SerializeField] MeshRenderer meshRend;
    [SerializeField] BoxCollider invisibleWall;

    [Header("Scene specific")]
    [SerializeField] GameObject tarotPrefab;
    [SerializeField] string nextScene;

    GameObject _tarot;

    private void Start()
    {
        _tarot = Instantiate(tarotPrefab, Camera.main.transform);
        _tarot.transform.localPosition = new Vector3(0f, 0f, 2f);
        StartCoroutine(_ShowTarot());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Victory!
        }
    }

    public void OpenDoor()
    {
        meshRend.enabled = false;
        invisibleWall.enabled = false;
    }

    IEnumerator _ShowTarot()
    {
        float duration = 1f;
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

        
    }

    void UpdateTransition(float interpolation)
    {
        _tarot.transform.localScale = Vector3.one * interpolation * 2;
    }
}

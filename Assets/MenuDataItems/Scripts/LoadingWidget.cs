using System.Collections;
using UnityEngine;

public class LoadingWidget : MonoBehaviour
{
    [SerializeField] private Transform _imageTransform;
    [SerializeField] private GameObject _panelObject;
    [SerializeField] private float _animationSpeed = 2.5f;

    private bool _animate = false;

    private void Awake()
    {
        _panelObject.SetActive(false);
    }

    public void ShowLoading()
    {
        if(_animate)
        {
            return;
        }
        _panelObject.SetActive(true);
        _animate = true;
        StartCoroutine(LoadingAnimationRoutine());
    }

    public void HideLoading()
    {
        _animate = false;
        _panelObject.SetActive(false);
    }


    private IEnumerator LoadingAnimationRoutine()
    {
        while(_animate)
        {
            _imageTransform.Rotate(_imageTransform.forward, _animationSpeed * Time.deltaTime);
            yield return 0;
        }
    }

}

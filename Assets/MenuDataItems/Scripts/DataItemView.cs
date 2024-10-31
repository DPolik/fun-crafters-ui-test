using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataItemView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _badgeImage;
    [SerializeField] private Sprite[] _categorySprites;
    [SerializeField] private Image _glowImage;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _indexText;


    private void Awake()
    {
        Hide();
    }

    public void Setup(int categorySpriteIndex, bool glow, string description, string index)
    {
        if(_categorySprites.Length < categorySpriteIndex+1)
        {
            Debug.LogError($"Could not find sprite for index {categorySpriteIndex}");
            return;
        }
        _badgeImage.sprite = _categorySprites[categorySpriteIndex];
        _glowImage.enabled = glow;
        _descriptionText.text = description;
        _indexText.text = index;
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }

}

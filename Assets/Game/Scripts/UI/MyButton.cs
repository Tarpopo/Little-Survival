using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SquareDino;
using DG.Tweening;

[RequireComponent(typeof(Button), typeof(Image))]
public class MyButton : MonoBehaviour
{
    public event System.Action OnClick;

    [SerializeField] private float scalerDuration = 0.5f;

    private Tween scalerTween;

    private bool interactable;
    private Image image;

    protected Button button;

    public void Show(bool enable)
    {
        if (scalerTween != null) scalerTween.Kill();

        if (enable)
        {
            scalerTween = transform.DOScale(1.25f, scalerDuration).OnComplete(() => 
            scalerTween = transform.DOScale(1f, scalerDuration));
        }
        else
        {
            scalerTween = transform.DOScale(1.25f, scalerDuration).OnComplete(() =>
            scalerTween = transform.DOScale(0f, scalerDuration));
        }
    }
    
    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        
        interactable = true;
        
        button.onClick.AddListener(ClickButton);
    }

    protected virtual void ClickButton()
    {
        if (!interactable) return;
        
        OnClick?.Invoke();
        MyVibration.Haptic(MyHapticTypes.LightImpact);
    }

    public void SetInteractable(bool value)
    {
        if (image == null) image = GetComponent<Image>();

        interactable = value;

        if (image != null)
        {
            image.color = interactable ? Color.white : Color.gray;
        }
    }
}
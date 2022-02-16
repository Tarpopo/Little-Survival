using SquareDino;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VibroButton : MyButton
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;

    protected override void ClickButton()
    {
        base.ClickButton();
        
        Settings.VibrationEnabled = !Settings.VibrationEnabled;

        if (Settings.VibrationEnabled)
            MyVibration.Haptic(MyHapticTypes.LightImpact);
        
        ChangeImage();
    }
    
    private void ChangeImage()
    {
        targetImage.sprite = Settings.VibrationEnabled ? onImage : offImage;
    }

    private void Start()
    {
        ChangeImage();
    }
}
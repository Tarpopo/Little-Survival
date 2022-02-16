using SquareDino.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace SquareDino.Scripts.Editor
{
    [CustomEditor(typeof(MyPlayerSettings))]
    public class MyPlayerSettingsEditor : UnityEditor.Editor
    {
        // private void SetAndroidAdaptiveIcons(Texture2D[][] textures)
        // {
        //     var platform = BuildTargetGroup.Android;
        //     var kind = UnityEditor.Android.AndroidPlatformIconKind.Adaptive;
        //
        //     var icons = PlayerSettings.GetPlatformIcons(platform, kind);
        //
        //     //Assign textures to each available icon slot.
        //     for (var i = 0; i < icons.Length; i++)
        //     {
        //         icons[i].SetTextures(textures[i]);    
        //     }
        //     PlayerSettings.SetPlatformIcons(platform, kind, icons);
        // }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var settings = target as MyPlayerSettings;
            if (settings == null) return;

            if (!GUILayout.Button("Apply Settings")) return;
            
            // Texture2D[] icon = {settings.icon};
            // PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, icon);
            
            // Texture2D[][] iconAdaptiveArr =
            // {
            //     new[] {settings.iconBackground},
            //     new[] {settings.iconForeground}
            // };
            //
            // SetAndroidAdaptiveIcons(iconAdaptiveArr);
            
            PlayerSettings.companyName = "SquareDino";
            // PlayerSettings.productName = settings.gameName;
            PlayerSettings.bundleVersion = "0.0." + settings.buildStoreVersion;
            PlayerSettings.Android.bundleVersionCode = settings.buildStoreVersion;
            PlayerSettings.iOS.buildNumber = settings.buildStoreVersion.ToString();
            if (settings.applicationIdentifier != "")
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, settings.applicationIdentifier);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, settings.applicationIdentifier);
            }

            PlayerSettings.SplashScreen.show = true;
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SplashScreen.unityLogoStyle = PlayerSettings.SplashScreen.UnityLogoStyle.DarkOnLight;
            PlayerSettings.SplashScreen.backgroundColor = Color.red;
            var logos = new PlayerSettings.SplashScreenLogo[1];
            var splashScreenLogo = PlayerSettings.SplashScreenLogo.Create(2f, settings.logo);
            logos[0] = splashScreenLogo;
            PlayerSettings.SplashScreen.logos = logos;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        }
    }
}

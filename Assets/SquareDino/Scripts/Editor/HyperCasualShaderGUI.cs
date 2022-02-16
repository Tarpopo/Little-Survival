using System;
using UnityEditor;
using UnityEngine;

public class HyperCasualShaderGUI : ShaderGUI
{
    public enum BlendMode
    {
        Opaque,
        Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
    }
    
    private static class Styles
    {
        public static GUIContent uvSetLabel = EditorGUIUtility.TrTextContent("UV Set");

        public static GUIContent albedoText = EditorGUIUtility.TrTextContent("Albedo", "Albedo (RGB) and Transparency (A)");
        public static GUIContent albedoHColorText = EditorGUIUtility.TrTextContent("Highlight Color", "Albedo (RGB) and Transparency (A)");
        public static GUIContent albedoSColorText = EditorGUIUtility.TrTextContent("Shadow Color", "Albedo (RGB) and Transparency (A)");
        public static GUIContent alphaCutoffText = EditorGUIUtility.TrTextContent("Alpha Cutoff", "Threshold for alpha cutoff");
        public static GUIContent specularMapText = EditorGUIUtility.TrTextContent("Specular", "Specular (RGB) and Smoothness (A)");
        public static GUIContent metallicMapText = EditorGUIUtility.TrTextContent("Metallic", "Metallic (R) and Smoothness (A)");
        public static GUIContent smoothnessText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness value");
        public static GUIContent smoothnessScaleText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness scale factor");
        public static GUIContent smoothnessMapChannelText = EditorGUIUtility.TrTextContent("Source", "Smoothness texture and channel");
        public static GUIContent highlightsText = EditorGUIUtility.TrTextContent("Specular Highlights", "Specular Highlights");
        public static GUIContent reflectionsText = EditorGUIUtility.TrTextContent("Reflections", "Glossy Reflections");
        public static GUIContent emissionText = EditorGUIUtility.TrTextContent("Color", "Emission (RGB)");

        public static string primaryMapsText = "Main Maps";
        public static string secondaryMapsText = "Secondary Maps";
        public static string forwardText = "Forward Rendering Options";
        public static string renderingMode = "Rendering Mode";
        public static string advancedText = "Advanced Options";
        public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
    }
    
    MaterialProperty blendMode = null;
    MaterialProperty flashMode = null;
    MaterialProperty pureFlashMode = null;
    MaterialProperty flashAmount = null;
    MaterialProperty flashColor = null;
    MaterialProperty albedoMap = null;
    MaterialProperty albedoHighligntMap = null;
    MaterialProperty albedoShadowMap = null;
    MaterialProperty albedoColor = null;
    MaterialProperty emissionColorForRendering = null;
    MaterialProperty emissionMap = null;
    MaterialProperty threshould = null;
    MaterialProperty smoothing = null;
    
    MaterialEditor m_MaterialEditor;
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        //base.OnGUI(materialEditor, properties);
        FindProperties(props);
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;
        ShaderPropertiesGUI(material);
    }

    public void FindProperties(MaterialProperty[] props)
    {
        blendMode = FindProperty("_Mode", props);
        albedoMap = FindProperty("_MainTex", props);
        albedoColor = FindProperty("_Color", props);
        albedoHighligntMap = FindProperty("_HColor", props);
        albedoShadowMap = FindProperty("_SColor", props);
        emissionColorForRendering = FindProperty("_EmissionColor", props);
        emissionMap = FindProperty("_EmissionMap", props);
        flashMode = FindProperty("_FlashMode", props);
        pureFlashMode = FindProperty("_FlashPureMode", props);
        flashAmount = FindProperty("_FlashAmount", props);
        flashColor = FindProperty("_FlashColor", props);
        threshould = FindProperty("_RampThreshold", props);
        smoothing = FindProperty("_RampSmoothing", props);
    }

    private void ShaderPropertiesGUI(Material material)
    {
        EditorGUIUtility.labelWidth = 0f;
        
        bool blendModeChanged = false;
        bool flashMode = false;
        
        EditorGUI.BeginChangeCheck();
        {
            blendModeChanged = BlendModePopup();
            
            // Primary properties
            GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
            DoAlbedoArea(material);
            GUILayout.Space(5);
            GUILayout.Label("Emission", EditorStyles.boldLabel);
            DoEmissionArea(material);;
            GUILayout.Space(5);
            GUILayout.Label("Flash Mode", EditorStyles.boldLabel);
            flashMode = DoFlashMode(material);
            GUILayout.Space(5);
            m_MaterialEditor.RangeProperty(threshould, "Threshold");
            m_MaterialEditor.RangeProperty(smoothing, "Smoothing");
            GUILayout.Space(5);
            
            m_MaterialEditor.RenderQueueField();
        }
        if (EditorGUI.EndChangeCheck())
        {
            ChangeFlashMode(material, flashMode);
            
            foreach (var obj in blendMode.targets)
                MaterialChanged((Material)obj, blendModeChanged);
        }

        m_MaterialEditor.EnableInstancingField();
        m_MaterialEditor.DoubleSidedGIField();
    }

    private void DoEmissionArea(Material material)
    {
        if (m_MaterialEditor.EmissionEnabledProperty())
        {
            bool hadEmissionTexture = emissionMap.textureValue != null;

            // Texture and HDR color controls
            //m_MaterialEditor.TexturePropertyWithHDRColor(Styles.emissionText, emissionMap, emissionColorForRendering, false);
            m_MaterialEditor.ColorProperty(emissionColorForRendering, "Emission");

            // If texture was assigned and color was black set color to white
            float brightness = emissionColorForRendering.colorValue.maxColorComponent;
            if (emissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
                emissionColorForRendering.colorValue = Color.white;

            // change the GI flag and fix it up with emissive as black if necessary
            m_MaterialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
        }
    }

    static void MaterialChanged(Material material, bool overrideRenderQueue)
    {
        SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"), overrideRenderQueue);
        SetMaterialKeywords(material);
    }
    
    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode, bool overrideRenderQueue)
    {
        int minRenderQueue = -1;
        int maxRenderQueue = 5000;
        int defaultRenderQueue = -1;
        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                minRenderQueue = -1;
                maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
                defaultRenderQueue = -1;
                break;
            case BlendMode.Fade:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                minRenderQueue = (int)UnityEngine.Rendering.RenderQueue.GeometryLast + 1;
                maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay - 1;
                defaultRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case BlendMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                minRenderQueue = (int)UnityEngine.Rendering.RenderQueue.GeometryLast + 1;
                maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay - 1;
                defaultRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }

        if (overrideRenderQueue || material.renderQueue < minRenderQueue || material.renderQueue > maxRenderQueue)
        {
            if (!overrideRenderQueue)
                Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Render queue value outside of the allowed range ({0} - {1}) for selected Blend mode, resetting render queue to default", minRenderQueue, maxRenderQueue);
            material.renderQueue = defaultRenderQueue;
        }
    }
    
    static void SetMaterialKeywords(Material material)
    {
        MaterialEditor.FixupEmissiveFlag(material);
        
        bool shouldEmissionBeEnabled = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
        SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);
    }
    
    static void SetKeyword(Material m, string keyword, bool state)
    {
        if (state)
            m.EnableKeyword(keyword);
        else
            m.DisableKeyword(keyword);
    }

    private void DoAlbedoArea(Material material)
    {   
        m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
        m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);

        //m_MaterialEditor.ColorProperty(albedoColor, "Main Color");
        m_MaterialEditor.ColorProperty(albedoHighligntMap, "Highlight Color");
        m_MaterialEditor.ColorProperty(albedoShadowMap, "Shadow Color");
    }

    private bool DoFlashMode(Material material)
    {
        EditorGUI.showMixedValue = blendMode.hasMixedValue;
        
        flashMode.floatValue = EditorGUILayout.Toggle("Flash Mode", Math.Abs(flashMode.floatValue) > 0.01f) ? 1f : 0f;
        bool isFlashModeOn = (Math.Abs(flashMode.floatValue) > 0.01f);
        bool isPureFlashMode = false;
        
        if (!isFlashModeOn) return false;

        EditorGUI.BeginChangeCheck();
        {            
            pureFlashMode.floatValue =
                EditorGUILayout.Toggle("Pure Flash Mode", Math.Abs(pureFlashMode.floatValue) > 0.01f) ? 1f : 0f;
            
            isPureFlashMode = DoPureFlashMode();
        }
        if (EditorGUI.EndChangeCheck())
        {
            ChangePureFlashMode(material, isPureFlashMode);
        }

        m_MaterialEditor.RangeProperty(flashAmount, "Flash Amount");
        m_MaterialEditor.ColorProperty(flashColor, "Flash Color");
        return true;
    }

    private bool DoPureFlashMode()
    {
        return pureFlashMode.floatValue > 0.01f;
    }

    private void ChangeFlashMode(Material material, bool value)
    {
        SetKeyword(material, "_FLASHMODE", value);

        if (!value)
        {
            ChangePureFlashMode(material, false);
            pureFlashMode.floatValue = 0f;
        }
    }

    private void ChangePureFlashMode(Material material, bool value)
    {
        SetKeyword(material, "_FLASHMODEPURECOLOR", value);
    }
    
    bool BlendModePopup()
    {
        EditorGUI.showMixedValue = blendMode.hasMixedValue;
        var mode = (BlendMode)blendMode.floatValue;

        EditorGUI.BeginChangeCheck();
        mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
        bool result = EditorGUI.EndChangeCheck();
        if (result)
        {
            m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
            blendMode.floatValue = (float)mode;
        }

        EditorGUI.showMixedValue = false;

        return result;
    }
}

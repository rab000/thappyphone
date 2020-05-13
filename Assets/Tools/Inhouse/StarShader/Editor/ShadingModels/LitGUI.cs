using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace StarEditor
{
    public static class LitGUI
    {
        public static class Styles
        {
            public static GUIContent mraMapText =new GUIContent("MRA (Metal,Rough,AO)", "Metallic (R) Roughness (G) AO (B)");
            public static GUIContent metallicText = new GUIContent("Metallic", "Metallic scale factor");
            public static GUIContent metallicMapScaleText = new GUIContent("Metallic (R)", "Metallic (R) scale factor");
            public static GUIContent roughnessText = new GUIContent("Roughness", "Roughness scale factor");
            public static GUIContent roughnessMapScaleText = new GUIContent("Roughness (G)", "Roughness (G) scale factor");
            public static GUIContent occlusionStrengthText = new GUIContent("Occlusion Strength", "Occlusion (B) scale factor");
            public static GUIContent highlightsText = new GUIContent("Specular Highlights",
                "When enabled, the Material reflects the shine from direct lighting.");

            public static GUIContent reflectionsText =
                new GUIContent("Environment Reflections",
                    "When enabled, the Material samples reflections from the nearest Reflection Probes or Lighting Probe.");
        }

        public struct LitProperties
        {
            // Surface Input Props
            public MaterialProperty metallic;
            public MaterialProperty bumpMapProp;
            public MaterialProperty bumpScaleProp;
            public MaterialProperty occlusionStrength;

            public MaterialProperty mraMap;
            public MaterialProperty metallicMapScale;
            public MaterialProperty roughness;
            public MaterialProperty roughnessMapScale;

            // Advanced Props
            public MaterialProperty highlights;
            public MaterialProperty reflections;

            public LitProperties(MaterialProperty[] properties)
            {
                // Surface Input Props
                mraMap = BaseShaderGUI.FindProperty("_MRAMap", properties);
                metallic = BaseShaderGUI.FindProperty("_Metallic", properties);
                metallicMapScale = BaseShaderGUI.FindProperty("_MetallicMapScale", properties);
                roughness = BaseShaderGUI.FindProperty("_Roughness", properties, false);
                roughnessMapScale = BaseShaderGUI.FindProperty("_RoughnessMapScale", properties, false);
                bumpMapProp = BaseShaderGUI.FindProperty("_BumpMap", properties, false);
                bumpScaleProp = BaseShaderGUI.FindProperty("_BumpScale", properties, false);
                occlusionStrength = BaseShaderGUI.FindProperty("_OcclusionStrength", properties, false);
                // Advanced Props
                highlights = BaseShaderGUI.FindProperty("_SpecularHighlights", properties, false);
                reflections = BaseShaderGUI.FindProperty("_EnvironmentReflections", properties, false);
            }
        }

        public static void Inputs(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            DoMRAArea(properties, materialEditor, material);
            BaseShaderGUI.DrawNormalArea(materialEditor, properties.bumpMapProp, properties.bumpScaleProp);
        }

        public static void DoMRAArea(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            // show MRA (metallic, roughness, AO)
            materialEditor.TexturePropertySingleLine(Styles.mraMapText, properties.mraMap);
            bool enableMRAMap = properties.mraMap.textureValue != null;
            int indentLevel = MaterialEditor.kMiniTextureFieldLabelIndentLevel;
            EditorGUI.indentLevel += indentLevel;
            {
                if (enableMRAMap)
                {
                    materialEditor.ShaderProperty(properties.metallicMapScale, Styles.metallicMapScaleText);
                    materialEditor.ShaderProperty(properties.roughnessMapScale, Styles.roughnessMapScaleText);
                }
                else
                {
                    materialEditor.ShaderProperty(properties.metallic, Styles.metallicText);
                    materialEditor.ShaderProperty(properties.roughness, Styles.roughnessText);
                }
                materialEditor.ShaderProperty(properties.occlusionStrength, Styles.occlusionStrengthText);
            }
            EditorGUI.indentLevel -= indentLevel;
        }

        public static void SetMaterialKeywords(Material material)
        {
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
           
            CoreUtils.SetKeyword(material, "_MRAMAP", material.GetTexture("_MRAMap"));

            if (material.HasProperty("_SpecularHighlights"))
                CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF",
                    material.GetFloat("_SpecularHighlights") == 0.0f);
            if (material.HasProperty("_EnvironmentReflections"))
                CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF",
                    material.GetFloat("_EnvironmentReflections") == 0.0f);
        }
    }
}

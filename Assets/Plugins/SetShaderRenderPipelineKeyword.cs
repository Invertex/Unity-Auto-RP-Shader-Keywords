using UnityEngine;
using UnityEngine.Rendering;

namespace Invertex.Unity.RP
{
    [DefaultExecutionOrder(1000)]
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class SetShaderRenderPipelineKeyword
    {
        public static readonly string KEYWORD_URP = "RP_URP";
        public static readonly string KEYWORD_HDRP = "RP_HDRP";
        const string ACTIVE = "_ACTIVE";
        const string INSTALLED = "_INSTALLED";

        public static void ChangeRenderPipelineKeyword(string rpKeyword)
        {
            DisableActiveRenderPipelineKeywords();
            Shader.EnableKeyword(rpKeyword + ACTIVE);
        }

        public static void DisableActiveRenderPipelineKeywords()
        {
            Shader.DisableKeyword(KEYWORD_URP + ACTIVE);
            Shader.DisableKeyword(KEYWORD_HDRP + ACTIVE);
        }

        private static void RenderPipelineChanged()
        {
            var currentRP = GraphicsSettings.currentRenderPipeline;
            if (currentRP == null)
            {
                DisableActiveRenderPipelineKeywords();
                return;
            }

            var curPipeline = currentRP.name.ToLower();

            if (curPipeline.Contains("universal"))
            { ChangeRenderPipelineKeyword(KEYWORD_URP); }
            else if (curPipeline.Contains("high definition"))
            { ChangeRenderPipelineKeyword(KEYWORD_HDRP); }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void SetCurrentPipelineKeyword()
        {
            var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();

            Shader.DisableKeyword(KEYWORD_URP + INSTALLED);
            Shader.DisableKeyword(KEYWORD_HDRP + INSTALLED);

            foreach (var package in packages)
            {
                if (package.name.ToLower().Contains("universal"))
                {
                    Shader.EnableKeyword(KEYWORD_URP + INSTALLED);
                }
                if (package.name.ToLower().Contains("high-definition"))
                {
                    Shader.EnableKeyword(KEYWORD_HDRP + INSTALLED);
                }
            }

            RenderPipelineChanged();

            #if UNITY_2021_1_OR_NEWER
                RenderPipelineManager.activeRenderPipelineTypeChanged += RenderPipelineChanged;
            #endif
        }

        /// <summary>
        /// Automatically sets a global shader keyword for the currently loaded pipeline
        /// allowing your shaders to conditionally compile based on these keywords, avoiding reference errors.
        /// </summary>
        static SetShaderRenderPipelineKeyword()
        {
            SetCurrentPipelineKeyword();
        }
    }
}
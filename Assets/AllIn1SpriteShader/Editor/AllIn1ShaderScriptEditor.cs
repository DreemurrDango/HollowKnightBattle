#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
    [CustomEditor(typeof(AllIn1Shader)), CanEditMultipleObjects]
    public class AllIn1ShaderScriptEditor : UnityEditor.Editor
    {
        private bool showUrpWarning = false;
        private double warningTime = 0f;
        private SerializedProperty m_NormalStrength, m_NormalSmoothing;
    
        private enum ImageType
        {
            ShowImage,
            HideInComponent,
            HideEverywhere
        }
        private ImageType imageType;

        private void OnEnable()
        {
            m_NormalStrength = serializedObject.FindProperty("normalStrength");
            m_NormalSmoothing = serializedObject.FindProperty("normalSmoothing");
        }

        public override void OnInspectorGUI()
        {
            ChooseAndDisplayAssetImage();

            AllIn1Shader myScript = (AllIn1Shader)target;

            SetCurrentShaderType(myScript);

            if (GUILayout.Button("Deactivate All Effects"))
            {
                for (int i = 0; i < targets.Length; i++) ((AllIn1Shader)targets[i]).ClearAllKeywords();
                AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Deactivated All Effects");
            }


            if (GUILayout.Button("New Clean Material"))
            {
                bool successOperation = true;
                for (int i = 0; i < targets.Length; i++)
                {
                    successOperation &= ((AllIn1Shader)targets[i]).TryCreateNew();
                }
                if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Clean Material");
            }


            if (GUILayout.Button("Create New Material With Same Properties (SEE DOC)"))
            {
                bool successOperation = true;
                for (int i = 0; i < targets.Length; i++)
                {
                    successOperation &= ((AllIn1Shader)targets[i]).MakeCopy();
                }
                if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Copy Created");
            }

            if (GUILayout.Button("Save Material To Folder (SEE DOC)"))
            {
                bool successOperation = true;
                for(int i = 0; i < targets.Length; i++)
                {
                    successOperation &= ((AllIn1Shader) targets[i]).SaveMaterial();
                }
                if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Material Saved");
            }

            if (GUILayout.Button("Apply Material To All Children"))
            {
                bool successOperation = true;
                for(int i = 0; i < targets.Length; i++)
                {
                    successOperation &= ((AllIn1Shader) targets[i]).ApplyMaterialToHierarchy();
                }
                if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Material Applied To Children");
                else EditorUtility.DisplayDialog("No children found", "All In 1 Shader component couldn't find any children to this GameObject (" + targets[0].name + ")", "Ok");
            }

            if (myScript.shaderTypes != AllIn1Shader.ShaderTypes.Urp2dRenderer)
            {
                if (GUILayout.Button("Render Material To Image"))
                {
                    bool successOperation = true;
                    for(int i = 0; i < targets.Length; i++)
                    {
                        successOperation &= ((AllIn1Shader) targets[i]).RenderToImage();
                    }
                    if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Material Rendered To Image");
                }
            }

            bool isUrp = false;
            Shader temp = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
            if (temp != null) isUrp = true;
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Change Shader Variant:", GUILayout.MaxWidth(140));
                int previousShaderType = (int)myScript.shaderTypes;
                myScript.shaderTypes = (AllIn1Shader.ShaderTypes)EditorGUILayout.EnumPopup(myScript.shaderTypes);
                if (previousShaderType != (int)myScript.shaderTypes)
                {
                    for (int i = 0; i < targets.Length; i++) ((AllIn1Shader)targets[i]).CheckIfValidTarget();
                    if (myScript == null) return;
                    if (isUrp || myScript.shaderTypes != AllIn1Shader.ShaderTypes.Urp2dRenderer)
                    {
                        AllIn1ShaderWindow.SceneViewNotificationAndLog(myScript.gameObject.name + " shader variant has been changed to: " + myScript.shaderTypes);
                        myScript.SetSceneDirty();

                        Renderer sr = myScript.GetComponent<Renderer>();
                        if (sr != null)
                        {
                            if (sr.sharedMaterial != null)
                            {
                                int renderingQueue = sr.sharedMaterial.renderQueue;
                                if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Default) sr.sharedMaterial.shader = Resources.Load("AllIn1SpriteShader", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.ScaledTime) sr.sharedMaterial.shader = Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.MaskedUI) sr.sharedMaterial.shader = Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer) sr.sharedMaterial.shader = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
                                else SetCurrentShaderType(myScript);
                                sr.sharedMaterial.renderQueue = renderingQueue;
                            }
                        }
                        else
                        {
                            Graphic img = myScript.GetComponent<Graphic>();
                            if (img != null && img.material != null)
                            {
                                int renderingQueue = img.material.renderQueue;
                                if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Default) img.material.shader = Resources.Load("AllIn1SpriteShader", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.ScaledTime) img.material.shader = Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.MaskedUI) img.material.shader = Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer) img.material.shader = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
                                else SetCurrentShaderType(myScript);
                                img.material.renderQueue = renderingQueue;
                            }
                        }
                    }
                    else if(!isUrp && myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
                    {
                        myScript.shaderTypes = (AllIn1Shader.ShaderTypes) previousShaderType;
                        showUrpWarning = true;
                        warningTime = EditorApplication.timeSinceStartup + 5;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (warningTime < EditorApplication.timeSinceStartup) showUrpWarning = false;
            if (isUrp) showUrpWarning = false;
            if (showUrpWarning) EditorGUILayout.HelpBox(
                "You can't set the URP 2D Renderer variant since you didn't import the URP package available in the asset root folder (SEE DOCUMENTATION)",
                MessageType.Error,
                true);

            if (isUrp && myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
            {
                EditorGUILayout.Space();
                DrawLine(Color.grey, 1, 3);
                EditorGUILayout.Space();

                if (GUILayout.Button("Create And Add Normal Map"))
                {
                    for (int i = 0; i < targets.Length; i++) ((AllIn1Shader)targets[i]).CreateAndAssignNormalMap();
                    AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Creating Normal Map");
                }
                serializedObject.Update();
                EditorGUILayout.PropertyField(m_NormalStrength, new GUIContent("Normal Strength"), GUILayout.Height(20));
                EditorGUILayout.PropertyField(m_NormalSmoothing, new GUIContent("Normal Blur"), GUILayout.Height(20));
                if (myScript.computingNormal)
                {
                    EditorGUILayout.LabelField("Normal Map is currently being created, be patient", EditorStyles.boldLabel, GUILayout.Height(40));
                }
                serializedObject.ApplyModifiedProperties();

                EditorGUILayout.Space();
            }

            DrawLine(Color.grey, 1, 3);
            EditorGUILayout.Space();

            if (GUILayout.Button("Sprite Atlas Auto Setup"))
            {
                bool successOperation = true;
                for(int i = 0; i < targets.Length; i++)
                {
                    successOperation &= ((AllIn1Shader) targets[i]).ToggleSetAtlasUvs(true);
                }
                if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Sprite Atlas Auto Setup");
            }
            if (GUILayout.Button("Remove Sprite Atlas Configuration"))
            {
                bool successOperation = true;
                for(int i = 0; i < targets.Length; i++)
                {
                    successOperation &= ((AllIn1Shader) targets[i]).ToggleSetAtlasUvs(false);
                }
                if(successOperation) AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Remove Sprite Atlas Configuration");
            }

            EditorGUILayout.Space();
            DrawLine(Color.grey, 1, 3);
            
            if(GUILayout.Button("Remove Component"))
            {
                for(int i = targets.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(targets[i] as AllIn1Shader);
                    ((AllIn1Shader)targets[i]).SetSceneDirty();
                }
                AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Component Removed");
            }

            if (GUILayout.Button("REMOVE COMPONENT AND MATERIAL"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    ((AllIn1Shader)targets[i]).CleanMaterial();
                }
                for (int i = targets.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(targets[i] as AllIn1Shader);
                }
                AllIn1ShaderWindow.ShowSceneViewNotification("AllIn1SpriteShader: Component And Material Removed");
            }
        }

        private void ChooseAndDisplayAssetImage()
        {
            if(!EditorPrefs.HasKey("allIn1ImageConfig"))
            {
                EditorPrefs.SetInt("allIn1ImageConfig", (int) ImageType.ShowImage);
            }

            imageType = (ImageType) EditorPrefs.GetInt("allIn1ImageConfig");
            Texture2D imageInspector = null;
            switch(imageType)
            {
                case ImageType.ShowImage:
                {
                    imageInspector =
                        (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/AllIn1SpriteShader/Textures/CustomEditorImage.png",
                            typeof(Texture2D));
                    break;
                }
                case ImageType.HideInComponent:
                    imageInspector =
                        (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/AllIn1SpriteShader/Textures/CustomEditorImage.png",
                            typeof(Texture2D));
                    break;
            }

            if(imageInspector && imageType != ImageType.HideInComponent && imageType != ImageType.HideEverywhere && imageInspector)
            {
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(40));
                GUI.DrawTexture(rect, imageInspector, ScaleMode.ScaleToFit, true);
            }
        }

        private void SetCurrentShaderType(AllIn1Shader myScript)
        {
            string shaderName = "";
            Renderer sr = myScript.GetComponent<Renderer>();
            if (sr != null)
            {
                if(sr.sharedMaterial == null) return;
                shaderName = sr.sharedMaterial.shader.name;
            }
            else
            {
                Graphic img = myScript.GetComponent<Graphic>();
                if(img.material == null) return;
                if (img != null) shaderName = img.material.shader.name;
            }
            shaderName = shaderName.Replace("AllIn1SpriteShader/", "");

            if (shaderName.Equals("AllIn1SpriteShader")) myScript.shaderTypes = AllIn1Shader.ShaderTypes.Default;
            else if (shaderName.Equals("AllIn1SpriteShaderScaledTime")) myScript.shaderTypes = AllIn1Shader.ShaderTypes.ScaledTime;
            else if (shaderName.Equals("AllIn1SpriteShaderUiMask")) myScript.shaderTypes = AllIn1Shader.ShaderTypes.MaskedUI;
            else if (shaderName.Equals("AllIn1Urp2dRenderer")) myScript.shaderTypes = AllIn1Shader.ShaderTypes.Urp2dRenderer;
        }

        private void DrawLine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += (padding / 2);
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}
#endif
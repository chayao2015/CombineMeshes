﻿using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace ImportNinja
{
    public class AdvancedBehaviour : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Public void

        #endregion

        #region System

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Reset()
        {
            Init();
        }

        #endregion

        #region Class Methods

        private void Init()
        {
            if (m_transform == null)
                m_transform = GetComponent<Transform>();
        }


        #endregion

        #region Tools Debug and Utility

    #if UNITY_EDITOR
        [ContextMenu("Generate Editor for this script")]
        private void GenerateEditor_instance()
        {
            _GenerateEditor(this.GetType().Name);
        }

        [MenuItem("Assets/Generate Editor for this script", true)]
        private static bool GenerateEditor_validation()
        {
            // Will only show up this menu if the asset being clicked on is a script

            return Selection.activeObject is MonoScript;
        }

        [MenuItem("Assets/Generate Editor for this script")]
        private static void GenerateEditor_static()
        {
            _GenerateEditor(Selection.activeObject.name);
        }

        private static void _GenerateEditor(string _name)
	    {
            string template_path        = _GetFullPath(ImportNinja.EditorTemplate);
            string template_content     = File.ReadAllText(template_path);

            string destination          = Directory.GetParent(template_path).FullName;
            string fileName             = _name + "Editor";
            string newEditor_path       = Path.Combine(destination, fileName + ".cs");
            string newEditor_content    = template_content.Replace("#CLASSNAME#", _name);

            string newEditor_relpath    = "Assets" + newEditor_path.Substring(Application.dataPath.Length);

            string[] customEditor = AssetDatabase.FindAssets(fileName);

            if (customEditor.Length != 0)
            {
                Debug.LogError(
                    "Custom editor already existing! Aborting.",
                    AssetDatabase.LoadAssetAtPath<MonoScript>(
                        AssetDatabase.GUIDToAssetPath(customEditor[0])
                    )
                );
            }
            else
            {
                File.WriteAllText(newEditor_path, newEditor_content);

                AssetDatabase.Refresh();

                Debug.Log(
                    "Generated editor from template at '" + newEditor_relpath + "'",
                    AssetDatabase.LoadAssetAtPath<MonoScript>(newEditor_relpath)
                );
            }
        }

        private static string _GetFullPath(string _filename)
        {
            string assetGUID     = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(_filename)).FirstOrDefault();
            string assetRelPath  = AssetDatabase.GUIDToAssetPath(assetGUID);
            string assetFullPath = new DirectoryInfo(assetRelPath).FullName;

            return assetFullPath;
        }
    #endif

        #endregion

        #region Private and Protected Members

        [SerializeField]
        protected Transform m_transform;

        #endregion
    }
}

using Haztech.SpriteEditor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Haztech.SpriteEditor.Editor
{
    [CustomEditor(typeof(SpriteRig))]
    public class SpriteRigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SpriteRig rig = (SpriteRig)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Build Sprite Rig"))
            {
                Undo.RegisterFullObjectHierarchyUndo(
                    rig.gameObject,
                    "Build Sprite Rig");

                rig.Build();

                EditorUtility.SetDirty(rig);
            }
        }
    }
}

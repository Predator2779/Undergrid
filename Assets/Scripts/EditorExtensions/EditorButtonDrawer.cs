using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class EditorButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var type = target.GetType();
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<EditorButtonAttribute>();
            if (attr == null) continue;

            string label = string.IsNullOrEmpty(attr.Label) ? method.Name : attr.Label;

            if (GUILayout.Button(label))
            {
                method.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }
        }
    }
}
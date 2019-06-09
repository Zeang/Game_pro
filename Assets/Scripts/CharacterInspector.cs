using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{
    public SerializedProperty characType;
    public SerializedProperty fbx;
    public SerializedProperty weapon;
    public SerializedProperty walk_forward;
    public SerializedProperty walk_backward;
    public SerializedProperty walk_left;
    public SerializedProperty walk_right;
    public SerializedProperty walk_to_dye;
    public SerializedProperty crouch_forward;
    public SerializedProperty crouch_to_dye;

    private void OnEnable()
    {
        characType = serializedObject.FindProperty("characType");
        fbx = serializedObject.FindProperty("fbx");
        weapon = serializedObject.FindProperty("weapon");
        walk_forward = serializedObject.FindProperty("walk_forward");
        walk_backward = serializedObject.FindProperty("walk_backward");
        walk_left = serializedObject.FindProperty("walk_left");
        walk_right = serializedObject.FindProperty("walk_right");
        walk_to_dye = serializedObject.FindProperty("walk_to_dye");
        crouch_forward = serializedObject.FindProperty("crouch_forward");
        crouch_to_dye = serializedObject.FindProperty("crouch_to_dye");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.indentLevel = 1;

        EditorGUILayout.PropertyField(characType, new GUIContent("角色类型"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(fbx, new GUIContent("角色模型"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(weapon, new GUIContent("武器模型"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(walk_forward, new GUIContent("walk_forward"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(walk_backward, new GUIContent("walk_backward"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(walk_left, new GUIContent("walk_left"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(walk_right, new GUIContent("walk_right"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(walk_to_dye, new GUIContent("walk_to_dye"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(crouch_forward, new GUIContent("crouch_forward"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(crouch_to_dye, new GUIContent("crouch_to_dye"));

        GUILayout.Space(10);


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }

}

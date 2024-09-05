using UnityEngine;
using UnityEditor;

namespace yours_indie_gameDev.Plugin.DataStructures.Editor
{
    [CustomPropertyDrawer(typeof(ObservedList<>))]
    public class ObservedListDrawer : PropertyDrawer
    {
        SerializedProperty foldoutProperty;
        SerializedProperty myListProperty;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            myListProperty = property.FindPropertyRelative("observedList");

            if (myListProperty != null)
            {
                EditorGUI.PropertyField(position, myListProperty, label, true);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, $"Field {label} not found");
            }

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 4;
            if (myListProperty == null) return height;
            //if folded out
            if (myListProperty.arraySize > 0)
            {
                for (int i = 0; i < myListProperty.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(myListProperty.GetArrayElementAtIndex(i));
                }
            }


            return height;
        }
    }
}
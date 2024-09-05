using UnityEngine;
using UnityEditor;
namespace yours_indie_gameDev.Plugin.DataStructures.Editor
{
    [CustomPropertyDrawer(typeof(Array2D<>))]
    public class Array2DDrawer : PropertyDrawer
    {

        int rowCount = 4;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);
            Rect newposition = position;
            newposition.y += 18f;
            SerializedProperty rowData = property.FindPropertyRelative("rowData");
            //data.rows[0][]
            // Get the size of the rowData array
            rowCount = rowData.arraySize;
            for (int j = 0; j < rowCount; j++)
            {
                SerializedProperty row = rowData.GetArrayElementAtIndex(j).FindPropertyRelative("row");
                newposition.height = 18f;

                // Use the size of the row array
                int colCount = row.arraySize;

                //if (colCount != rowCount)
                //  row.arraySize = rowCount;
                newposition.width = position.width / rowCount;
                for (int i = 0; i < colCount; i++)
                {
                    EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                    newposition.x += newposition.width;
                }

                newposition.x = position.x;
                newposition.y += 18f;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18f * (8 + 2);
        }

    }
}

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(Animation2dArray))]
public class CustomTileData : PropertyDrawer 
{
    
    int x;
    int y;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect startPosition = position;
        float yStoredPosition = 0;
        Rect newPosition = position;

        //---------------------------------------------------------------------------------------------------------------------
        
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Nº of States"));

        SerializedProperty serializedX = property.FindPropertyRelative("x");

        newPosition.height = 20;

        newPosition.width = 40;

        newPosition.x += 75;

        EditorGUI.PropertyField(newPosition, serializedX, GUIContent.none);

        newPosition.x -= 75;

        EditorGUI.EndProperty();

        x = serializedX.intValue;

        //---------------------------------------------------------------------------------------------------------------------

        EditorGUI.BeginProperty(position, label, property);

        newPosition.y += 23f;  

        position = EditorGUI.PrefixLabel(newPosition, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Nº of Animations"));

        SerializedProperty serializedY = property.FindPropertyRelative("y");

        newPosition.height = 20;

        newPosition.width = 40;

        newPosition.x += 100;

        EditorGUI.PropertyField(newPosition, serializedY, GUIContent.none);

        newPosition.x -= 100;

        EditorGUI.EndProperty();

        y = serializedY.intValue;

        //---------------------------------------------------------------------------------------------------------------------
       
        EditorGUI.BeginProperty(startPosition, label, property);  

        newPosition.y += 23f;    

        position = EditorGUI.PrefixLabel(newPosition, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Clips"));  
        
        newPosition.y += 23f;
        SerializedProperty rows = property.FindPropertyRelative("rows");
        rows.arraySize = x;
        
        newPosition.x = startPosition.x;

        yStoredPosition = newPosition.y;
        for(int i=0; i < x; i++)
        {
            newPosition.x = startPosition.x;
            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");
            row.arraySize = y;
            newPosition.height = 30;               
            newPosition.width = (((EditorGUIUtility.currentViewWidth)-23) - (2 * y))/y;

            for(int j=0; j < y; j++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                newPosition.x += newPosition.width + 2;
            }

            newPosition.y += newPosition.height+ 5;
        }
        EditorGUI.EndProperty();

        //---------------------------------------------------------------------------------------------------------------------
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 32 * (x + 3);
    }
}

 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Experimental.AI;
using UnityEngine.AI;

[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    private void OnSceneGUI()
    {
        if (!Application.isPlaying)
        {
            

            EnemyAI enemyAI = target as EnemyAI;

            if (enemyAI.PatrolPath.Length == 0)
            {
                return;
            }

            Vector3 previous = enemyAI.transform.position;
            Vector3 current = Vector3.zero;
        
            Handles.color = Color.red;
            Handles.DrawWireArc(previous, Vector3.up, Vector3.forward, 360f, 2);

            for(int i = 0; i < enemyAI.PatrolPath.Length; i++)
            {
                current = enemyAI.PatrolPath.ElementAt(i).transform.position;
                Handles.DrawWireArc(current, Vector3.up, Vector3.forward, 360f, 2);
                Handles.DrawLine(previous, current);

                previous = current;
            }

            current = enemyAI.PatrolPath.ElementAt(0).transform.position;
            Handles.DrawWireArc(current, Vector3.up, Vector3.forward, 360f, 2);
            Handles.DrawLine(previous, current);
        }
        
    }

}

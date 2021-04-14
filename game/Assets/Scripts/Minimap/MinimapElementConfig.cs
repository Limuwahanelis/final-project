using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MinimapElementConfig : MonoBehaviour
{
    int listIndex;
    private void OnDestroy()
    {
        if (Time.frameCount == 0) return;

        if (GameObject.FindGameObjectWithTag("Minimap"))
        {
            GameObject.FindGameObjectWithTag("Minimap").GetComponent<MinimapConfig>().RemoveElementAtPos(listIndex);
        }
    }

    public void Setup(int num)
    {
        listIndex = num;
    }
}

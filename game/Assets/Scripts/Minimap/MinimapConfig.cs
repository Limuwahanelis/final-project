using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class MinimapConfig : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> spriteRenders;
    public List<GameObject> triggers;

    public Transform sR;
    public Transform tr;
    public Color colorOnMinimap;
    public Sprite spriteOnMinimap;
    void Update()
    {
    }
    public void AddElement()
    {
        GameObject el = new GameObject("minimap element" +(spriteRenders.Count+1));
        GameObject el2 = new GameObject("minimap element" + (spriteRenders.Count+1));
        spriteRenders.Add(el);
        triggers.Add(el2);
        el.transform.SetParent(sR);
        el2.transform.SetParent(tr);
        el.transform.position = sR.transform.position;
        el2.transform.position = tr.transform.position;
        el.AddComponent(typeof(SpriteRenderer));
        el2.AddComponent(typeof(BoxCollider2D));
        el2.AddComponent(typeof(MinimapTriggerElement));
        el2.GetComponent<BoxCollider2D>().isTrigger = true;
        el2.GetComponent<MinimapTriggerElement>().SetupElement(triggers.Count - 1, el.GetComponent<SpriteRenderer>(),colorOnMinimap);
        el2.AddComponent(typeof(MinimapElementConfig));
        el2.GetComponent<MinimapElementConfig>().Setup(triggers.Count - 1);
        el2.layer = LayerMask.NameToLayer("Minimap");
        el.layer = LayerMask.NameToLayer("Minimap");
        el.transform.localScale = new Vector3(5f, 5f);
        el.GetComponent<SpriteRenderer>().color = colorOnMinimap;
        el.GetComponent<SpriteRenderer>().sprite = spriteOnMinimap;

    }

    public void RemoveElement()
    {
        DestroyImmediate(spriteRenders[spriteRenders.Count - 1]);
        DestroyImmediate(triggers[triggers.Count - 1]);
        //spriteRenders.RemoveAt(spriteRenders.Count-1);
        //triggers.RemoveAt(triggers.Count - 1);
    }
    public void RemoveElementAtPos(int index)
    {
        DestroyImmediate(spriteRenders[index]);
        spriteRenders.RemoveAt(index);
        triggers.RemoveAt(index);
        for(int i=0;i<triggers.Count;i++)
        {
            if(i>=index)
            {
                triggers[i].GetComponent<MinimapElementConfig>().Setup(i);
                triggers[i].name = "minimap element" +(i+1);
                spriteRenders[i].name= "minimap element" + (i + 1);
            }
        }
    }
    public Vector3 GetSpritePos(int index)
    {
        Vector3 toReturn = spriteRenders[index].transform.position;
        toReturn.z = -0.5f;
        return toReturn;
    }
    private void OnDestroy()
    {
        if (Time.frameCount == 0) return;
        Debug.Log("dsa");
    }
}

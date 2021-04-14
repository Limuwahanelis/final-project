using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapTriggerElement : MonoBehaviour
{
    [SerializeField]
    private int listIndex;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color whiteColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField]
    private Coroutine cor;
    private Minimap minimap;
    // Start is called before the first frame update

    void Start()
    {
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        minimap.SwitchPlaces();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        minimap.SetElement(this);
       
    }

    IEnumerator Blink()
    {
        for(; ;)
        {
            sr.color = whiteColor;
            yield return new WaitForSeconds(0.5f);
            sr.color = normalColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void StartBlinking()
    {
        cor = StartCoroutine(Blink());
    }
    public void StopBlinking()
    {
        StopCoroutine(cor);
        sr.color = normalColor;
    }
    public void SetupElement(int num, SpriteRenderer spriteRend,Color normalCol)
    {
        listIndex = num;
        sr = spriteRend;
        normalColor = normalCol;
    }
    public int GetIndex()
    {
        return listIndex;
    }
}

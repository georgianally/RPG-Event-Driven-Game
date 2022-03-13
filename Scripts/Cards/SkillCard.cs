using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum eSkillType
    {
        Attack,
        Debuff,
        Heal
    };

    public eSkillType skillType;
    public int potency;
    public string skillname;
    public string description;
    public Sprite sprite;

    private bool mouse_over = false;
    void Update()
    {
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 30, this.transform.position.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 30, this.transform.position.z);
    }
}

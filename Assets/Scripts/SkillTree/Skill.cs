using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName;
    public Sprite skillSprite;
    
    public int requiredLvl;

    [TextArea(1, 3)]
    public string skillDes;
    public bool isActivated;
}

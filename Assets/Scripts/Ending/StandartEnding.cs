using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnding", menuName = "Game/StandartEnding", order = 0)]
public class StandartEnding : ScriptableObject
{
    public int id;

    public string endingName;
    public string description;

    public Sprite image;
    public string timeline;
}

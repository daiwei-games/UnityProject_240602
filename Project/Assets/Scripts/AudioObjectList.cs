using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio ListObject", menuName = "object/Audio/Add New ListObject")]
public class AudioObjectList : ScriptableObject
{
    public List<AudioClip> clipList;
}

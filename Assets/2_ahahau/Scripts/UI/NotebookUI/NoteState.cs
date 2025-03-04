using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Note
{
    Chapter,
    person,
    previso
}
public class NoteState : MonoBehaviour
{
    public Note note;
    public List<Image> NoteImages;
    
}

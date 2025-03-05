using System;
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
    public Note ChangeNote;
    public List<GameObject> NoteObjects;

    private void Awake()
    {
        Change(note);
    }

    private void Update()
    {
        if (ChangeNote != note)
        {
            Change(note);
        }
    }

    private void Change(Note note1)
    {
        if (note1 == null) return;
        switch (note1)
        {
            case Note.Chapter:
                NoteObjects[0].SetActive(true);
                NoteObjects[1].SetActive(false);
                NoteObjects[2].SetActive(false);
                break;
            case Note.previso:
                NoteObjects[0].SetActive(false);
                NoteObjects[1].SetActive(true);
                NoteObjects[2].SetActive(false);
                break;
            case Note.person:
                NoteObjects[0].SetActive(false);
                NoteObjects[1].SetActive(false);
                NoteObjects[2].SetActive(true);
                break;
        }
        ChangeNote = note1;
    }
}

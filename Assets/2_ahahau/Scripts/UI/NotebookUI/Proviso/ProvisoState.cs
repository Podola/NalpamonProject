using System;
using System.Collections.Generic;
using UnityEngine;

public enum Proviso
{
    proviso1,
    proviso2,
    proviso3,
    proviso4,
    proviso5,
    proviso6,
    proviso7,
    proviso8,
    proviso9,
}
public class ProvisoState : MonoBehaviour
{
    public Proviso proviso;
    public List<GameObject> provisoes;
    public void ChangeProviso(int provisoNumber)
    {
        if(provisoNumber == null)
            return;
        proviso = (Proviso)provisoNumber;
        
        print(proviso);
    }
}

using UnityEngine;
using System.Collections;



public class Instrument
{
    //Properties
    public string Name;
    public InstrumentFamily Family;
    public AudioClip Sound;
    public Sprite Image;

    //Constructors
    public Instrument()
    {
    }

    public Instrument(string newName, InstrumentFamily newFamily)
    {
        Name = newName;
        Family = newFamily;
    }

    public Instrument(string newName, InstrumentFamily newFamily, AudioClip newSound)
    {
        Name = newName;
        Family = newFamily;
        Sound = newSound;
    }

    public Instrument(string newName, InstrumentFamily newFamily, AudioClip newSound, Sprite newImage)
    {
        Name = newName;
        Family = newFamily;
        Sound = newSound;
        Image = newImage;
    }
}

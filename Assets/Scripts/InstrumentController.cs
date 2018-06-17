using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Make sure we have required components on gameobject
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AudioSource))]

public class InstrumentController : MonoBehaviour
{

    [SerializeField] InstrumentDictionary dictionary;
    [SerializeField] InstrumentFamily instrumentFamilyToPickFrom;
    Instrument instrument;
    Button button;

    // Use this for initialization
    void Start()
    {
        // Add an event listener for when clicked
        button = GetComponent<Button>();
        button.onClick.AddListener(SetSpriteAndPlayAudio);

        if (dictionary == null)
        {
            Debug.LogError("Missing reference to InstrumentDictionary! Assign in my inspector", this);
        }
      
    }


    private void SetSpriteAndPlayAudio()
    {
        // Get a random instrument from the provided family
        instrument = dictionary.GetRandomInstrumentByFamily(instrumentFamilyToPickFrom);
        // Use assets on me
        GetComponent<Image>().sprite = instrument.Image;
        GetComponent<AudioSource>().PlayOneShot(instrument.Sound);

    }
}

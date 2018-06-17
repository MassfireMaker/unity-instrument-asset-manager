using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

public class InstrumentDictionary : MonoBehaviour {

    // The dictionary with instruments
    Dictionary<string, Instrument> instruments = new Dictionary<string, Instrument>();
    // Raw asset files
    UnityEngine.Object[] rawInstrumentFiles;
    // Text file containing all instrument asset file paths
    string txtFilePath = "Assets/Resources/assetPaths.txt";
    List<string> assetPaths = new List<string>();

    // Use this for initialization
    void Start () {

        // TODO: Create InstrumentAssets folder structure && assetPaths.txt if not present

        // Store all asset files for instruments
        rawInstrumentFiles = Resources.LoadAll("InstrumentAssets");

        // TODO: Only do WriteAssetPaths if in Unity Editor
        WriteAssetPaths();

        // Load asset paths to memory
        assetPaths.AddRange( GetAssetPaths() );

        // Setup
        LoadInstruments();



        /* Debug methods */

        
    }



    #region METHODS

    #region Public 
    public Instrument GetInstrumentByName(string name) //Returns instrument object by name
    {
        //Return instrument object
        return instruments[name];
    }

    public Instrument GetRandomInstrumentByFamily(InstrumentFamily family)
    {
        List<Instrument> familyList = new List<Instrument>();
        // Gather all instruments part of same family
        foreach(var item in instruments)
        {
            if(item.Value.Family == family)
            {
                familyList.Add(item.Value);
            }
        }
        // Pick a random instrument to return
        return familyList[UnityEngine.Random.Range(0, familyList.Count)];
    }
    #endregion

    #region Private
    private void LoadInstruments() // Loads all instruments
    {
        // Create a list with all instrument names
        List<string> instrumentNameList = GetUniqueInstrumentNames(rawInstrumentFiles);
        // For each instrument, get assets and add to dictionary
        instrumentNameList.ForEach(delegate (String name) {
            Instrument instrument = new Instrument(
                name,
                GetInstrumentFamilyName(name),
                GetInstrumentAudioClip(name),
                GetInstrumentSprite(name));

            instruments.Add(name, instrument);
        });
        

       
    }

    private List<string> GetUniqueInstrumentNames(UnityEngine.Object[] rawInstrumentFiles)
    {
        // Parse files to figure out uniqly named instruments
        List<string> instrumentNamesList = new List<string>();

        foreach (var item in rawInstrumentFiles)
        {
            if ( !instrumentNamesList.Exists(x => x.Contains(item.name)) )
            {
                // Only add if not already in list
                instrumentNamesList.Add(item.name);
            }
        }
        return instrumentNamesList;
    }

    private void WriteAssetPaths() // Writes all instrument asset file paths to TextAsset for instrument family parsing later
    {
        StreamWriter writer = new StreamWriter(txtFilePath);

        foreach (var item in rawInstrumentFiles)
        {
            string pathToWrite = AssetDatabase.GetAssetPath(item);
            writer.WriteLine(pathToWrite);
        }
        
        writer.Close();
        

    }

    private List<string> GetAssetPaths()
    {
        List<string> paths = new List<string>();
        StreamReader reader = new StreamReader(txtFilePath);
        string currentLine;

        // Read and store every line from file until no more lines are found
        while ((currentLine = reader.ReadLine()) != null)
        {
            paths.Add(currentLine);
        }
        
        return paths;
    } // Returns all instrument asset file paths as a List

    private InstrumentFamily GetInstrumentFamilyName(string instrumentName) // Returns the specified instrument family as enum
    {
        string pathToParse;
        string parsedFamilyName = "";
        InstrumentFamily familyName;
        // Parse folder from assets to get family name
        pathToParse = Path.GetDirectoryName( assetPaths.Find(x => x.Contains(instrumentName) ) );

        parsedFamilyName = pathToParse.Substring(pathToParse.LastIndexOf('/') +1 );

        // Convert to enum
        switch (parsedFamilyName)
        {
            case "bowed":
                    familyName = InstrumentFamily.BOWED;
                break;
            case "brass":
                familyName = InstrumentFamily.BRASS;
                break;
            case "key":
                familyName = InstrumentFamily.KEY;
                break;
            case "perc":
                familyName = InstrumentFamily.PERCUSSION;
                break;
            case "string":
                familyName = InstrumentFamily.STRING;
                break;
            case "woodwind":
                familyName = InstrumentFamily.WOODWIND;
                break;
            default:
                familyName = InstrumentFamily.UNKNOWN;
                break;


        }
        return familyName;
    }    

    private AudioClip GetInstrumentAudioClip(string instrumentName)
    {
        AudioClip audioClip = Array.Find(rawInstrumentFiles,                    // Search assets
                                         x => x.name.Contains(instrumentName)   // for file named same as instrument
                                         && x is AudioClip)                     // and is type AudioClip.
                                         as AudioClip;                          // Get it as type AudioClip

        return audioClip;
    } // Returns the AudioClip for specified instrument

    private Sprite GetInstrumentSprite(string instrumentName)
    {
        Sprite sprite = Array.Find(rawInstrumentFiles,                          // Search assets
                                         x => x.name.Contains(instrumentName)   // for file named same as instrument
                                         && x is Sprite)                        // and is type Sprite.
                                         as Sprite;                             // Get it as type Sprite

        return sprite;
    } // Returns the Sprite for specified instrument


    private Instrument CreateInstrument(string instrumentName) // Creates and returns instrument object by name
    {
        // TODO: Add error handling for missing assets
        Instrument newInstrument = new Instrument(
           instrumentName,                          //Name
           GetInstrumentFamilyName(instrumentName), //Family
           GetInstrumentAudioClip(instrumentName),  //AudioClip
           GetInstrumentSprite(instrumentName) );   //Sprite

        return newInstrument;
    }
    #endregion

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartManager : MonoBehaviour
{
    public GameObject optionsMenu;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public GameObject dataPersistenceManager;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        LoadOptions();
        optionsMenu.SetActive(false);
        StartCoroutine(clearDataPersistence());
    }

    public IEnumerator clearDataPersistence(){
        yield return new WaitForSeconds(.1f);
        var dm = FindObjectsOfType<DataPersistenceManager>();
        foreach (var d in dm){
            Destroy(d.gameObject);
        }
        yield return new WaitForSeconds(.1f);
        Instantiate(dataPersistenceManager);
    }
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            SaveOptions();
        }
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }
    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;

        UpdateMixerVolume();
        SaveOptions();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;

        UpdateMixerVolume();
        SaveOptions();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        Debug.Log("Starting new game...");
        DataPersistenceManager.instance.NewGame();
        FindObjectOfType<LevelManager>().updateScene("Level1");
        FindObjectOfType<LevelManager>().ResetCheckPoint();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("Level1");
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.getScene());
        print(DataPersistenceManager.instance.getScene());
    }

    public void ShowOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

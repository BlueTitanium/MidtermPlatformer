using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum pauseMenuStates {resume, options, exit};
    
    public bool paused;
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private GameObject PauseOverlay;
    [SerializeField] private Transform pauseCircle;
    [SerializeField] private Animator animatePause;
    [SerializeField] private Image[] buttonBGs;
    private InputActionMap actionmap;
    public Color[] colors;
    private pauseMenuStates pauseState;
    [Header("Options")]
    public GameObject options;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public GameObject leaveScreen;

    // Start is called before the first frame update
    void Start()
    {
        LoadOptions();
        actionmap = playerControls.FindActionMap("MenuControls");
        PauseOverlay.SetActive(false);
        options.SetActive(false);
        var menuBTN = actionmap.FindAction("MENU");
        menuBTN.performed += MenuBTN_performed;
        var backBTN = actionmap.FindAction("BACK");
        backBTN.performed += BackBTN_performed;
        var nextBTN = actionmap.FindAction("NEXT");
        nextBTN.performed += NextBTN_performed;
        var chooseBTN = actionmap.FindAction("CHOOSE");
        chooseBTN.performed += ChooseBTN_performed;
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

    private void ChooseBTN_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            StartCoroutine(DoButtonPress());
        }
    }

    private void NextBTN_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            switch (pauseState)
            {
                case pauseMenuStates.resume:
                    pauseState = pauseMenuStates.options;
                    SetBlack();
                    SetColor(1, 1);
                    break;
                case pauseMenuStates.options:
                    pauseState = pauseMenuStates.exit;
                    SetBlack();
                    SetColor(2, 1);
                    break;
                case pauseMenuStates.exit:
                    pauseState = pauseMenuStates.resume;
                    SetBlack();
                    SetColor(0, 1);
                    break;
                default:
                    break;
            }
        }
    }

    private void BackBTN_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            switch (pauseState)
            {
                case pauseMenuStates.resume:
                    pauseState = pauseMenuStates.exit;
                    SetBlack();
                    SetColor(2, 1);
                    break;
                case pauseMenuStates.options:
                    pauseState = pauseMenuStates.resume;
                    SetBlack();
                    SetColor(0, 1);
                    break;
                case pauseMenuStates.exit:
                    pauseState = pauseMenuStates.options;
                    SetBlack();
                    SetColor(1, 1);
                    break;
                default:
                    break;
            }
        }
    }

    private void MenuBTN_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            Unpause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBlack()
    {
        foreach (Image i in buttonBGs)
        {
            i.color = colors[0];
        }
    }
    void SetColor(int index, int color)
    {
        buttonBGs[index].color = colors[color];
    }
    IEnumerator DoButtonPress()
    {
        actionmap.Disable();
        SetColor((int)pauseState, 2);
        yield return new WaitForSecondsRealtime(0.15f);
        actionmap.Enable();
        print(pauseState);
        switch (pauseState)
        {
            case pauseMenuStates.resume:
                Unpause();
                break;
            case pauseMenuStates.options:
                print("Hello");
                //TODO
                ShowOptions();
                break;
            case pauseMenuStates.exit:
                //TODO should return to main menu
                Exit();
                break;
            default:
                break;
        }
        SetColor((int)pauseState, 1);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        paused = true;
        PauseOverlay.SetActive(true);
        pauseState = pauseMenuStates.resume;
        SetBlack();
        SetColor(0, 1);
        pauseCircle.localPosition = new Vector3(-982f, 0f, 0f);
        pauseCircle.eulerAngles = new Vector3(0, 0, 90f);
        actionmap.Enable();

    }
    
    public void Unpause()
    {
        Time.timeScale = 1f;
        paused = false;
        PauseOverlay.SetActive(false);
        actionmap.Disable();
        GameObject.FindObjectOfType<PlayerController>().actionmap.Enable();
        CloseOptions();
    }
    public void Exit()
    {
        //should return to main menu
        Application.Quit();
    }
    public void ShowOptions()
    {
        print("Pressed");

        if (!options.activeInHierarchy)
        {
            options.SetActive(true);
        } else
        {
            options.SetActive(false);
        }
    }
    public void CloseOptions()
    {
        options.SetActive(false);
    }
}

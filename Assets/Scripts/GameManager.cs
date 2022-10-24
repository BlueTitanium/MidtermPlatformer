using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        actionmap = playerControls.FindActionMap("MenuControls");
        PauseOverlay.SetActive(false);
        var menuBTN = actionmap.FindAction("MENU");
        menuBTN.performed += MenuBTN_performed;
        var backBTN = actionmap.FindAction("BACK");
        backBTN.performed += BackBTN_performed;
        var nextBTN = actionmap.FindAction("NEXT");
        nextBTN.performed += NextBTN_performed;
        var chooseBTN = actionmap.FindAction("CHOOSE");
        chooseBTN.performed += ChooseBTN_performed;
    }

    private void ChooseBTN_performed(InputAction.CallbackContext obj)
    {
        
        StartCoroutine(DoButtonPress());

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
        Unpause();
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
        switch (pauseState)
        {
            case pauseMenuStates.resume:
                Unpause();
                break;
            case pauseMenuStates.options:
                //TODO
                break;
            case pauseMenuStates.exit:
                //TODO
                Application.Quit();
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
    }
}

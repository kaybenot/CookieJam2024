using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class InputManager
{
    public static InputManager Instance { get; private set; }
    
    public Action<float, float> OnLeftClick { get; set; }

    private readonly PlayerInput playerInput;

    private InputManager()
    {
        var inputManager = GameObject.FindWithTag("InputManager");
        if (inputManager == null)
        {
            var mgr = Object.Instantiate(Resources.Load("InputManager"));
            inputManager = (GameObject)mgr;
        }

        playerInput = inputManager.GetComponent<PlayerInput>();
        RegisterCallbacks();
        
        Object.DontDestroyOnLoad(inputManager);
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        Instance = new InputManager();
    }

    private void RegisterCallbacks()
    {
        playerInput.actions["Attack"].performed += (callback) =>
        {
            var mousePos = Input.mousePosition;
            OnLeftClick?.Invoke(mousePos.x, mousePos.y);
        };
    }
}

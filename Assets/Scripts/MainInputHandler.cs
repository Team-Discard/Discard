using JetBrains.Annotations;
using UnityEngine;

public class MainInputHandler
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration), UsedImplicitly]
    private static void StaticInit()
    {
        _instance = null;
    }

    private static MainInputHandler _instance;

    private MainPlayerControl _control;
    
    
    private MainInputHandler()
    {
        _control = new MainPlayerControl();
        _control.Enable();
    }

    public static MainInputHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MainInputHandler();
            }

            return _instance;
        }
    }

    public MainPlayerControl Control => _control;

}
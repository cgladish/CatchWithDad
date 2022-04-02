using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

     
    private bool m_IsDevModeEnabled = false;
    public delegate void OnToggleDevModeDelegate(bool newIsDevModeEnabled);
    public event OnToggleDevModeDelegate OnToggleDevMode;
    [SerializeField]
    public bool IsDevModeEnabled
    {
        get {
            return m_IsDevModeEnabled;
        }
        set {
            if (m_IsDevModeEnabled != value) {
                m_IsDevModeEnabled = value;
                if (OnToggleDevMode != null) {
                    OnToggleDevMode(m_IsDevModeEnabled);
                }
            }
        }
    }
}

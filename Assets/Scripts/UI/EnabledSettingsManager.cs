using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledSettingsManager : MonoBehaviour
{
    public GameObject ButtonThrowSettings;

    private void OnEnable() {
        ButtonThrowSettings.SetActive(GlobalSettings.Instance.IsDevModeEnabled);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageRoot : MonoBehaviour
{

    [SerializeField] Button BackBtn;
    private void Awake()
    {
        ScreenOrientationHelper.SetOrientation(ScreenOrientationHelper.NScreenOrientation.LandscapeLeft);
    }

    private void Start()
    {
        ScreenOrientationHelper.SetOrientation(ScreenOrientationHelper.NScreenOrientation.LandspaceAutoRotation);
    }

    public void TempClick() 
    {
        SceneManager.LoadScene("Main");
    }

}


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainRoot : MonoBehaviour
{
    [SerializeField] Button EnterBtn;
    private void Awake()
    {
        ScreenOrientationHelper.SetOrientation(ScreenOrientationHelper.NScreenOrientation.Portrait);
    }

    public void TempClick() 
    {
        SceneManager.LoadScene("StageScn1");
    }

}

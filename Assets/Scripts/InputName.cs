using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InputName : MonoBehaviour
{
    [SerializeField] TMP_InputField inputName;
    [SerializeField] GameObject warningText;
    public static string userName;
    public static InputName Instance;

    public void SaveName()
    {
        userName = inputName.text;
        if(userName == "")
        {
            warningText.SetActive(true);
        }
        else
        {
            Debug.Log("start");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;//Do not miss this line
        DontDestroyOnLoad(gameObject);
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentChapter = 1;
    public int currentDay = 1;

    private DialogueRunner dr;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dr = FindFirstObjectByType<DialogueRunner>();
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"[GameManager] Loading Scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    public void AdvanceDay()
    {
        currentDay++;
        Debug.Log($"[GameManager] Day advanced -> {currentDay}");

        // Yarn 변수와 동기화
        SetYarnVariable("day", currentDay);
    }

    public void SetYarnVariable(string varName, object value)
    {
        string fullName = "$" + varName;
        var storage = dr.VariableStorage;

        if (value is int iVal)
        {
            storage.SetValue(fullName, iVal);
        }
        else if (value is float fVal)
        {
            storage.SetValue(fullName, fVal);
        }
        else if (value is bool bVal)
        {
            storage.SetValue(fullName, bVal);
        }
        else if (value is string sVal)
        {
            storage.SetValue(fullName, sVal);
        }
        else
        {
            Debug.Log($"[GameManager] Unsupported type for Yarn variables {varName}");
        }
    }

    public object GetYarnVariable<T>(string varName)
    {
        string fullName = "$" + varName;
        var storage = dr.VariableStorage;
        var yarnValue = storage.TryGetValue<T>(fullName, out T outVal);

        return outVal;

    }

}

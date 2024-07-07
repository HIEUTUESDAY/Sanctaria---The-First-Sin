using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Coroutine StartCoroutineManager(IEnumerator coroutine)
    {
        Coroutine newCoroutine = StartCoroutine(CoroutineWrapper(coroutine));
        activeCoroutines.Add(newCoroutine);
        return newCoroutine;
    }

    public void StopCoroutineManager(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void StopCoroutineManager(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            activeCoroutines.Remove(coroutine);
        }
    }

    public void StopAllCoroutinesManager()
    {
        foreach (Coroutine coroutine in activeCoroutines)
        {
            StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();
    }

    private IEnumerator CoroutineWrapper(IEnumerator coroutine)
    {
        while (true)
        {
            try
            {
                if (!coroutine.MoveNext())
                {
                    yield break;
                }
            }
            catch (MissingReferenceException)
            {
                // Silent fail, stop the coroutine
                yield break;
            }
            yield return coroutine.Current;
        }
    }
}

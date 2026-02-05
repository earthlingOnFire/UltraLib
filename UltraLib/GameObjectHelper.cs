using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

namespace UltraLib;

[PublicAPI]
public static class GameObjectHelper
{

    /// <summary>
    ///     Gets the full path of a GameObject.
    /// </summary>
    /// <param name="gameObject">The gameObject to get the full path of.</param>
    /// <returns>
    ///     The full path of the GameObject. An empty string if the GameObject is null.
    /// </returns>
    public static string GetFullPath(GameObject gameObject)
    {
        if (gameObject == null) return "";

        Transform transform = gameObject.transform;
        string path = gameObject.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }

        return path;
    }

    /// <summary>
    ///     Finds and returns a GameObject with the specified path.
    ///     Works even if the GameObject is disabled.
    /// </summary>
    /// <remarks>
    ///     In cases where the name of a GameObject in the path contains a '/' character, 
    ///     the '%' character should be used in its place.
    /// </remarks>
    /// <param name="gameObjectPath">The path of the GameObject to find.</param>
    /// <returns>
    ///     The GameObject with the specified path. Null if such a GameObject cannot be found.
    /// </returns>
    public static GameObject? FindGameObject(string gameObjectPath)
    {
        GameObject gameObject = GameObject.Find(gameObjectPath);
        if (gameObject != null) return gameObject;

        GameObject[] sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        string[] parts = gameObjectPath.Split(new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = parts[i].Replace("%", "/");
        }

        int rootParentIndex = Array.FindIndex(sceneObjects, o => o.name == parts[0]);
        if (rootParentIndex == -1) return null;
        GameObject rootParent = sceneObjects[rootParentIndex];
        if (parts.Length == 1) return rootParent;

        string[] rest = new string[parts.Length - 1];
        Array.Copy(parts, 1, rest, 0, rest.Length);
        string subPath = String.Join("/", rest);

        return rootParent.transform.Find(subPath)?.gameObject;
    }
}


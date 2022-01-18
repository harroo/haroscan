
using UnityEngine;

using System.Collections.Generic;

public static class Cache {

    public static int totalScanned = 0;

    public static List<string> results = new List<string>();

    public static void Save () {

        PlayerPrefs.SetInt("TOTAL_SCANNED", totalScanned);

        string resultCache = "";
        foreach (string result in results)
            resultCache += result + "%";

        PlayerPrefs.SetString("RESULTS", resultCache);
    }

    public static void Load () {

        totalScanned = PlayerPrefs.GetInt("TOTAL_SCANNED", 0);

        results.Clear();
        foreach (string result in PlayerPrefs.GetString("RESULTS", "").Split('%'))
            if (result.Length == 16) results.Add(result);
    }
}

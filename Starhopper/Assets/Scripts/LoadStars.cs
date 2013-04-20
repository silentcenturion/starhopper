using UnityEngine;
using System.Collections;
using System.IO;
using System;

public static class LoadStars
{
    public static Star[] Load()
    {
        TextAsset starDatabase = (TextAsset)Resources.Load("hygxyz");

        string[] lines = starDatabase.text.Split('\n');
        int numberOfStars = lines.Length - 1;
        Debug.Log("Parsing " + numberOfStars + " stars...");
        Star[] stars = new Star[numberOfStars];
        
        int currentStar = 0;
        for (int lineIndex = 1; lineIndex < lines.Length - 1; lineIndex++) // skip first line that contains headers
        {
            stars[currentStar++] = ParseStar(lines[lineIndex]);

            if (currentStar > stars.Length)
                break;
        }
        Debug.Log(currentStar + " stars parsed!");
        Array.Resize(ref stars, currentStar);
        return stars;
    }

    private static Star ParseStar(string data)
    {
        int i = 0;
        Star star = new Star();
        star.StarID = ParseInt(data, ref i);
        star.HIP = ParseInt(data, ref i);
        star.HD = ParseInt(data, ref i);
        star.HR = ParseInt(data, ref i);
        star.Gliese = ParseString(data, ref i);
        star.BayerFlamsteed = ParseString(data, ref i);
        star.ProperName = ParseString(data, ref i);
        star.RA = ParseFloat(data, ref i);
        star.Dec = ParseFloat(data, ref i);
        star.Distance = ParseFloat(data, ref i);
        star.PMRA = ParseFloat(data, ref i);
        star.PMDec = ParseFloat(data, ref i);
        star.RV = ParseFloat(data, ref i);
        star.Mag = ParseFloat(data, ref i);
        star.AbsMag = ParseFloat(data, ref i);
        star.Spectrum = ParseString(data, ref i);
        star.ColorIndex = ParseFloat(data, ref i);
        star.X = ParseFloat(data, ref i);
        star.Y = ParseFloat(data, ref i);
        star.Z = ParseFloat(data, ref i);
        star.VX = ParseFloat(data, ref i);
        star.VY = ParseFloat(data, ref i);
        star.VZ = ParseFloat(data, ref i);

        return star;
    }

    private static string ParseString(string data, ref int i)
    {
        string text = Parse(data, i);

        if (text.Length == 0)
        {
            i++;
            return string.Empty;
        }

        i += text.Length + 1;
        return text;
    }

    private static float ParseFloat(string data, ref int i)
    {
        string text = Parse(data, i);

        if (text.Length == 0)
        {
            i++;
            return 0;
        }

        i += text.Length + 1;
        return float.Parse(text);
    }

    private static int ParseInt(string data, ref int i)
    {
        string text = Parse(data, i);

        if (text.Length == 0)
        {
            i++;
            return 0;
        }

        i += text.Length + 1;
        return int.Parse(text);
    }

    private static string Parse(string data, int i)
    {
        int index = data.IndexOf(',', i) - i;
        string text;
        if (index < 0)
            text = data.Substring(i, data.Length - i);
        else
            text = data.Substring(i, index);
        return text;
    }
}

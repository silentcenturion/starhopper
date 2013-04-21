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
        star.StarID = CSVUtils.ParseInt(data, ref i);
        star.HIP = CSVUtils.ParseInt(data, ref i);
        star.HD = CSVUtils.ParseInt(data, ref i);
        star.HR = CSVUtils.ParseInt(data, ref i);
        star.Gliese = CSVUtils.ParseString(data, ref i);
        star.BayerFlamsteed = CSVUtils.ParseString(data, ref i);
        star.ProperName = CSVUtils.ParseString(data, ref i);
        star.RA = CSVUtils.ParseFloat(data, ref i);
        star.Dec = CSVUtils.ParseFloat(data, ref i);
        star.Distance = CSVUtils.ParseFloat(data, ref i);
        star.PMRA = CSVUtils.ParseFloat(data, ref i);
        star.PMDec = CSVUtils.ParseFloat(data, ref i);
        star.RV = CSVUtils.ParseFloat(data, ref i);
        star.Mag = CSVUtils.ParseFloat(data, ref i);
        star.AbsMag = CSVUtils.ParseFloat(data, ref i);
        star.Spectrum = CSVUtils.ParseString(data, ref i);
        star.ColorIndex = CSVUtils.ParseFloat(data, ref i);
        star.X = CSVUtils.ParseFloat(data, ref i);
        star.Y = CSVUtils.ParseFloat(data, ref i);
		star.Z = CSVUtils.ParseFloat(data, ref i);
		star.VX = CSVUtils.ParseFloat(data, ref i);
		star.VY = CSVUtils.ParseFloat(data, ref i);
		star.VZ = CSVUtils.ParseFloat(data, ref i);

        return star;
    }

}

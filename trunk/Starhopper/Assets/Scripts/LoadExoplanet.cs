using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public static class LoadExoplanet
{
    public static void Load(Star[] stars)
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        UnityEngine.Debug.Log(sw.Elapsed.TotalMilliseconds);

        Exoplanet[] exoplanets = LoadExoplanets(stars);
        Dictionary<int, List<int>> planetsByHDID = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> planetsByHiPID = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> planetsByHDR = new Dictionary<int, List<int>>();
        
        for(int i= 0; i<exoplanets.Length;i++)
        {
            Exoplanet exoplanet = exoplanets[i];
            if (exoplanet.SystemName.StartsWith("HD"))
            {
                string id = exoplanet.SystemName.Substring(3);
                int hdID;
                if (int.TryParse(id, out hdID) == false)
                    continue;

                if (! planetsByHDID.ContainsKey(hdID))
                {
                    planetsByHDID[hdID] = new List<int>();
                }
                planetsByHDID[hdID].Add(i);
            }
            else if(exoplanet.SystemName.StartsWith("HIP"))
            {
                string id = exoplanet.SystemName.Substring(4);
                int hipID;
                if (int.TryParse(id, out hipID) == false)
                    continue;

                if (! planetsByHiPID.ContainsKey(hipID))
                {
                    planetsByHiPID[hipID] = new List<int>();
                }
                planetsByHiPID[hipID].Add(i);
            }
            else if (exoplanet.SystemName.StartsWith("HR"))
            {
                string id = exoplanet.SystemName.Substring(3);
                int hrID;
                if (int.TryParse(id, out hrID) == false)
                    continue;

                if (! planetsByHDR.ContainsKey(hrID))
                {
                    planetsByHDR[hrID] = new List<int>();
                }
                planetsByHDR[hrID].Add(i);
            }
        }
        
        Dictionary<int, List<Exoplanet>> starMap = new Dictionary<int, List<Exoplanet>>();
        foreach (Star star in stars)
        {
            if (star.HD != 0)
            {
                List<int> planets;
                if (planetsByHiPID.TryGetValue(star.HD, out planets))
                {
                    foreach (int planetID in planets)
                    {
                        star.Planets.Add(exoplanets[planetID]);
                    }
                }
            }
            if (star.HIP != 0)
            {
                List<int> planets;
                if (planetsByHDID.TryGetValue(star.HIP, out planets))
                {
                    foreach (int planetID in planets)
                    {
                        star.Planets.Add(exoplanets[planetID]);
                    }
                }

            } 
            if (star.HR != 0)
            {
                List<int> planets;
                if (planetsByHDR.TryGetValue(star.HR, out planets))
                {
                    foreach (int planetID in planets)
                    {
                        star.Planets.Add(exoplanets[planetID]);
                    }
                }

            }
        }
    }

    public static Exoplanet[] LoadExoplanets(Star[] stars)
    {
        Exoplanet[] exoplanets = new Exoplanet[10000];
        Dictionary<string, int> labels;

        StreamReader sr = new StreamReader("exoplanets.csv");

        // The first line has captions about the data...
        labels = ParseLabels(sr.ReadLine());

        int currentExoplanet = 0;
        while (sr.EndOfStream == false)
        {
            string line = sr.ReadLine();
            exoplanets[currentExoplanet] = ParseExoplanet(labels, line);

            currentExoplanet++;

            if (currentExoplanet > exoplanets.Length)
                break;
        }

        Array.Resize(ref exoplanets, currentExoplanet);
        return exoplanets;
    }

    private static Dictionary<string, int> ParseLabels(string data)
    {
        int current_label = 0;
        Dictionary<string, int> labels = new Dictionary<string, int>();

        string[] words = data.Split(',');
        foreach (string word in words)
        {
            labels.Add(word.Trim(), current_label);
            current_label++;
        }
        return labels;
    }

    private static Exoplanet ParseExoplanet(Dictionary<string, int> labels, string data)
    {
        Exoplanet exoplanet = new Exoplanet();
        exoplanet.SystemName = ParseString(data, labels["STAR"]);
        exoplanet.PlanetName = ParseString(data, labels["NAME"]);
        exoplanet.SemiMajorAxis = ParseFloat(data, labels["A"]);
        exoplanet.OrbitalEccentricity = ParseFloat(data, labels["ECC"]);
        return exoplanet;
    }

    private static string ParseString(string data, int i)
    {
        string text = Parse(data, i);

        if (text.Length == 0)
        {
            return string.Empty;
        }
        return text;
    }

    private static float ParseFloat(string data, int i)
    {
        string text = Parse(data, i);

        if (text.Length == 0)
        {
            return 0;
        }

        return float.Parse(text);
    }

    private static string Parse(string data, int i)
    {
        string[] words = data.Split(',');
        int index = 0;
        foreach (string word in words)
        {
            if (index == i)
            {
                return word;
            }
            index++;
        }
        return string.Empty;
    }
}

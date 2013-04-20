using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public static class LoadExoplanet
{
    public static Dictionary<int, List<Exoplanet>> Load(Star[] stars)
    {
        Exoplanet[] exoplanets = LoadExoplanets(stars);
        Dictionary<int, List<Exoplanet>> star_map = new Dictionary<int, List<Exoplanet>>();

        foreach (Exoplanet exoplanet in exoplanets)
        {
            if (exoplanet.SystemName.StartsWith("HD"))
            {
                string hdID = exoplanet.SystemName.Substring(3);
                foreach (Star star in stars)
                {
                    if (star.HD.ToString() == hdID)
                    {
                        if (!star_map.ContainsKey(star.StarID))
                        {
                            star_map[star.StarID] = new List<Exoplanet>();
                        }
                        star_map[star.StarID].Add(exoplanet);
                        break;
                    }
                }
            }

            if (exoplanet.SystemName.StartsWith("HIP"))
            {
                string hipID = exoplanet.SystemName.Substring(4);
                foreach (Star star in stars)
                {
                    if (star.HIP.ToString() == hipID)
                    {
                        if (!star_map.ContainsKey(star.StarID))
                        {
                            star_map[star.StarID] = new List<Exoplanet>();
                        }
                        star_map[star.StarID].Add(exoplanet);

                        break;
                    }
                }
            }

            if (exoplanet.SystemName.StartsWith("HR"))
            {
                string hrID = exoplanet.SystemName.Substring(3);
                foreach (Star star in stars)
                {
                    if (star.HR.ToString() == hrID)
                    {
                        if (!star_map.ContainsKey(star.StarID))
                        {
                            star_map[star.StarID] = new List<Exoplanet>();
                        }
                        star_map[star.StarID].Add(exoplanet);
                        break;
                    }
                }
            }
        }
        return star_map;
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

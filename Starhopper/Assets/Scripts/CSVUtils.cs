using UnityEngine;
using System.Collections;

public static class CSVUtils {
	
	public static string ParseString(string data, ref int i)
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
	
	public static float ParseFloat(string data, ref int i)
	{
		string text = Parse(data, i);
		
		if (text.Length == 0)
		{
			i++;
			return 0;
		}
		i += text.Length + 1;
		float val = 0;
		float.TryParse(text, out val);
		return val;
	}
	
	public static int ParseInt(string data, ref int i)
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
	
	public static string Parse(string data, int i)
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

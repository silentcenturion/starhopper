using System.Collections.Generic;
public class Star
{
    public int StarID;
    public int HIP;
    public int HD;
    public int HR;
    public string Gliese;
    public string BayerFlamsteed;
    public string ProperName;
    public float RA;
    public float Dec;
    public float Distance;
    public float PMRA;
    public float PMDec;
    public float RV;
    public float Mag;
    public float AbsMag;
    public string Spectrum;
    public float ColorIndex;
    public float X;
    public float Y;
    public float Z;
    public float VX;
    public float VY;
    public float VZ;

    public List<Exoplanet> Planets = new List<Exoplanet>();
	
	public string GetName()
	{
		if (string.IsNullOrEmpty(ProperName) == false)
            return ProperName;
        else if (string.IsNullOrEmpty(BayerFlamsteed) == false)
            return BayerFlamsteed;
        else if (string.IsNullOrEmpty(Gliese) == false)
            return Gliese;
        else
            return "Unnamed Star";
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//Class for reading Standard weather files
namespace WeatherWatch
{
    public class WeatherReader
    {
        #region Enumerations
        public enum WeaData
        {
            WriteDate = 0,
            WriteTime,
            TempScale,
            WindScale,
            SkyTemp,
            AmbTemp,
            SenTemp,
            WindSpeed,
            Humidity,
            DewPoint,
            DewHeat,
            RainFlag,
            WetFlag,
            ElapsedSeconds,
            LastDataWrite,
            Cloudiness,
            Windiness,
            Raininess,
            Darkness,
            RoofCloseFlag,
            AlertFlag
        }

        public enum WeaTempScale
        {
            Celsius = 0,
            Farenheit
        }

        public enum WeaWindScale
        {
            MPH = 0,
            Knots
        }

        public enum WeaCloudiness
        {
            Clear = 1,
            Cloudy,
            VeryCloudy
        }

        public enum WeaWindiness
        {
            Calm = 1,
            Windy,
            VeryWindy
        }

        public enum WeaRaininess
        {
            Dry = 1,
            Damp,
            Rain
        }

        public enum WeaDarkness
        {
            Dark = 1,
            Dim,
            Daylight
        }

        public enum WeaRainFlag
        {
            NoRain = 0,
            Rain
        }
        public enum WeaWetFlag
        {
            NoWet = 0,
            Wet
        }
        public enum WeaRoofFlag
        {
            NoRoof = 0,
            Roof
        }

        public enum WeaAlert
        {
            NoAlert = 0,
            Alert
        }

        #endregion

        private string weatherDataFilePath;
        private List<string> weaList;
        private List<string> LastValidReading = new List<string>() //Initialized with bogus data
        {   "2023-04-15",
            "03:13:33.33",
            "F",
            "M",
            "13.7",
            "60.1",
            "60.1",
            "0.0",
            "10",
            "0.0",
            "0",
            "0",
            "0",
            "00003",
            "045031.13441",
            "1",
            "1",
            "1",
            "1",
            "0",
            "0"
        };

        public WeatherReader()
        {
            weatherDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CloudWatcher\\AAG_CCDAP4.dat";
            weaList = ReadWeatherDataIn();
            return;
        }

        public WeatherReader(string weatherFilePath)
        {
            weatherDataFilePath = weatherFilePath;
            weaList = ReadWeatherDataIn();
            return;
        }

        public bool IsWeatherValid()
        {
            weaList = ReadWeatherDataIn();
            if (weaList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void WeatherUpdate()
        {
            weaList = ReadWeatherDataIn();
            return;
        }

        public string WeatherDataListing()
        {
            //Forms the weather data into a string separated by line feeds
            string wsList = "";
            if (weaList.Count > 0)
            {
                {
                    wsList += "WriteDate:\t" + ReportDate.ToString("hh:mm:ss");
                    wsList += "\r\n" + "TempScale:\t" + TempScale.ToString();
                    wsList += "\r\n" + "WindScale:\t" + WindScale.ToString();
                    wsList += "\r\n" + "SkyTemp:\t" + SkyTemp.ToString();
                    wsList += "\r\n" + "AmbTemp:\t" + AmbTemp.ToString();
                    wsList += "\r\n" + "SenTemp:\t" + SenTemp.ToString();
                    wsList += "\r\n" + "WindSpeed:\t" + WindSpeed.ToString();
                    wsList += "\r\n" + "Humidity:\t\t" + Humidity.ToString();
                    wsList += "\r\n" + "DewPoint:\t" + DewPoint.ToString();
                    wsList += "\r\n" + "DewHeat:\t" + DewHeat.ToString();
                    wsList += "\r\n" + "RainFlag:\t\t" + RainFlag.ToString();
                    wsList += "\r\n" + "WetFlag:\t\t" + WetFlag.ToString();
                    wsList += "\r\n" + "ElapsedTime:\t" + ElapsedSeconds.ToString();
                    wsList += "\r\n" + "LastDataWrite:\t" + LastDataWrite.ToString("hh:mm:ss");
                    wsList += "\r\n" + "Cloudiness:\t" + Cloudiness.ToString();
                    wsList += "\r\n" + "Windiness:\t" + Windiness.ToString();
                    wsList += "\r\n" + "Raininess:\t" + Raininess.ToString();
                    wsList += "\r\n" + "Darkness:\t" + Darkness.ToString();
                    wsList += "\r\n" + "RoofCloseFlag:\t" + RoofCloseFlag.ToString();
                    wsList += "\r\n" + "AlertFlag:\t\t" + AlertFlag.ToString();
                }
            }
            return wsList;
        }

        private List<string> ReadWeatherDataIn()
        {
            //Read the weather data line from the weather data file and returns as a list of strings
            string wfdata = "";
            try
            { wfdata = File.ReadAllText(weatherDataFilePath); }
            catch
            {
                //The only known reason for an error is if the file can't be read -- i.e. access collision
                //So return the last valid reading
                return LastValidReading;
            }
            List<string> wfd = wfdata.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < wfd.Count; i++)
            {
                if (wfd[i] == "") { wfd[i] = "0"; }
            }
            LastValidReading = wfd;
            return (wfd);
        }

        private string WeatherData(WeaData wDataField)
        {
            //Returns the data string to which the wData enum refers
            return (weaList[(int)wDataField]);
        }

        public bool IsWeatherSafe()
        {
            //Check the weather station, if enabled
            //  if safe (no worries is true) then return true, otherwise false
            // in weather station isn't enabled, then return true.

            WeatherUpdate();
            if (AlertFlag == WeatherReader.WeaAlert.Alert)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        #region Weather Data Properties

        public DateTime ReportDate
        {
            get
            {
                string weaDateTimeString = WeatherData(WeaData.WriteDate) + " " + WeatherData(WeaData.WriteTime);
                return (Convert.ToDateTime(weaDateTimeString));
            }
        }

        public WeaTempScale TempScale
        {
            get
            {
                if (WeatherData(WeaData.TempScale) == "F")
                { return (WeaTempScale.Farenheit); }
                else
                { return (WeaTempScale.Celsius); }
            }
        }

        public WeaWindScale WindScale
        {
            get
            {
                if (WeatherData(WeaData.WindScale) == "M")
                { return (WeaWindScale.MPH); }
                else
                { return (WeaWindScale.Knots); }
            }
        }

        public double SkyTemp
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.SkyTemp))); }
        }

        public double AmbTemp
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.AmbTemp))); }
        }

        public double SenTemp
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.SenTemp))); }
        }

        public double WindSpeed
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.WindSpeed))); }
        }

        public double Humidity
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.Humidity))); }
        }

        public double DewPoint
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.DewPoint))); }
        }

        public double DewHeat
        {
            get { return (Convert.ToDouble(WeatherData(WeaData.DewHeat))); }
        }

        public WeaRainFlag RainFlag
        {
            get { return ((WeaRainFlag)Convert.ToInt32(WeatherData(WeaData.RainFlag))); }
        }

        public WeaWetFlag WetFlag
        {
            get { return ((WeaWetFlag)Convert.ToInt32(WeatherData(WeaData.WetFlag))); }
        }

        public TimeSpan ElapsedSeconds
        {
            get
            {
                return (TimeSpan.FromSeconds(Convert.ToDouble(WeatherData(WeaData.ElapsedSeconds))));
            }
        }

        public DateTime LastDataWrite
        {
            get
            {
                string wstr = WeatherData(WeaData.LastDataWrite);
                double wdbl = Convert.ToDouble(wstr);
                long wlong = Convert.ToInt64(wdbl * TimeSpan.TicksPerDay);
                DateTime wdt = new DateTime(wlong);
                return (wdt);
            }
        }

        public WeaCloudiness Cloudiness
        {
            get { return (WeaCloudiness)Convert.ToInt32(WeatherData(WeaData.Cloudiness)); }
        }

        public WeaWindiness Windiness
        {
            get { return (WeaWindiness)Convert.ToInt32(WeatherData(WeaData.Windiness)); }
        }

        public WeaRaininess Raininess
        {
            get
            {
                return (WeaRaininess)Convert.ToInt32(WeatherData(WeaData.Raininess));
            }
        }

        public WeaDarkness Darkness
        {
            get { return (WeaDarkness)Convert.ToInt32(WeatherData(WeaData.Darkness)); }
        }

        public WeaRoofFlag RoofCloseFlag
        {
            get { return ((WeaRoofFlag)Convert.ToInt32(WeatherData(WeaData.RoofCloseFlag))); }
        }

        public WeaAlert AlertFlag
        {
            get { return ((WeaAlert)Convert.ToInt32(WeatherData(WeaData.AlertFlag))); }
        }
        #endregion
    }

}



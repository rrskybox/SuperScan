using AstroMath;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

// PlateSolve2.exe 
//  (Right ascension in radians),
//  (Declination in radians),
//  (x dimension in radians),
//  (y dimension in radians),
//  (Number of regions to search),
//  (fits filename),
//  (wait time at the end)

namespace SuperScan
{
    public partial class PS2ReRap
    {
        public class Coordinate
        {
            public double RA { get; set; }
            public double Dec { get; set; }
            public double Scale { get; set; }
            public double PA { get; set; }

        }

        // This procedure will throw an exception if PlateSolve2 is not installed at the psFilePath

        public static Coordinate StartPlateSolve(string ps2FilePath,
                                          double raHours,
                                          double decDegrees,
                                          double fieldWidth,
                                          double fieldHeight,
                                          int maxTiles,
                                          string solverPath,
                                          CancellationToken cancellationToken)
        {
            Coordinate coordinate = null;

            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = solverPath;
            proc.StartInfo.Arguments =
                AstroMath.Transform.HoursToRadians(raHours).ToString("0.00000", CultureInfo.InvariantCulture) + "," +
                AstroMath.Transform.DegreesToRadians(decDegrees).ToString("0.00000", CultureInfo.InvariantCulture) + "," +
                AstroMath.Transform.DegreesToRadians(fieldWidth / 60d).ToString("0.000", CultureInfo.InvariantCulture) + "," +
                AstroMath.Transform.DegreesToRadians(fieldHeight / 60d).ToString("0.000", CultureInfo.InvariantCulture) + "," +
                maxTiles +","+
                ps2FilePath + "," +
                "0";
            //_fileName = fileName;
            proc.Start();

            while (!proc.HasExited)
            {
                Thread.Sleep(1000);
                if (cancellationToken.IsCancellationRequested)
                {
                    proc.Kill();
                }
                cancellationToken.ThrowIfCancellationRequested();
            }

            //string apmFileName = Path.Combine(
            //Path.GetDirectoryName(_fileName),
            //Path.ChangeExtension(Path.GetFileNameWithoutExtension(_fileName), "apm")
            string apmFileName = Path.Combine(Path.GetDirectoryName(ps2FilePath),
                                              Path.ChangeExtension(Path.GetFileNameWithoutExtension(ps2FilePath), "apm"));
            coordinate = ReadApmFile(apmFileName);

            return coordinate;
        }


        private static Coordinate ReadApmFile(string fileName)
        {
            Coordinate result = null;

            try
            {
                var lines = File.ReadAllLines(fileName);
                if (lines.Count() >= 3 && lines[2].StartsWith("Valid"))
                {
                    string[] firstLine = HandleSeparators(lines[0]).Split(',');
                    string[] secondLine = HandleSeparators(lines[1]).Split(',');
                    result = new Coordinate
                    {
                        RA = Transform.RadiansToHours(double.Parse(firstLine[0], CultureInfo.InvariantCulture)),
                        Dec = Transform.RadiansToDegrees(double.Parse(firstLine[1], CultureInfo.InvariantCulture)),
                        Scale = double.Parse(secondLine[0], CultureInfo.InvariantCulture),
                        PA = double.Parse(secondLine[1], CultureInfo.InvariantCulture),
                    };
                }
            }
            catch (Exception)
            {

            }

            return result;
        }



        private static string HandleSeparators(string input)
        {
            //Handle case when system decimal separator is , and output looks like this:
            //0,88122,359,13,-1,00013,-0,00017,404
            string result = null;
            if (!input.Contains("."))
            {
                var split = input.Split(',');
                for (int i = 0; i < split.Count(); i++)
                {
                    if (i == split.Count() - 1)
                    {
                        result += split[i];
                    }
                    else if (i % 2 == 0)
                    {
                        result += split[i] + ".";
                    }
                    else
                    {
                        result += split[i] + ",";
                    }
                }
            }
            else
            {
                result = input;
            }

            return result;
        }
    }


}

using System;
using System.IO;
using System.Text;

namespace Utils
{
  public static class StringExtensions
  {

    /// <summary>
    /// Sucht die Datei mit dem Namen ausgehend vom Arbeitsverzeichnis der Anwendung
    /// bis zum Wurzelverzeichnis des Laufwerks und gibt den vollen Pfad zurück.
    /// Einsetzbar, um Dateien für Unittests zugreifbar zu machen (jeder Run des Tests erzeugt neues Verzeichnis)
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFullNameInApplicationTree(this string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        return null;
      }
      string path = Environment.CurrentDirectory;
      // string path = AppDomain.CurrentDomain.BaseDirectory;
      while (path != Directory.GetDirectoryRoot(path) && Directory.GetFiles(path, fileName).Length == 0)
      {
        path = Directory.GetParent(path).FullName;
      }
      if (Directory.GetFiles(path, fileName).Length > 0)  // Datei existiert
      {
        string fullName = Path.Combine(path, fileName);
        return fullName;
      }
      else
      {
        return null;
      }
    }


    /// <summary>
    /// Text in DoubleWert umwandeln
    /// </summary>
    /// <param name="text"></param>
    /// <returns>DoubleWert oder MinValue im Fehlerfall</returns>
    public static double ParseDoubleOrError(this string text)
    {
      if (!double.TryParse(text, out double wert))
      {
        wert = double.MinValue;
      }
      return wert;
    }


    /// <summary>
    /// Liest eine csv-Datei, die im Pfad liegt in ein zweidimensionales
    /// String-Array ein.
    /// </summary>
    /// <param name="fileName">Dateiname für Datei, die im Pfad der Anwendung/Test liegt</param>
    /// <param name="overreadTitleLine">enthält die csv-Datei eine zu überlesende Titelzeile</param>
    /// <returns>Zweidimensionales Stringarray zur Weiterbearbeitung</returns>
    public static string[][] ReadStringMatrixFromCsv(this string fileName, bool overreadTitleLine)
    {
      int startLine = 0;          // soll die Titelzeile überlesen werden startet der Zeilenzähler bei 1
      int subtractIndex = 0;      // und eine Zeile ist zu überlesen
      fileName = fileName.GetFullNameInApplicationTree(); // csv-Datei liegt im Projektverzeichnis
      if (fileName == null)
      {
        throw new FileNotFoundException("File " + fileName + " not found in applicationpath");
      }
      string[] lines = File.ReadAllLines(fileName, Encoding.Unicode);
      int lineCount = lines.Length;
      if (overreadTitleLine)
      {
        lineCount--;
        startLine = 1;
        subtractIndex = 1;
      }
      string[][] elements = new String[lineCount][];
      for (int line = startLine; line < lines.Length; line++)
      {
        elements[line - subtractIndex] = lines[line].Split(';');
      }
      return elements;
    }


  }
}

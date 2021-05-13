
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class StreamReaderPro 
{
    List<string> file = new List<string>();
    public int position = 0;
    string fileName;

    public StreamReaderPro(string filePath) 
    {
        fileName = filePath;
        StreamReader reader = File.OpenText(filePath);
        while(!reader.EndOfStream) { file.Add(reader.ReadLine()); }
        reader.Dispose();
    }

    public string ReadLine() { if (position < file.Count) { string result = file[position]; position++; return result; } else return "End of file."; }
    public string ReadLine(int lineNumber) { if (lineNumber < file.Count) return file[lineNumber]; else return "Line number does not exist."; }
    public string PeakLine() { return file[position]; }

    public bool EndOfStream() { return position == file.Count; }

    public void ChangeLineNumber(string lineToChange, string lineToPushDown, TowerPartInventorySlot slot)
	{
        int index = -1;
        for(int i = 0; i < file.Count; i++) if(file[i].Contains(lineToPushDown)) {  index = i; }
        foreach(string s in file)
		{
			if (s.Contains(lineToChange)) {  file.Remove(s); break; }
		}
        if(FMTowerPartEditor.editIsPathed && file.Count == index) { index--; }
        file.Insert(index, lineToChange);
        StreamWriter streamWriter = new StreamWriter(fileName);
        foreach(string s in file) { streamWriter.WriteLine(s); }
        streamWriter.Dispose();
    }

    public string ToString()
	{
        string result = "";
        foreach(string s in file)
		{
            result += s + "\n";
		}
        return result;
	}
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HobbitUtilz
{
	/// <summary>
	/// Stream Reader with advanced functionality.
	/// </summary>
	public class StreamReaderPro
	{
		readonly string _fileName;
		readonly List<string> _file = new List<string>();
		
		int _position;

		public StreamReaderPro(string filePath)
		{
			if (!File.Exists(filePath))
			{
				Debug.Log("[TPStreamReader] Could not be constructed. File: \"" + filePath + "\" does not exist.");
				return;
			}
			_fileName = filePath;
			StreamReader reader = File.OpenText(filePath);
			while (!reader.EndOfStream) { _file.Add(reader.ReadLine()); }
			reader.Dispose();
		}

		/// <summary>
		/// Returns current line of the stream reader and advances the position by 1.
		/// Returns "EOF" if line is end of file.
		/// </summary>
		/// <returns></returns>
		public string ReadLine()
		{
			if (_position >= _file.Count) return "EOF";
			string result = _file[_position];
			_position++;
			return result;
		}

		/// <summary>
		/// Returns requested line without advancing position.
		/// Returns "Error" if line number does not exist.
		/// </summary>
		/// <param name="lineNumber"></param>
		/// <returns></returns>
		public string ReadLine(int lineNumber)
		{
			if (lineNumber < _file.Count) return _file[lineNumber];
			Debug.Log("[TPStreamReader.ReadLine(" + lineNumber + ")] File does not contain line.");
			return "Error";
		}

		/// <summary>
		/// Returns line at current position without advancing position.
		/// Returns "EOF" if line is end of file.
		/// </summary>
		/// <returns></returns>
		public string PeakLine() { return _position < _file.Count ? _file[_position] : "EOF"; }

		public bool EndOfStream() { return _position == _file.Count; }

		/// <summary>
		/// Inserts lineToChange at position of lineToPushDown.
		/// </summary>
		/// <param name="lineToChange"></param>
		/// <param name="lineToPushDown"></param>
		public void ChangeLineNumber(string lineToChange, string lineToPushDown)
		{
			int index = GetLineNumber(lineToPushDown);
			foreach (string s in _file.Where(s => s.Contains(lineToChange))) { _file.Remove(s); break; }
			_file.Insert(index, lineToChange);
			Write();
		}

		/// <summary>
		/// Returns line number of a string, given it is a line of the file.
		/// Returns -1 if file does not contain the string.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public int GetLineNumber(string line)
		{
			for (int i = 0; i < _file.Count; i++) { if (_file[i] == line) { return i; } }
			Debug.Log("[TPStreamReader.GetLineNumber] The file does not contain line \"" + line + "\"");
			return -1;
		}

		/// <summary>
		/// Replaces old line with new line.
		/// </summary>
		/// <param name="oldLine"></param>
		/// <param name="newLine"></param>
		public void ReplaceLine(string oldLine, string newLine)
		{
			int index = GetLineNumber(oldLine);
			if (index > -1) { _file[index] = newLine; }
			else { Debug.Log("[TPStreamReader.ReplaceLine] The file does not contain line \"" + oldLine + "\""); }
			Write();
		}

		public override string ToString() { return _file.Aggregate("", (current, s) => current + (s + "\n")); }

		/// <summary>
		/// Writes to file.
		/// </summary>
		void Write()
		{
			StreamWriter streamWriter = new StreamWriter(_fileName);
			foreach (string s in _file) { streamWriter.WriteLine(s); }
			streamWriter.Dispose();
		}
	}
}
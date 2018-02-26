using System.Collections.Generic;
using System.IO;
using TodoApi.Models;
using System.Text;

namespace TodoApi.Parsers
{
    public class SDFileParser
    {
        string _fileName;

        public SDFileParser(string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException();

            _fileName = fileName;
        }

        public IEnumerable<SDFileItem> Parse()
        {
            //List<SDFileItem> items = new List<SDFileItem>();

            using (StreamReader sr = File.OpenText(_fileName))
            {
                List<string> lines = new List<string>();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("$$$$"))
                    {
                        yield return new SDFileItem(new List<string>(lines));
                        lines.Clear();
                    }
                    else
                    {
                        lines.Add(line);
                    }
                }
            }

            //return items;
        }
    }
}
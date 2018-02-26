
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Models
{
    public class Header
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Comment { get; set; }
    }

    public class SDFileItem
    {
        public Header Header { get; set; }
        public string CTab { get; set; }
        public string Version {get;set;}
        public Dictionary<string, List<string>> Properties { get; set; }
        public string SVG {get;set;}

        public SDFileItem(IEnumerable<string> lines)
        {
            Queue<string> q = new Queue<string>(lines);

            Properties = new Dictionary<string, List<string>>();
            Header = new Header() { Name = q.Dequeue(), Info = q.Dequeue(), Comment = q.Dequeue() };

            string versionLine = q.Dequeue();
            Version = versionLine.Split(' ').Last();

            CTab += Header.Name+ System.Environment.NewLine;
            CTab += Header.Info+ System.Environment.NewLine;
            CTab += Header.Comment+ System.Environment.NewLine;
            CTab += versionLine + System.Environment.NewLine;

            while (true)
            {
                var line = q.Dequeue();
                CTab += line + System.Environment.NewLine;
                if (line.Equals("M  END"))
                {
                    break;
                }
            }

            string currDict = null;
            while (q.Count > 0)
            {
                var line = q.Dequeue();
                if (string.IsNullOrEmpty(line)) continue;

                if (line.StartsWith("> <"))
                {
                    currDict = line.Substring(3).TrimEnd('>');
                    Properties.Add(currDict, new List<string>());
                }
                else
                {
                    Properties[currDict].Add(line);
                }
            }

        }
    }


}
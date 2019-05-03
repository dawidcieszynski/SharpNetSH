using SharpNetSH.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpNetSH
{
    internal class TabulatedObjectProcessor : IResponseProcessor
    {
		StandardResponse IResponseProcessor.ProcessResponse(IEnumerable<string> responseLines, int exitCode, string splitRegEx)
		{
			var lines = responseLines.ToList();
			var standardResponse = new StandardResponse();
			((IResponseProcessor)standardResponse).ProcessResponse(lines, exitCode);

			if (exitCode != 0) return standardResponse;

			var regex = new Regex(@"[ ]{4}");
			var tabulatedLines = lines.ToList().Select(x =>
			{
				while (Regex.IsMatch(x, @"^\t*([ ]{4})+")) // Ensures we are still working on a tab at the beginning of the line
					x = regex.Replace(x, "\t", 1);
				return x;
			}).ToList(); //Convert the beginning spaces to tabs

			if (tabulatedLines.Any(x => !Regex.IsMatch(x, @"^\t"))) // If any lines start with a tab level of 0, we need to tab everything over by 1 tab
				tabulatedLines = tabulatedLines.Select(x =>
				{
                    if (!StringExtension.IsNullOrWhiteSpace(x))
						return "\t" + x;
					return x;
				}).ToList();

			var root = new Tree();
			RecursivelyProcessToTree(tabulatedLines.Skip(3).GetEnumerator(), root, splitRegEx);
            standardResponse.ResponseObject = root;
			return standardResponse;
		}

        void RecursivelyProcessToTree(IEnumerator<string> lineEnumerator, Tree parent, string splitRegEx)
        {
            //We are in the scope of the owning tree here
            while (lineEnumerator.MoveNext())
            {
                var line = lineEnumerator.Current;
                if (StringExtension.IsNullOrWhiteSpace(line))
                    continue;

                var level = line.Length - line.TrimStart('\t').Length;
                var tree = new Tree(line.Trim(), level, splitRegEx) { Parent = parent };

                if (parent.TreeLevel == level - 1) // If the new tree is a child of this tree, add it to the parent
                    parent.Children.Add(tree);
                else if (level - 1 > parent.TreeLevel) // If the new tree is a distant child of this tree, recursively add it to the last tree we processed
                {
                    var newParent = parent.Children.Any() ? parent.Children.Last() : parent;
                    tree.Parent = newParent;
                    newParent.Children.Add(tree);
                    RecursivelyProcessToTree(lineEnumerator, tree, splitRegEx);
                }
                else // Otherwise, get the correct parent and recursively add it's children in
                {
                    while (parent.TreeLevel >= 0 && parent.TreeLevel > ((level - 1 < 0 ? 0 : level - 1)))
                        parent = parent.Parent;

                    parent.Children.Add(tree);
                }
            }
        }
    }
}
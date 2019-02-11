﻿using SharpNetSH.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SharpNetSH
{
    internal class BlockProcessor : IResponseProcessor
    {
        StandardResponse IResponseProcessor.ProcessResponse(IEnumerable<string> responseLines, int exitCode, string splitRegEx = null)
        {
            var lines = responseLines.ToList();
            var standardResponse = new StandardResponse();
            ((IResponseProcessor)standardResponse).ProcessResponse(lines, exitCode);

            if (exitCode != 0) return standardResponse;

            var objects = new List<dynamic>();
            var currentObjectRows = new List<string>();

            foreach (var line in lines.Skip(3))
            {
                if (StringExtension.IsNullOrWhiteSpace(line))
                {
                    if (currentObjectRows.Count > 0)
                        objects.Add(currentObjectRows.ProcessRawData(splitRegEx));
                    currentObjectRows = new List<string>();
                }
                else
                    currentObjectRows.Add(line);
            }

            standardResponse.ResponseObject = objects;
            return standardResponse;
        }
    }
}
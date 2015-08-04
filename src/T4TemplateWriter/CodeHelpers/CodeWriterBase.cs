﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vipr.Core.CodeModel;
using Vipr.T4TemplateWriter.Extensions;

namespace Vipr.T4TemplateWriter.CodeHelpers
{
    abstract public class CodeWriterBase
    {

        private String defaultHeader =
@"**NOTE** This code was generated by a tool and will occasionally be
overwritten. We welcome comments and issues regarding this code; they will be
addressed in the generation tool. If you wish to submit pull requests, please
do so for the templates in that tool.

This code was generated by Vipr (https://github.com/microsoft/vipr) using
the T4TemplateWriter (https://github.com/msopentech/vipr-t4templatewriter).

Copyright (c) Microsoft Open Technologies, Inc. All Rights Reserved.
Licensed under the Apache License 2.0; see LICENSE in the source repository
root for authoritative license information.﻿";


        public OdcmModel CurrentModel { get; set; }

        public CodeWriterBase() : this(null) { }

        public CodeWriterBase(OdcmModel model)
        {
            this.CurrentModel = model;
        }

        public static String Write(params String[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String arg in args)
            {
                sb.Append(arg);
            }
            return sb.ToString();
        }

        abstract public String WriteOpeningCommentLine();
        abstract public String WriteClosingCommentLine();

        abstract public String WriteInlineCommentChar();

        public String WriteHeader()
        {
            return Write(new String[] {
                WriteOpeningCommentLine(),
                defaultHeader,
                WriteClosingCommentLine()
              });
        }

        public Node GetModelGraph(OdcmModel model)
        {
            Node n = new Node(null, null);
            foreach (var odcmProperty in model.EntityContainer.Properties)
            {
                n.ChildProperties.Add(new Node(n, odcmProperty));
            }

            n.GenerateGraph();

            return n;
        }

        public String GetTestName(Node prop)
        {
            var testName = "";
            if (prop != null && prop.Property != null)
            {
                var propName = prop.Property.IsCollection ? prop.Property.Name : prop.Property.Name.Singularize();
                testName += GetTestName(prop.Parent) + propName;
            }

            return testName;
        }
    }
}

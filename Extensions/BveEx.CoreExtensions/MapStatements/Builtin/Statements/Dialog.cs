﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

namespace BveEx.Extensions.MapStatements.Builtin.Statements
{
    internal class Dialog : IParser
    {
        private static readonly ClauseFilter RootFilter = ClauseFilter.Element("Dialog", 0);
        private static readonly ClauseFilter ShowFilter = ClauseFilter.Function("Show", 1);

        public bool CanParse(Statement statement) => statement.IsOfficialStatement(RootFilter);

        public void Parse(Statement statement)
        {
            IReadOnlyList<MapStatementClause> clauses = statement.Source.Clauses;
            if (clauses.Count != 3) throw new SyntaxException(statement);
            if (clauses[0].Keys.Count != 0) throw new SyntaxException(statement);

            if (statement.IsOfficialStatement(RootFilter, ShowFilter))
            {
                MessageBox.Show(clauses[2].Args[0].ToString());
            }
            else
            {
                throw new SyntaxException(statement);
            }
        }
    }
}

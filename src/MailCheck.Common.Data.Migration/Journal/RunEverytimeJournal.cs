using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DbUp.Engine;

namespace MailCheck.Common.Data.Migration.UpgradeEngine
{
    public class RunEverytimeJournal : IJournal
    {
        private readonly IJournal _innerJournal;

        public RunEverytimeJournal(IJournal innerJournal)
        {
            _innerJournal = innerJournal;
        }

        public void EnsureTableExistsAndIsLatestVersion(Func<IDbCommand> dbCommandFactory)
        {
            _innerJournal.EnsureTableExistsAndIsLatestVersion(dbCommandFactory);
        }

        public string[] GetExecutedScripts()
        {
            List<string> executedScripts = _innerJournal.GetExecutedScripts().ToList();

            return executedScripts.Where(x => !x.Contains("everytime")).ToArray();
        }

        public void StoreExecutedScript(SqlScript script, Func<IDbCommand> dbCommandFactory)
        {   
            if (!script.Name.Contains("everytime"))
            {
                _innerJournal.StoreExecutedScript(script, dbCommandFactory);
            }
        }
    }
}
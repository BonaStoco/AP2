using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindSubTerminalById", @"Select subterminalid,subterminalname,terminalid FROM mappingsubterminal WHERE subterminalid = @subterminalid")]
    [NamedSqlQuery("FindSubTerminalByCategoryId", @"Select subterminalid,subterminalname,terminalid FROM mappingsubterminal WHERE categoryid=@categoryid")]
    [NamedSqlQuery("FindSubTerminalByCategoryIdAndTerminalId", @"Select subterminalid,subterminalname,terminalid FROM mappingsubterminal WHERE categoryid=@categoryid AND terminalid=@terminalid")]
    [NamedSqlQuery("FindSubTerminalByCategoryIdAndLocationId", @"Select subterminalid,subterminalname,terminalid FROM mappingsubterminal WHERE categoryid=@categoryid AND locationid=@locationid")]
    public class MappingSubTerminal:IViewModel
    {
        public int SubTerminalId { get; set; }
        public string SubTerminalName { get; set; }
        public int TerminalId { get; set; }
    }
}

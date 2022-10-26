using System.Collections.Generic;

namespace MailCheck.Common.Contracts.Findings
{
    public class Finding
    {
        // A URI for the entity this finding relates to e.g. a domain, a dkim selector, an MX host, etc
        public string EntityUri { get; set; }

        // Map of parameters related to the entity e.g. might contain DKIM selector, certificate thumbprint, etc
        public IDictionary<string, object> EntityParams { get; set; }

        // Mail Check URL that points to the entity
        public string SourceUrl { get; set; }

        // Finding human-readable name e.g. "mailcheck.dmarc.noDmarcRecordFound"
        public string Name { get; set; }

        // Urgent, Advisory, Informational, Positive
        public string Severity { get; set; }

        // English default title for the finding
        public string Title { get; set; }
    }
}
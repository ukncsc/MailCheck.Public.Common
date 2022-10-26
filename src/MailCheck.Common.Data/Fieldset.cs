using System.Collections.Generic;

namespace MailCheck.Common.Data
{
    public class Fieldset : List<string>
    {
        public static readonly Fieldset Empty = new Fieldset();

        public Fieldset() : base()
        {
        }

        public Fieldset(IEnumerable<string> collection) : base(collection)
        {
        }
    }
}
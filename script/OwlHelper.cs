using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OwlImport
{
    public class OwlHelper
    {
        public static string ToIRI(string value)
        {
            var s = value.ToLower();
            return Regex.Replace(s, "[^a-zA-Z0-9]", "_"); //remove non-breaking space and other stuff
        }

        public static string DataPropertyAssertion_String(string prop, string namedIndividualIRI, string propValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<DataPropertyAssertion>");
            sb.AppendLine("<DataProperty IRI=\"#" + prop + "\" />");
            sb.AppendLine("<NamedIndividual IRI=\"#" + namedIndividualIRI + "\" />");
            sb.AppendLine("<Literal datatypeIRI=\"&xsd;string\">" + propValue + "</Literal>");
            sb.AppendLine("</DataPropertyAssertion>");

            return sb.ToString();
        }

        public static string DataPropertyAssertion_Integer(string prop, string namedIndividualIRI, int propValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<DataPropertyAssertion>");
            sb.AppendLine("<DataProperty IRI=\"#" + prop + "\" />");
            sb.AppendLine("<NamedIndividual IRI=\"#" + namedIndividualIRI + "\" />");
            sb.AppendLine("<Literal datatypeIRI=\"&xsd;integer\">" + propValue + "</Literal>");
            sb.AppendLine("</DataPropertyAssertion>");

            return sb.ToString();
        }
    }
}

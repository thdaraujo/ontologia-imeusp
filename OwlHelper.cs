using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport
{
    public class OwlHelper
    {
        public static string ToIRI(string value)
        {
            return value.ToLower()
                .Replace(" ", "_")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(".", "")
                .Replace("\"", "")
                .Replace("/", "")
                .Replace("-", "_")
                .Replace("ú", "u")
                .Replace("'", "")
                .Replace("&", "")
                .Replace("á", "a")
                .Replace("ã", "a")
                .Replace("é", "e")
                .Replace("ê", "e")
                .Replace("í", "i")
                .Replace("õ", "o")
                .Replace("ó", "o")
                .Replace("ç", "c");
        }

        public static string DataPropertyAssertion_String(string prop, string namedIndividualIRI, string propValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<DataPropertyAssertion>");
            sb.AppendLine("<DataProperty IRI=\"#" + prop + "\" />");
            sb.AppendLine("<NameIndividual IRI=\"#" + namedIndividualIRI + "\" />");
            sb.AppendLine("<Literal dataTypeIRI=\"&xsd;string\">" + propValue + "</Literal>");
            sb.AppendLine("</DataPropertyAssertion>");

            return sb.ToString();
        }

        public static string DataPropertyAssertion_Integer(string prop, string namedIndividualIRI, int propValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<DataPropertyAssertion>");
            sb.AppendLine("<DataProperty IRI=\"#" + prop + "\" />");
            sb.AppendLine("<NameIndividual IRI=\"#" + namedIndividualIRI + "\" />");
            sb.AppendLine("<Literal dataTypeIRI=\"&xsd;integer\">" + propValue + "</Literal>");
            sb.AppendLine("</DataPropertyAssertion>");

            return sb.ToString();
        }
    }
}

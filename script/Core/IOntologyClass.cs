using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Core
{
    public interface IOntologyIndividual
    {
        string GetOWLDataProperties(string iri);
        string IRI { get; }         
    }
}

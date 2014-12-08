using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Core
{
    public class Universidade : IOntologyIndividual
    {
        public string nome_completo { get; set; }

        public Pais pais { get; set; }

        public string IRI
        {
            get;
            private set;
        }

        public Universidade(string iri, Pais pais)
        {
            this.IRI = iri;
            this.pais = pais;
        }

        public string GetOWLDataProperties(string iri)
        {
            return OwlHelper.DataPropertyAssertion_String("nome_completo", iri, this.nome_completo);
        }

    }
}

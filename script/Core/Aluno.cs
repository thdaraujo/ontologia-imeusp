using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Core
{
    class Aluno : IOntologyIndividual
    {        
        public string nome_completo { get; set; }
        
        public string IRI
        {
            get;
            private set;
        }

        public Aluno(string iri, string nomeCompleto)
        {
            this.nome_completo = nomeCompleto;
            this.IRI = iri;
        }

        public string GetOWLDataProperties(string iri)
        {
            return OwlHelper.DataPropertyAssertion_String("nome_completo", iri, this.nome_completo);
        }
    }
}

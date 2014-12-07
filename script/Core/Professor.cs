using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Core
{
    public class Professor : IOntologyIndividual
    {
        public string nome_completo { get; set; }
        public string nome_citacao { get; set; }

        public string IRI
        {
            get;
            private set;
        }

        public Professor(string iri)
        {
            this.IRI = iri;
        }

        /**
         *  <DataPropertyAssertion>
             <DataProperty IRI="#nome_completo"/>
             <NamedIndividual IRI="#Renata_Wassermann"/>
             <Literal datatypeIRI="&xsd;string">Renata Wassermann</Literal>
            </DataPropertyAssertion> 
         */
        public string GetOWLDataProperties(string iri)
        {
            return OwlHelper.DataPropertyAssertion_String("nome_completo", iri, this.nome_completo) +
                   OwlHelper.DataPropertyAssertion_String("nome_citacao", iri, this.nome_citacao);
        }
    }
}

using OwlImport.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Relations
{
    class OntologyRelation : IOntologyRelation
    {    
        public string Individual { get; set; }    
        public string Relation { get; set; }        
        public string RelatedTo { get; set; }

        public OntologyRelation(IOntologyIndividual individual, string relation, IOntologyIndividual relatedTo)
        {            
            this.Individual = individual.IRI;
            this.Relation = relation;
            this.RelatedTo = relatedTo.IRI;
        }

        /*
             <ObjectPropertyAssertion>
                <ObjectProperty IRI="#oferecido_por"/>
                <NamedIndividual IRI="#Graduação_em_Ciência_da_Computação"/>
                <NamedIndividual IRI="#Departamento_de_Ciência_da_Computação"/>
            </ObjectPropertyAssertion>

         */
        public string getRelationOWL()
        {
            const string assertion = 
                        @"
                             <ObjectPropertyAssertion>
                                <ObjectProperty IRI=""#{0}"" />
                                <NamedIndividual IRI=""#{1}"" />
                                <NamedIndividual IRI=""#{2}"" />
                             </ObjectPropertyAssertion>
                         ";

            return string.Format(assertion, this.Relation, this.Individual, this.RelatedTo);
        }

        public override int GetHashCode()
        {
            return (this.Individual + this.Relation + this.RelatedTo).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is OntologyRelation)
            {
                return (obj as OntologyRelation).GetHashCode() == this.GetHashCode();
            }
            return false;
        }
    }
}

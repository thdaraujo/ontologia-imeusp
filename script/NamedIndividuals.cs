using OwlImport.Core;
using OwlImport.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport
{
    public class NamedIndividuals
    {
        private static NamedIndividuals _instance;
        public static NamedIndividuals Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NamedIndividuals();

                return _instance;
            }
        }

        public Dictionary<string, IOntologyIndividual> Profs { get; set; }
        public Dictionary<string, IOntologyIndividual> Alunos { get; set; }
        public Dictionary<string, IOntologyIndividual> Cursos { get; set; }
        public Dictionary<string, IOntologyIndividual> Paises { get; set; }
        public Dictionary<string, IOntologyIndividual> Universidades { get; set; }
        public Dictionary<string, IOntologyIndividual> Revistas { get; set; }
        public Dictionary<string, IOntologyIndividual> Artigos { get; set; }
        public Dictionary<string, IOntologyIndividual> Conferencias { get; set; }
        public Dictionary<string, IOntologyIndividual> Simposios { get; set; }

        public HashSet<IOntologyRelation> Relations { get; set; }

        private NamedIndividuals()
        {
            this.Profs = new Dictionary<string, IOntologyIndividual>();
            this.Alunos = new Dictionary<string, IOntologyIndividual>();
            this.Cursos = new Dictionary<string, IOntologyIndividual>();
            this.Paises = new Dictionary<string, IOntologyIndividual>();
            this.Universidades = new Dictionary<string, IOntologyIndividual>();
            this.Revistas = new Dictionary<string, IOntologyIndividual>();
            this.Artigos = new Dictionary<string, IOntologyIndividual>();
            this.Conferencias = new Dictionary<string, IOntologyIndividual>();
            this.Simposios = new Dictionary<string, IOntologyIndividual>();

            this.Relations = new HashSet<IOntologyRelation>();
        }

        /*
         * 
           <Declaration>
            <NamedIndividual IRI="#Guilherme_Rey"/>
           </Declaration>
         * 
        */

        private string GenerateDeclaration(Dictionary<string, IOntologyIndividual> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string iri in dict.Keys)
            {
                string IRI = (string)iri;

                sb.AppendLine("<Declaration>");                
                sb.AppendLine(" <NamedIndividual IRI=\"#" + IRI + "\" />");
                sb.AppendLine("</Declaration>");                 
            }

            return sb.ToString();
        }

        /*
         * <ClassAssertion>
                <Class IRI="#Aluno"/>
                <NamedIndividual IRI="#Guilherme_Rey"/>
           </ClassAssertion>
         * */

        private string GenerateClassAssertion(string classIRI, Dictionary<string, IOntologyIndividual> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string iri in dict.Keys)
            {
                string IRI = (string)iri;

                sb.AppendLine("<ClassAssertion>");
                sb.AppendLine(" <Class IRI=\"#" + classIRI + "\" />");
                sb.AppendLine(" <NamedIndividual IRI=\"#" + IRI + "\" />");
                sb.AppendLine("</ClassAssertion>");                
            }

            return sb.ToString();
        }


        /*
         
         <ObjectPropertyAssertion>
            <ObjectProperty IRI="#membros"/>
            <NamedIndividual IRI="#LIAMF"/>
            <NamedIndividual IRI="#Renata_Wassermann"/>
        </ObjectPropertyAssertion>
         
         */

        private string GenerateObjectPropertyAssertions(HashSet<IOntologyRelation> relations)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IOntologyRelation relation in relations)
            {
                sb.AppendLine(relation.getRelationOWL());
            }
            return sb.ToString();
        }

        /*         
        <DataPropertyAssertion>
            <DataProperty IRI="#sobrenome"/>
            <NamedIndividual IRI="#Renata_Wassermann"/>
            <Literal datatypeIRI="&xsd;string">Wassermann</Literal>
        </DataPropertyAssertion>         
         */
        
        private string GenerateDataPropertyAssertions(Dictionary<string, IOntologyIndividual> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string iri in dict.Keys)
            {
                sb.AppendLine(dict[iri].GetOWLDataProperties(iri));
            }
            return sb.ToString();
        }   
                
        internal string GetOWLNamedIndividualDeclarations()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Profs));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Alunos));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Cursos));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Paises));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Universidades));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Revistas));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Artigos));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Conferencias));
            sb.AppendLine(GenerateDeclaration(NamedIndividuals.Instance.Simposios));

            return sb.ToString();
        }

        public string GetOWLClassAssertions()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GenerateClassAssertion("Professor", NamedIndividuals.Instance.Profs));
            sb.AppendLine(GenerateClassAssertion("Aluno", NamedIndividuals.Instance.Alunos));
            sb.AppendLine(GenerateClassAssertion("Curso", NamedIndividuals.Instance.Cursos));
            sb.AppendLine(GenerateClassAssertion("Pais", NamedIndividuals.Instance.Paises));
            sb.AppendLine(GenerateClassAssertion("Universidade", NamedIndividuals.Instance.Universidades));
            sb.AppendLine(GenerateClassAssertion("Revista", NamedIndividuals.Instance.Revistas));
            sb.AppendLine(GenerateClassAssertion("Artigo", NamedIndividuals.Instance.Artigos));
            sb.AppendLine(GenerateClassAssertion("Conferencia", NamedIndividuals.Instance.Conferencias));
            sb.AppendLine(GenerateClassAssertion("Simposio", NamedIndividuals.Instance.Simposios));

            return sb.ToString();
        }

        public string GetOWLDataPropertyAssertions()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Profs));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Alunos));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Cursos));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Paises));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Universidades));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Revistas));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Artigos));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Conferencias));
            sb.AppendLine(GenerateDataPropertyAssertions(NamedIndividuals.Instance.Simposios));

            return sb.ToString();
        }
        public string GetOWLObjectPropertyAssertions()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GenerateObjectPropertyAssertions(NamedIndividuals.Instance.Relations));

            return sb.ToString();
        }       
    }
}

using OwlImport.Core;
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

        public Dictionary<string, OntologyClass> Profs { get; set; }
        public Dictionary<string, OntologyClass> Cursos { get; set; }
        public Dictionary<string, OntologyClass> Paises { get; set; }
        public Dictionary<string, OntologyClass> Universidades { get; set; }
        public Dictionary<string, OntologyClass> Revistas { get; set; }
        public Dictionary<string, OntologyClass> Artigos { get; set; }
        public Dictionary<string, OntologyClass> Conferencias { get; set; }
        public Dictionary<string, OntologyClass> Simposios { get; set; }

        private NamedIndividuals()
        {
            this.Profs = new Dictionary<string, OntologyClass>();
            this.Cursos = new Dictionary<string, OntologyClass>();
            this.Paises = new Dictionary<string, OntologyClass>();
            this.Universidades = new Dictionary<string, OntologyClass>();
            this.Revistas = new Dictionary<string, OntologyClass>();
            this.Artigos = new Dictionary<string, OntologyClass>();
            this.Conferencias = new Dictionary<string, OntologyClass>();
            this.Simposios = new Dictionary<string, OntologyClass>();
        }

        /*
         * 
           <Declaration>
            <NamedIndividual IRI="#Guilherme_Rey"/>
           </Declaration>
         * 
        */

        private string GenerateDeclaration(Dictionary<string, OntologyClass> dict)
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

        private string GenerateClassAssertion(string classIRI, Dictionary<string, OntologyClass> dict)
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

        private string GenerateObjectPropertyAssertions(string classIRI, Dictionary<string, OntologyClass> dict)
        {
           //TODO
            return string.Empty;
        }


        /*         
        <DataPropertyAssertion>
            <DataProperty IRI="#sobrenome"/>
            <NamedIndividual IRI="#Renata_Wassermann"/>
            <Literal datatypeIRI="&xsd;string">Wassermann</Literal>
        </DataPropertyAssertion>         
         */
        
        private string GenerateDataPropertyAssertions(Dictionary<string, OntologyClass> dict)
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

            //TODO

            return sb.ToString();
        }       
    }
}

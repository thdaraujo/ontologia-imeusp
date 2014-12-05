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
                sb.AppendLine("<Class IRI=\"#" + classIRI + "\" />");
                sb.AppendLine("<NamedIndividual IRI=\"#" + IRI + "\" />");
                sb.AppendLine("</ClassAssertion>");
                sb.AppendLine(dict[iri].GetOWLDataProperties(iri));
            }

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
    }
}

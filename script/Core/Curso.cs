using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Core
{
    public class Curso : IOntologyIndividual
    {
        public string titulo { get; set; }
        public TipoCurso tipo_curso { get; set; }

        public string IRI
        {
            get;
            private set;
        }

        public Curso(string iri)
        {
            this.IRI = iri;
        }

        public string GetOWLDataProperties(string iri)
        {
            return OwlHelper.DataPropertyAssertion_String("titulo", iri, this.titulo) +
                OwlHelper.DataPropertyAssertion_String("tipo_curso", iri, tipo_curso == TipoCurso.graduacao ? "graduacao" : "pos_graduacao");
        }
    }
}

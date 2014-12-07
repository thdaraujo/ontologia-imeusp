﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Core
{
    public class Artigo : OntologyClass
    {
        public string titulo { get; set; }
        public int Ano { get; set; }

        public string GetOWLDataProperties(string iri)
        {
            return OwlHelper.DataPropertyAssertion_String("titulo", iri, this.titulo) +
                OwlHelper.DataPropertyAssertion_Integer("ano", iri, this.Ano);
        }
    }
}
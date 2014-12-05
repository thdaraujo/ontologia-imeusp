using OwlImport.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Relations
{
    public class Autor
    {
        public Pessoa Pessoa { get; set; }
        public Artigo Artigo { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport.Relations
{
    public interface IOntologyRelation
    {
        string getRelationOWL();
        int GetHashCode();
        bool Equals(object a);
    }
}

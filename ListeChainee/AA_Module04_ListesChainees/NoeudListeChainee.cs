using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA_Module04_ListesChainees
{
    public class NoeudListeChainee<TypeElement>
    {
        // ***** PROPRIÉTÉS ***** //
        public NoeudListeChainee<TypeElement> Suivant { get; set; }
        public TypeElement Valeur { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlImport
{
    class Program
    {
        static void Main(string[] args)
        {
            OwlReader reader = new OwlReader(@"C:\Users\Grey\documents\visual studio 2013\Projects\OwlImport\OwlImport\ref\database.xml");
            reader.IdentifyIndividuals();

            try
            {
                TextWriter textWriter = new StreamWriter("namedIndividuals.owl");
                textWriter.Write(NamedIndividuals.Instance.GetOWLClassAssertions());
                textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}

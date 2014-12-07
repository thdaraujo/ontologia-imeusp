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
            string sourceFile = Environment.CurrentDirectory + @"\ref\database.xml";
            Console.WriteLine("Gerando arquivo da ontologia a partir de " + sourceFile);

            OwlReader reader = new OwlReader(sourceFile);
            reader.IdentifyIndividuals();

            NamedIndividuals individuals = NamedIndividuals.Instance;
            OutputOntology(individuals);
            Console.ReadLine();
        }

        private static void OutputOntology(NamedIndividuals individuals)
        {
            try
            {
                StringBuilder template = new StringBuilder(File.ReadAllText(Environment.CurrentDirectory + @"\ref\ontologia-template.owl"));

                StringBuilder builder = new StringBuilder();
                builder.AppendLine(individuals.GetOWLNamedIndividualDeclarations());
                builder.AppendLine(individuals.GetOWLClassAssertions());
                builder.AppendLine(individuals.GetOWLDataPropertyAssertions());
                //builder.AppendLine(individuals.GetOWLObjectPropertyAssertions());

                template.Replace("{{ADD-DATA}}", builder.ToString());

                string filename = string.Format("ontology-{0}.owl", DateTime.Now.ToString("ddMMyyy-hh-mm-ss"));
                using (var textWriter = new StreamWriter(filename))
                {
                    textWriter.Write(template.ToString());
                }

                Console.WriteLine("Arquivo gerado com sucesso: " + filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

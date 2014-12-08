using OwlImport.Core;
using OwlImport.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace OwlImport
{
    public class OwlReader
    {
        public XmlDocument XmlDoc { get; set; }
        public string FilePath { get; set; }

        public OwlReader(string filePath)
        {
            this.FilePath = filePath;
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.Load(this.FilePath);
        }

        public void IdentifyIndividuals()
        {
            foreach (XmlNode pesquisador in this.XmlDoc.SelectNodes("//curriculo_lattes/pesquisador"))
            {
                #region Identificando Pesquisadores
                string id = pesquisador.Attributes["id"].Value;
                if (!NamedIndividuals.Instance.Profs.ContainsKey(id))
                {
                    Professor auxProf = new Professor(OwlHelper.ToIRI(id));
                    XmlNode ident = pesquisador.SelectSingleNode("identificacao");
                    if (ident != null)
                    {
                        auxProf.nome_completo = ident["nome_completo"].InnerText;
                        auxProf.nome_citacao = ident["nome_citacao_bibliografica"].InnerText;
                    }

                    NamedIndividuals.Instance.Profs.Add(id, auxProf);
                }
                #endregion

                #region Identificando Cursos, Países e Universidades

                foreach (XmlNode formacao in pesquisador.SelectNodes("formacao_academica/formacao"))
                {
                    CheckCurso(formacao["tipo"]);
                    CheckUniversidadePais(formacao["nome_instituicao"]);
                }

                #endregion

                #region Identificando Artigos, revistas e conferências

                foreach (XmlNode artigo in pesquisador["artigos_em_periodicos"].SelectNodes("artigo"))
                {
                    /* artigo node:
                     *    <doi>http://dx.doi.org/10.1111/hae.12484</doi>
                          <titulo>A quantitative and humane tail bleeding assay for efficacy evaluation of antihaemophilic factors in haemophilia A mice</titulo>
                          <autores>MOLINA, E. ; Fujita, André ; SOGAYAR, Mari Cleide ; DEMASI, Marcos Angelo A.</autores>
                          <revista>Haemophilia (Oxford. Print)</revista>
                          <volume>Haemophilia (Oxford. Print)</volume>
                          <paginas>392-398</paginas>
                          <numero></numero>
                          <ano>2014</ano>
                     */
                    string artigo_iri = OwlHelper.ToIRI(artigo["titulo"].InnerText);
                    if (!NamedIndividuals.Instance.Artigos.ContainsKey(artigo_iri))
                    {
                        NamedIndividuals.Instance.Artigos.Add(artigo_iri, new Artigo(artigo_iri)
                        {
                            Ano = Convert.ToInt32(artigo["ano"].InnerText),
                            titulo = artigo["titulo"].InnerText
                        });
                    }

                    string revista_iri = OwlHelper.ToIRI(artigo["revista"].InnerText);
                    if (!NamedIndividuals.Instance.Revistas.ContainsKey(revista_iri))
                    {
                        NamedIndividuals.Instance.Revistas.Add(revista_iri, new Revista(revista_iri)
                        {
                            titulo = artigo["revista"].InnerText
                        });
                    }
                }

                foreach (XmlNode trabalhoCongresso in pesquisador["trabalho_completo_congresso"].SelectNodes("trabalho_completo"))
                {
                    #region Identificando Conferência/Simpósio
                    string evento = trabalhoCongresso["nome_evento"].InnerText.ToLower();
                    string conf_iri = OwlHelper.ToIRI(trabalhoCongresso["nome_evento"].InnerText);
                    TipoCongresso tipoTrabalho = evento.Contains("conference") ? TipoCongresso.Conferencia :
                        evento.Contains("conferência") ? TipoCongresso.Conferencia :
                        evento.Contains("simpósio") ? TipoCongresso.Simposio :
                        evento.Contains("symposium") ? TipoCongresso.Simposio : TipoCongresso.Conferencia;

                    switch (tipoTrabalho)
                    {
                        case TipoCongresso.Conferencia:
                            if (!NamedIndividuals.Instance.Conferencias.ContainsKey(conf_iri))
                            {
                                NamedIndividuals.Instance.Conferencias.Add(conf_iri, new Conferencia(conf_iri)
                                {
                                    titulo = trabalhoCongresso["nome_evento"].InnerText,
                                    ano = Convert.ToInt32(trabalhoCongresso["ano"].InnerText)
                                });
                            }
                            break;
                        case TipoCongresso.Simposio:
                            if (!NamedIndividuals.Instance.Simposios.ContainsKey(conf_iri))
                            {
                                NamedIndividuals.Instance.Simposios.Add(conf_iri, new Simposio(conf_iri)
                                {
                                    titulo = trabalhoCongresso["nome_evento"].InnerText,
                                    ano = Convert.ToInt32(trabalhoCongresso["ano"].InnerText)
                                });
                            }
                            break;
                    }

                    #endregion

                    #region Identificando o Artigo da Conferência
                    string artigoIri_conf = OwlHelper.ToIRI(trabalhoCongresso["titulo"].InnerText);
                    if (!NamedIndividuals.Instance.Artigos.ContainsKey(artigoIri_conf))
                    {
                        NamedIndividuals.Instance.Artigos.Add(artigoIri_conf, new Artigo(artigoIri_conf)
                        {
                            titulo = trabalhoCongresso["titulo"].InnerText,
                            Ano = Convert.ToInt32(trabalhoCongresso["ano"].InnerText)
                        });
                    }
                    #endregion
                }

                #endregion
            }
        }

        public void IdentifyRelations()
        {
            foreach (XmlNode pesquisador in this.XmlDoc.SelectNodes("//curriculo_lattes/pesquisador"))
            {
                string pesquisadorId = pesquisador.Attributes["id"].Value;
                var pesquisadorIndividual = NamedIndividuals.Instance.Profs[pesquisadorId];

                CheckAcademicHistory(pesquisador, pesquisadorIndividual);
                CheckWorkHistory(pesquisador, pesquisadorIndividual);
                CheckJournalsPublications(pesquisador, pesquisadorIndividual);
                CheckEventsPublications(pesquisador);
            }
        }           

        private void CheckAcademicHistory(XmlNode pesquisador, IOntologyIndividual pesquisadorIndividual)
        {
            foreach (XmlNode formacao in pesquisador.SelectNodes("formacao_academica/formacao"))
            {
                var curso = GetCurso(formacao["tipo"]);
                var universidade = GetUniversidadePais(formacao["nome_instituicao"]);

                var cursoIndividual = NamedIndividuals.Instance.Cursos[curso.IRI];
                var universidadeIndividual = NamedIndividuals.Instance.Universidades[universidade.IRI];
                var paisIndividual = NamedIndividuals.Instance.Paises[universidade.pais.IRI];

                IOntologyRelation cursouRelation = new OntologyRelation(pesquisadorIndividual, OntologyRelationType.Cursou, cursoIndividual);
                IOntologyRelation estudouEmRelation = new OntologyRelation(pesquisadorIndividual, OntologyRelationType.EstudouEm, universidadeIndividual);
                IOntologyRelation oferecidoPorRelation = new OntologyRelation(cursoIndividual, OntologyRelationType.OferecidoPor, universidadeIndividual);
                IOntologyRelation localizadoRelation = new OntologyRelation(universidadeIndividual, OntologyRelationType.Localizado, paisIndividual);

                AddRelation(cursouRelation);
                AddRelation(estudouEmRelation);
                AddRelation(oferecidoPorRelation);
                AddRelation(localizadoRelation);
            }
        }

        private void CheckWorkHistory(XmlNode pesquisador, IOntologyIndividual pesquisadorIndividual)
        {
            var trabalhoNode = pesquisador.SelectSingleNode("endereco/endereco_profissional");            
            if(trabalhoNode == null) return;

            var trabalho = trabalhoNode.InnerText;
            if (string.IsNullOrEmpty(trabalho)) return;

            var info = trabalho
                .Split(',')
                .FirstOrDefault();
            var universidade = OwlHelper.ToIRI(info);

            if (NamedIndividuals.Instance.Universidades.ContainsKey(universidade))
            {
                var universidadeIndividual = NamedIndividuals.Instance.Universidades[OwlHelper.ToIRI(info)];
                IOntologyRelation trabalhouEmRelation = new OntologyRelation(pesquisadorIndividual, OntologyRelationType.TrabalhouEm, universidadeIndividual);
                AddRelation(trabalhouEmRelation);
            }
        }

        private static void CheckEventsPublications(XmlNode pesquisador)
        {
            foreach (XmlNode trabalhoCongresso in pesquisador["trabalho_completo_congresso"].SelectNodes("trabalho_completo"))
            {
                string conf_iri = OwlHelper.ToIRI(trabalhoCongresso["nome_evento"].InnerText);
                string artigoIri = OwlHelper.ToIRI(trabalhoCongresso["titulo"].InnerText);

                IOntologyIndividual eventoIndividual = null;

                if (NamedIndividuals.Instance.Conferencias.ContainsKey(conf_iri))
                {
                    eventoIndividual = NamedIndividuals.Instance.Conferencias[conf_iri];
                }
                else if (NamedIndividuals.Instance.Simposios.ContainsKey(conf_iri))
                {
                    eventoIndividual = NamedIndividuals.Instance.Simposios[conf_iri];
                }
                else
                {
                    continue;
                }

                var artigoIndividual = NamedIndividuals.Instance.Artigos[artigoIri];
                IOntologyRelation publicadoEmRelation = new OntologyRelation(artigoIndividual, OntologyRelationType.PublicadoEm, eventoIndividual);
                //IOntologyRelation localizadoRelation = TODO ??? 

                AddRelation(publicadoEmRelation);
                //AddRelation(localizadoRelation); TODO ??? 
            }
        }

        private static void CheckJournalsPublications(XmlNode pesquisador, IOntologyIndividual pesquisadorIndividual)
        {
            foreach (XmlNode artigo in pesquisador["artigos_em_periodicos"].SelectNodes("artigo"))
            {
                string artigo_iri = OwlHelper.ToIRI(artigo["titulo"].InnerText);
                IOntologyIndividual artigoIndividual = NamedIndividuals.Instance.Artigos[artigo_iri];

                IOntologyRelation autorRelation = new OntologyRelation(pesquisadorIndividual, OntologyRelationType.Autor, artigoIndividual);
                AddRelation(autorRelation);

                string revista_iri = OwlHelper.ToIRI(artigo["revista"].InnerText);
                var revistaIndividual = NamedIndividuals.Instance.Revistas[revista_iri];
                IOntologyRelation publicadoEmRelation = new OntologyRelation(artigoIndividual, OntologyRelationType.PublicadoEm, revistaIndividual);
                AddRelation(publicadoEmRelation);
            }
        }  
        
        private void CheckUniversidadePais(XmlElement xmlElement)
        {
            Universidade universidade = GetUniversidadePais(xmlElement);
            Pais pais = universidade.pais;

            if (!NamedIndividuals.Instance.Universidades.ContainsKey(universidade.IRI))
            {
                NamedIndividuals.Instance.Universidades.Add(universidade.IRI, universidade);
            }

            if (!NamedIndividuals.Instance.Paises.ContainsKey(pais.IRI))
            {
                NamedIndividuals.Instance.Paises.Add(pais.IRI, pais);
            }
        }

        private Universidade GetUniversidadePais(XmlElement xmlElement)
        {
            Universidade universidade;
            Pais pais;

            string value = xmlElement.InnerText;
            string[] values = value.Split(new char[] { ',' });
            string org = OwlHelper.ToIRI(values[0].Trim());

            string paisIRI = OwlHelper.ToIRI(values[2].Trim());
            pais = new Pais(paisIRI)
            {
                nome_completo = values[2]
            };

            if (org.Contains("universidade_de_sao_paulo"))
            {
                universidade = new Universidade("universidade_de_sao_paulo", pais)
                {
                    nome_completo = "Universidade de São Paulo"
                };
            }
            else
            {
                universidade = new Universidade(org, pais)
                 {
                     nome_completo = values[0]
                 };
            }

            return universidade;
        }

        private void CheckCurso(XmlElement xmlElement)
        {
            var curso = GetCurso(xmlElement);
            if (!NamedIndividuals.Instance.Cursos.ContainsKey(curso.IRI))
            {
                NamedIndividuals.Instance.Cursos.Add(curso.IRI, curso);
            }
        }

        private Curso GetCurso(XmlElement xmlElement)
        {
            string tipoIRI = OwlHelper.ToIRI(xmlElement.InnerText);
            return new Curso(tipoIRI)
            {
                tipo_curso = xmlElement.InnerText.Contains("Graduação") ? TipoCurso.graduacao : TipoCurso.pos_graduacao,
                titulo = xmlElement.InnerText
            };
        }

        private static void AddRelation(IOntologyRelation autorRelation)
        {
            NamedIndividuals.Instance.Relations.Add(autorRelation);
        }

    }
}

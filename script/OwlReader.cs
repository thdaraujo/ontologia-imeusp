using OwlImport.Core;
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
                    Professor auxProf = new Professor();
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
                        NamedIndividuals.Instance.Artigos.Add(artigo_iri, new Artigo()
                        {
                            Ano = Convert.ToInt32(artigo["ano"].InnerText),
                            titulo = artigo["titulo"].InnerText
                        });
                    }

                    string revista_iri = OwlHelper.ToIRI(artigo["revista"].InnerText);
                    if (!NamedIndividuals.Instance.Revistas.ContainsKey(revista_iri))
                    {
                        NamedIndividuals.Instance.Revistas.Add(revista_iri, new Revista()
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
                                NamedIndividuals.Instance.Conferencias.Add(conf_iri, new Conferencia()
                                {
                                     titulo = trabalhoCongresso["nome_evento"].InnerText,
                                     ano = Convert.ToInt32(trabalhoCongresso["ano"].InnerText)
                                });
                            }
                            break;
                        case TipoCongresso.Simposio:
                            if (!NamedIndividuals.Instance.Simposios.ContainsKey(conf_iri))
                            {
                                NamedIndividuals.Instance.Simposios.Add(conf_iri, new Simposio()
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
                        NamedIndividuals.Instance.Artigos.Add(artigoIri_conf, new Artigo()
                        {
                            titulo = trabalhoCongresso["titulo"].InnerText,
                            Ano =  Convert.ToInt32(trabalhoCongresso["ano"].InnerText)
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
                foreach (XmlNode artigo in pesquisador["artigos_em_periodicos"])
                {
                    // TODO: Relações...
                }
            }
        }

        private void CheckUniversidadePais(XmlElement xmlElement)
        {
            string value = xmlElement.InnerText;
            string[] values = value.Split(new char[] { ',' });
            string org = OwlHelper.ToIRI(values[0].Trim());

            if (org.Contains("universidade_de_sao_paulo"))
            {
                if (!NamedIndividuals.Instance.Universidades.ContainsKey("universidade_de_sao_paulo"))
                {
                    NamedIndividuals.Instance.Universidades.Add("universidade_de_sao_paulo", new Universidade()
                    {
                        nome_completo = "Universidade de São Paulo"
                    });
                }
            }
            else if (!NamedIndividuals.Instance.Universidades.ContainsKey(org))
            {
                NamedIndividuals.Instance.Universidades.Add(org, new Universidade()
                {
                    nome_completo = values[0]
                });
            }

            string pais = OwlHelper.ToIRI(values[2].Trim());
            if (!NamedIndividuals.Instance.Paises.ContainsKey(pais))
            {
                NamedIndividuals.Instance.Paises.Add(pais, new Pais()
                {
                    nome_completo = values[2]
                });
            }
        }

        private void CheckCurso(XmlElement xmlElement)
        {
            string tipoIRI = OwlHelper.ToIRI(xmlElement.InnerText);
            if (!NamedIndividuals.Instance.Cursos.ContainsKey(tipoIRI))
            {
                NamedIndividuals.Instance.Cursos.Add(tipoIRI, new Curso()
                {
                    tipo_curso = xmlElement.InnerText.Contains("Graduação") ? TipoCurso.graduacao : TipoCurso.pos_graduacao,
                    titulo = xmlElement.InnerText
                });
            }
        }

    }
}

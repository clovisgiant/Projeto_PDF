using System;
using System.Data;
using Npgsql;
using System.Linq;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

using System.Text.RegularExpressions;
using System.Security;


namespace PostgreSQL_CRUD
{
    public class DAL
    {
        static string serverName = "localhost";  //localhost
        static string port = "5432";             //porta default
        static string userName = "postgres";     //nome do administrador
        static string password = "d19m11";     //senha do administrador
        static string databaseName = "giant_novo_producao"; //nome do banco de dados
        NpgsqlConnection pgsqlConnection = null ;
        string connString = null;

        public DAL()
        {
             connString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                           serverName, port, userName, password, databaseName);
        }


        public void  GetTodosRegistrosNNR( string linha, bool master)
        {
            Master rs  = new Master();
            House rshouse = new House();
            string[] lines = Regex.Split(linha, "\n");           

                try
                {
                    int contador = 0;
                    int contadorhouses = 0;
                    foreach (string s in lines)
                    {
                       // for (int i = 0; i < lines.Length; i++)
                       // {
                            if (master)
                            {
                                Console.Write(" estou no master");
                                if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                                {
                                    string numeromaster;
                                    numeromaster = lines[contador + 1];
                                    var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                                    .Cast<Match>()
                                    .Select(m => m.Value)
                                    .ToList();
                                    //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                                    rs.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);
                                    // string transportador;
                                    //transportador = rs.nrmaster;
                                    rs.transportador = parts[1];
                                    goto BREAK;
                                }
                                else if (s.ToString() == "Airport of Departure Addr.of First Carrier and Requested Routing")
                                {
                                    string origem;
                                    origem = lines[contador + 1];
                                    rs.origem = origem.Substring(0, origem.Length);
                                    goto BREAK;
                                }
                                else if (s.ToString().Trim() == "if Insurance.")
                                {
                                    string Destino;
                                    string nrvoo;
                                    Destino = lines[contador + 1];
                                    //string[] partes = Regex.Split(Destino, " ");
                                    nrvoo = Destino.Substring(Destino.Length - 11);
                                    rs.Destino = Destino.Substring(0, Destino.Length - 9);
                                    rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                                    goto BREAK;
                                }
                                else if (s.ToString() == "Agents IATA Code Account No.")
                                {
                                    string agente;
                                    agente = lines[contador + 1];                                   
                                    rs.agente = agente.Substring(0, agente.Length);
                                    goto BREAK;
                                }
                                else if (s.ToString().Trim() == "Code")
                                {
                                    string moeda;
                                    moeda = lines[contador + 1];
                                    string[] partesmoedas = Regex.Split(moeda, " ");                                  
                                    rs.moedafrete = partesmoedas[4];
                                    string tipofrete;
                                    tipofrete = lines[contador + 1];                                   
                                    rs.tipofrete = partesmoedas[4];
                                    goto BREAK;
                                }                               
                                else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                                {

                                    string embarque;
                                    embarque = lines[contador - 1];
                                    embarque = embarque.Substring(embarque.Length - 16);
                                    string result = Regex.Replace(embarque, "[^0-9]", "");
                                    string mes = result.Substring(0, 2);
                                    string dia = result.Substring(2,2);
                                    string Ano = result.Substring(4, 4);                              
                                   
                                    rs.embarque = dia + "/" + mes + "/" + Ano;
                                    rs.emissaoconhecimento = dia + "/" + mes + "/" + Ano;
                                    rs.prevembarque = dia + "/" + mes + "/" + Ano;
                                    goto BREAK;
                                }

                                else
                                {
                                    goto BREAK;
                                }
                            } 
                            else //house
                            {
                                Console.Write(" estou no house");
                                if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                                {
                                    string numeromaster;
                                    numeromaster = lines[contador + 1];
                                    var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                                    .Cast<Match>()
                                    .Select(m => m.Value)
                                    .ToList();
                                    //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                                    rshouse.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);                                   
                                    rshouse.transportador = parts[1];
                                    string cliente;
                                    cliente = lines[contador + 2];
                                    rshouse.cliente = cliente.Substring(0, cliente.Length);                                    
                                    goto BREAK;
                                }
                                else if (s.ToString() == "Not Negotiable")
                                {
                                    string numerohouse;
                                    numerohouse = lines[contador + 1];
                                    rshouse.nrhouse = numerohouse.Substring(0, numerohouse.Length - 11);
                                    rshouse.quantidadeprocesso = contadorhouses.ToString();
                                    goto BREAK;
                                }
                                else if (s.ToString() == "Airport of Departure Addr.of First Carrier and Requested Routing")
                                {
                                    string origem;
                                    origem = lines[contador + 1];
                                    rshouse.origem = origem.Substring(0, origem.Length);
                                    goto BREAK;
                                }
                                else if (s.ToString().Trim() == "if Insurance.")
                                {
                                    string Destino;
                                    string nrvoo;
                                    Destino = lines[contador + 1];
                                    nrvoo = Destino.Substring(Destino.Length - 11);
                                    rshouse.Destino = Destino.Substring(0, Destino.Length - 9);
                                    rshouse.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                                    goto BREAK;
                                }
                                else if (s.ToString() == "Agents IATA Code Account No.")
                                {
                                    string agente;
                                    agente = lines[contador + 1];
                                    rshouse.agente = agente.Substring(0, agente.Length);
                                    goto BREAK;
                                }

                                else if (s.ToString() == "Issued By")
                                {
                                    string Exportador;
                                    Exportador = lines[contador + 1];
                                    rshouse.Exportador = Exportador.Substring(0, Exportador.Length - 28);
                                    goto BREAK;
                                }
                                else if (s.ToString() == "RPC")
                                {
                                    string qtdvolume;
                                    qtdvolume = lines[contador + 1];
                                    string[] partes = Regex.Split(qtdvolume, " ");
                                    rshouse.quantidade = partes[1]; //qtdvolume.Substring(0, 2);
                                    string pesobruto;
                                    pesobruto = lines[contador + 1];
                                    rshouse.pesobruto = partes[2]; //pesobruto.Substring(2, 3);
                                    string pesovolumetrico;
                                    pesovolumetrico = lines[contador + 1];
                                    rshouse.pesotaxado = partes[6]; //pesovolumetrico.Substring(9, 5);
                                    goto BREAK;
                                }
                                else if (s.ToString().Trim() == "Code")
                                {
                                    string moeda;
                                    moeda = lines[contador + 1];
                                    string[] partes = Regex.Split(moeda, " ");                                  
                                    rshouse.moedafrete = partes[4];
                                    string tipofrete;
                                    tipofrete = lines[contador + 1];
                                    rshouse.tipofrete = partes[4];
                                    goto BREAK;
                                }
                               
                               
                            }
                       // } // fim do for 
                    BREAK: ;
                        contador++;
                        contadorhouses++;
                    }
                    if (master)
                    {
                        InserirRegistrosMaster(rs);
                    }
                    else
                    {                        
                        InserirRegistroshouse(rshouse);
                    }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
                //return rs.ToString();
             
        }

        public void GetTodosRegistrosAramex(string linha, bool master)  //=============//Função salva os registros do layout Aramex==============
        {
            Master rs = new Master();
            House rshouse = new House();
            string[] lines = Regex.Split(linha, "\n");

            try
            {
                int contador = 0;
                int contadorhouses = 0;
                foreach (string s in lines)
                {
                    // for (int i = 0; i < lines.Length; i++)
                    // {
                    if (master)
                    {
                        Console.Write(" estou no master");
                        if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador + 1];
                            var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();
                            //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                            rs.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);                           
                            rs.transportador = parts[1];
                            goto BREAK;
                        }
                        else if (s.ToString() == "Airport of Departure Addr.of First Carrier and Requested Routing")
                        {
                            string origem;
                            origem = lines[contador + 1];
                            rs.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "if Insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            Destino = lines[contador + 1];
                            nrvoo = Destino.Substring(Destino.Length - 11);
                            rs.Destino = Destino.Substring(0, Destino.Length - 9);
                            rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rs.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            moeda = moeda.Substring(moeda.Length - 7);
                            rs.moedafrete = moeda.Substring(0, moeda.Length - 3);
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rs.tipofrete = tipofrete.Substring(tipofrete.Length - 3);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                        {

                            string embarque;
                            embarque = lines[contador - 1];
                            embarque = embarque.Substring(embarque.Length - 16);
                            string result = Regex.Replace(embarque, "[^0-9]", "");
                            string mes = result.Substring(0, 2);
                            string dia = result.Substring(2, 2);
                            string Ano = result.Substring(4, 4);
                            rs.embarque = dia + "/" + mes + "/" + Ano;
                            rs.emissaoconhecimento = dia + "/" + mes + "/" + Ano;
                            rs.prevembarque = dia + "/" + mes + "/" + Ano;
                            goto BREAK;
                        }

                        else
                        {
                            goto BREAK;
                        }
                    }
                    else //house
                    {
                        Console.Write(" estou no house");
                        if (s.ToString() == "MASTER AWB No. DUCR HAWB No.")
                        {
                            string numeromaster;
                            numeromaster = lines[contador + 2];                           
                            rshouse.nrmaster = numeromaster;//parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);                          
                            rshouse.transportador = numeromaster.Substring(0,3);                   
                          
                            string numerohouse;
                            numerohouse = lines[contador + 1];
                            rshouse.nrhouse = numerohouse.Substring(0, numerohouse.Length);
                            rshouse.quantidadeprocesso = contadorhouses.ToString();
                            goto BREAK;
                        }

                        if (s.ToString() == "Consignees Name and  Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                        {
                            string cliente;
                            cliente = lines[contador + 1];
                            rshouse.cliente = cliente.Substring(0, cliente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Airport of Departure and requested Routing")
                        {
                            string origem;
                            origem = lines[contador + 1];
                            rshouse.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "of insurance")
                        {
                            string Destino;
                            string nrvoo;
                            Destino = lines[contador - 1];
                            nrvoo = lines[contador -2]; 
                            rshouse.Destino = Destino.Substring(0, Destino.Length);
                            rshouse.nrvoo = nrvoo.Substring(0, nrvoo.Length);
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 2];
                            rshouse.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "HOUSE AIR WAYBILL Air Consignment Note")
                        {
                            string Exportador;
                            Exportador = lines[contador + 1];
                            rshouse.Exportador = Exportador.Substring(0, Exportador.Length);
                        }
                        else if (s.ToString() == "RCP")
                        {
                            string qtdvolume;
                            qtdvolume = lines[contador + 2];
                            string[] partes = Regex.Split(qtdvolume, " ");
                            rshouse.quantidade = partes[0]; //qtdvolume.Substring(0, 2);
                            string pesobruto;
                            pesobruto = lines[contador + 2];
                            rshouse.pesobruto = partes[1]; //pesobruto.Substring(2, 3);
                            string pesovolumetrico;
                            pesovolumetrico = lines[contador + 2];
                            rshouse.pesotaxado = partes[4]; //pesovolumetrico.Substring(9, 5);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "PPD COLL PPD COLL")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            string[] partes = Regex.Split(moeda, " ");
                            rshouse.moedafrete = partes[0];

                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rshouse.tipofrete = partes[0];


                            goto BREAK;
                        }

                    }
                // } // fim do for 
                BREAK: ;
                    contador++;
                    contadorhouses++;
                }
                if (master)
                {
                    InserirRegistrosMaster(rs);
                }
                else
                {
                    InserirRegistroshouse(rshouse);
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            //return rs.ToString();

        }

        public void GetTodosRegistroscargowiser(string linha, bool master)  //=============//Função salva os registros do layout cargowiser==============
        {
            Master rs = new Master();
            House rshouse = new House();
            string[] lines = Regex.Split(linha, "\n");

            try
            {
                int contador = 0;
                int contadorhouses = 0;
                foreach (string s in lines)
                {
                    // for (int i = 0; i < lines.Length; i++)
                    // {
                    if (master)
                    {
                        Console.Write(" estou no master");
                        if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador + 1];
                            var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();
                            //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                            rs.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);
                            // string transportador;
                            //transportador = rs.nrmaster;
                            rs.transportador = parts[1];
                            goto BREAK;
                        }
                        else if (s.ToString() == "Copies 1, 2 and 3 of this Air Waybill are originals and have the same validity")
                        {
                            string origem;
                            origem = lines[contador - 2];
                            rs.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "if Insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            Destino = lines[contador + 1];
                            nrvoo = Destino.Substring(Destino.Length - 11);
                            rs.Destino = Destino.Substring(0, Destino.Length - 9);
                            rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rs.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            moeda = moeda.Substring(moeda.Length - 7);
                            rs.moedafrete = moeda.Substring(0, moeda.Length - 3);
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rs.tipofrete = tipofrete.Substring(tipofrete.Length - 3);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                        {

                            string embarque;
                            embarque = lines[contador - 1];
                            embarque = embarque.Substring(embarque.Length - 16);
                            string result = Regex.Replace(embarque, "[^0-9]", "");
                            string mes = result.Substring(0, 2);
                            string dia = result.Substring(2, 2);
                            string Ano = result.Substring(4, 4);

                            rs.embarque = dia + "/" + mes + "/" + Ano;
                            rs.emissaoconhecimento = dia + "/" + mes + "/" + Ano;
                            rs.prevembarque = dia + "/" + mes + "/" + Ano;
                            goto BREAK;
                        }

                        else
                        {
                            goto BREAK;
                        }
                    }
                    else //house
                    {
                        Console.Write(" estou no house");
                        if (s.ToString() == "Shippers Name and Address Shippers Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador - 2];
                            rshouse.nrmaster = numeromaster.Substring(0, 16);                          
                            rshouse.transportador = numeromaster.Substring(0, 3);

                            string numerohouse;
                            numerohouse = lines[contador - 2];
                            rshouse.nrhouse = numerohouse.Substring(numerohouse.Length -11);
                            rshouse.quantidadeprocesso = contadorhouses.ToString();
                            goto BREAK;
                        }

                        if (s.ToString() == "GOODS MAY BE CARRIED BY ANY OTHER MEANS INCLUDING ROAD OR ANY OTHER CARRIER ")
                        {
                            string cliente;
                            cliente = lines[contador + 3];
                            rshouse.cliente = cliente.Substring(0, cliente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Airport of Departure Addr. of First Carrier and Requested Routing Reference Number Optional Shipping Information")
                        {
                            string origem;
                            origem = lines[contador + 1];
                            rshouse.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "insured in figures in box marked amount of insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            string nrvoo2;
                            Destino = lines[contador + 1];
                            nrvoo = lines[contador + 1];
                            rshouse.Destino = Destino.Substring(0, Destino.Length - 24);                            
                            nrvoo2 = nrvoo.Substring(nrvoo.Length - 21);
                            rshouse.nrvoo = nrvoo2.Substring(0,nrvoo2.Length - 3);
                          
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rshouse.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Issued by")
                        {
                            string Exportador;
                            Exportador = lines[contador - 1];
                            rshouse.Exportador = Exportador.Substring(0, Exportador.Length);
                        }
                        else if (s.ToString() == "RCP Item No.")
                        {
                            string qtdvolume;                           
                            qtdvolume = lines[contador + 2];           
                            
                            string[] partes = Regex.Split(qtdvolume, " ");
                            rshouse.quantidade = partes[0]; //qtdvolume.Substring(0, 2);
                            string pesobruto;
                            pesobruto = lines[contador + 2];
                            rshouse.pesobruto = partes[1]; //pesobruto.Substring(2, 3);
                            string pesovolumetrico;
                            pesovolumetrico = lines[contador + 2];
                            rshouse.pesotaxado = partes[4]; //pesovolumetrico.Substring(9, 5);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "PPD COLL PPD COLL")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            string[] moedapart = Regex.Split(moeda, " ");
                            moeda = moedapart[4];
                            rshouse.moedafrete = moedapart[4];
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rshouse.tipofrete = moedapart[5];
                            goto BREAK;
                        }

                    }
                // } // fim do for 
                BREAK: ;
                    contador++;
                    contadorhouses++;
                }
                if (master)
                {
                    InserirRegistrosMaster(rs);
                }
                else
                {
                    InserirRegistroshouse(rshouse);
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            //return rs.ToString();

        }

        public void GetTodosRegistrosFREIGHTLOG(string linha, bool master)  //=============//Função salva os registros do layout cargowiser==============
        {
            Master rs = new Master();
            House rshouse = new House();
            string[] lines = Regex.Split(linha, "\n");

            try
            {
                int contador = 0;
                int contadorhouses = 0;
                foreach (string s in lines)
                {
                    // for (int i = 0; i < lines.Length; i++)
                    // {
                    if (master)
                    {
                        Console.Write(" estou no master");
                        if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador + 1];
                            var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();
                            //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                            rs.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);
                            // string transportador;
                            //transportador = rs.nrmaster;
                            rs.transportador = parts[1];
                            goto BREAK;
                        }
                        else if (s.ToString() == "Copies 1, 2 and 3 of this Air Waybill are originals and have the same validity")
                        {
                            string origem;
                            origem = lines[contador - 2];
                            rs.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "if Insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            Destino = lines[contador + 1];
                            nrvoo = Destino.Substring(Destino.Length - 11);
                            rs.Destino = Destino.Substring(0, Destino.Length - 9);
                            rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rs.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            moeda = moeda.Substring(moeda.Length - 7);
                            rs.moedafrete = moeda.Substring(0, moeda.Length - 3);
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rs.tipofrete = tipofrete.Substring(tipofrete.Length - 3);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                        {

                            string embarque;
                            embarque = lines[contador - 1];
                            embarque = embarque.Substring(embarque.Length - 16);
                            string result = Regex.Replace(embarque, "[^0-9]", "");
                            string mes = result.Substring(0, 2);
                            string dia = result.Substring(2, 2);
                            string Ano = result.Substring(4, 4);

                            rs.embarque = dia + "/" + mes + "/" + Ano;
                            rs.emissaoconhecimento = dia + "/" + mes + "/" + Ano;
                            rs.prevembarque = dia + "/" + mes + "/" + Ano;
                            goto BREAK;
                        }

                        else
                        {
                            goto BREAK;
                        }
                    }
                    else //house
                    {
                        Console.Write(" estou no house");
                        if (s.ToString() == "Shippers Name and Address Shippers Account Number Not negotiable")
                        {
                            string numeromaster;
                            numeromaster = lines[contador - 1];
                            numeromaster = numeromaster.Replace(" ", "");
                            rshouse.nrmaster = numeromaster.Substring(0, 14);
                            rshouse.transportador = numeromaster.Substring(0, 3);

                            string numerohouse;
                            numerohouse = lines[contador - 1];
                            numerohouse = numerohouse.Replace(" ", "");
                            rshouse.nrhouse = numerohouse.Substring(numerohouse.Length - 10);
                            rshouse.quantidadeprocesso = contadorhouses.ToString();
                            goto BREAK;
                        }

                        if (s.ToString() == "LIABILITY.    Shipper    may   increase  such  limitation of liability by declaring a higher value for carriage and paying a supplemental charge if required.")
                        {
                            string cliente;
                            cliente = lines[contador + 1];
                            rshouse.cliente = cliente.Substring(0, cliente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Airport of Departure Addr of First Carrier and Requested Routing VAT Code Imprenditore Non Imprenditore")
                        {
                            string origem;
                            origem = lines[contador + 2];
                            rshouse.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Handling Information")
                        {
                            string Destino;
                            string nrvoo;
                            string nrvoo2;
                            Destino = lines[contador - 1];
                            string[] partes = Regex.Split(Destino.Replace(" ",""), " ");
                            nrvoo = lines[contador - 1];
                            rshouse.Destino = partes[0].Substring(0, 9);///Destino.Substring(0, Destino.Length - 10);
                            nrvoo2 = nrvoo.Substring(partes[0].Length - 10);
                            rshouse.nrvoo = nrvoo2.Substring(0, nrvoo2.Length -6);

                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rshouse.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Issued by")
                        {
                            string Exportador;
                            Exportador = lines[contador + 1];
                            rshouse.Exportador = Exportador.Substring(0, Exportador.Length - 26);
                        }
                        else if (s.ToString() == "RCP")
                        {
                            string qtdvolume;
                            qtdvolume = lines[contador + 1];
                            string[] partes = Regex.Split(qtdvolume.Replace(" ",""), " ");
                            rshouse.quantidade = partes[0].Substring(0, 1);
                            string pesobruto;
                            pesobruto = lines[contador + 1];
                            rshouse.pesobruto = partes[0].Substring(1, 5);
                            string pesovolumetrico;
                            pesovolumetrico = lines[contador + 2];
                            rshouse.pesotaxado = partes[0].Substring(8, 5);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code PPD COLL PPD COLL")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            string[] moedapart = Regex.Split(moeda, " ");
                            moeda = moedapart[22];
                            rshouse.moedafrete = moedapart[22];
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rshouse.tipofrete = moedapart[22];
                            goto BREAK;
                        }

                    }
                // } // fim do for 
                BREAK: ;
                    contador++;
                    contadorhouses++;
                }
                if (master)
                {
                    InserirRegistrosMaster(rs);
                }
                else
                {
                    InserirRegistroshouse(rshouse);
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            //return rs.ToString();

        }

        public void GetTodosRegistrosModulAIR(string linha, bool master)  //=============//Função salva os registros do layoutcargopartner==============
        {
            Master rs = new Master();
            House rshouse = new House();
            string[] lines = Regex.Split(linha, "\n");

            try
            {
                int contador = 0;
                int contadorhouses = 0;
                foreach (string s in lines)
                {
                    // for (int i = 0; i < lines.Length; i++)
                    // {
                    if (master)
                    {
                        Console.Write(" estou no master");
                        if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador + 1];
                            var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();
                            //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                            rs.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);
                            // string transportador;
                            //transportador = rs.nrmaster;
                            rs.transportador = parts[1];
                            goto BREAK;
                        }
                        else if (s.ToString() == "Copies 1, 2 and 3 of this Air Waybill are originals and have the same validity")
                        {
                            string origem;
                            origem = lines[contador - 2];
                            rs.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "if Insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            Destino = lines[contador + 1];
                            nrvoo = Destino.Substring(Destino.Length - 11);
                            rs.Destino = Destino.Substring(0, Destino.Length - 9);
                            rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rs.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            moeda = moeda.Substring(moeda.Length - 7);
                            rs.moedafrete = moeda.Substring(0, moeda.Length - 3);
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rs.tipofrete = tipofrete.Substring(tipofrete.Length - 3);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                        {

                            string embarque;
                            embarque = lines[contador - 1];
                            embarque = embarque.Substring(embarque.Length - 16);
                            string result = Regex.Replace(embarque, "[^0-9]", "");
                            string mes = result.Substring(0, 2);
                            string dia = result.Substring(2, 2);
                            string Ano = result.Substring(4, 4);

                            rs.embarque = dia + "/" + mes + "/" + Ano;
                            rs.emissaoconhecimento = dia + "/" + mes + "/" + Ano;
                            rs.prevembarque = dia + "/" + mes + "/" + Ano;
                            goto BREAK;
                        }

                        else
                        {
                            goto BREAK;
                        }
                    }
                    else //house
                    {
                        Console.Write(" estou no house");
                        if (s.ToString() == "Shippers Name and Address Shippers Account Number Not Negotiable")
                        {
                            string numeromaster;
                            numeromaster = lines[contador - 1];
                            numeromaster = numeromaster.Replace(" ", "");
                            rshouse.nrmaster = numeromaster.Substring(0, 14);
                            rshouse.transportador = numeromaster.Substring(0, 3);

                            string numerohouse;
                            numerohouse = lines[contador - 1];
                            numerohouse = numerohouse.Replace(" ", "");
                            rshouse.nrhouse = numerohouse.Substring(numerohouse.Length - 12);
                            rshouse.quantidadeprocesso = contadorhouses.ToString();

                            string Exportador;
                            Exportador = lines[contador + 1];
                            rshouse.Exportador = Exportador.Substring(0, Exportador.Length - 26);
                            goto BREAK;
                        }

                        if (s.ToString() == "REVERSE HEREOF. ALL GOODS MAY BE CARRIED BY ANY OTHER MEANS INCLUDING ROAD OR ANY OTHER ")
                        {
                            string cliente;
                            cliente = lines[contador + 1];
                            rshouse.cliente = cliente.Substring(0, cliente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Airport of Departure Addr. of First Carrier and Requested Routing Reference Number Optional Shipping Information")
                        {
                            string origem;
                            origem = lines[contador + 1];
                            string[] partes = Regex.Split(origem, " ");
                            rshouse.origem = partes[0]; //origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "to be insured in figures in box marked Amount of insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            string nrvoo2;
                            Destino = lines[contador + 1];
                            string[] partes = Regex.Split(Destino, " ");
                            nrvoo = lines[contador + 1];
                            rshouse.Destino = partes[0];///Destino.Substring(0, Destino.Length - 10);
                           // nrvoo2 = partes[1] + partes[2] + partes[3] + partes[4]; //nrvoo.Substring(partes[0].Length - 10);
                            rshouse.nrvoo = partes[1] + partes[2] + partes[3] + partes[4];
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rshouse.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }


                        else if (s.ToString() == "Item No.")
                        {
                            string qtdvolume;
                            qtdvolume = lines[contador + 1];
                            string[] partes = Regex.Split(qtdvolume, " ");
                            rshouse.quantidade = partes[0];
                            string pesobruto;
                            pesobruto = lines[contador + 1];
                            rshouse.pesobruto = partes[1];
                            string pesovolumetrico;
                            pesovolumetrico = lines[contador + 1];
                            rshouse.pesotaxado = partes[4];
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            string[] moedapart = Regex.Split(moeda, " ");
                            moeda = moedapart[6];
                            rshouse.moedafrete = moedapart[6];
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rshouse.tipofrete = moedapart[7];
                            goto BREAK;
                        }

                    }
                // } // fim do for 
                BREAK: ;
                    contador++;
                    contadorhouses++;
                }
                if (master)
                {
                    InserirRegistrosMaster(rs);
                }
                else
                {
                    InserirRegistroshouse(rshouse);
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            //return rs.ToString();

        }

        public void GetTodosRegistrosCARGOPARTNER(string linha, bool master)  //=============//Função salva os registros do layoutcargopartner==============
        {
            Master rs = new Master();
            House rshouse = new House();
            string[] lines = Regex.Split(linha, "\n");

            try
            {
                int contador = 0;
                int contadorhouses = 0;
                foreach (string s in lines)
                {
                    // for (int i = 0; i < lines.Length; i++)
                    // {
                    if (master)
                    {
                        Console.Write(" estou no master");
                        if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador + 1];
                            var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();
                            //string result = Regex.Replace(numeromaster, "[^0-9]", "");
                            rs.nrmaster = parts[1] + parts[2] + parts[3].ToString(); //numeromaster.Substring(0, numeromaster.Length - 6);
                            // string transportador;
                            //transportador = rs.nrmaster;
                            rs.transportador = parts[1];
                            goto BREAK;
                        }
                        else if (s.ToString() == "Copies 1, 2 and 3 of this Air Waybill are originals and have the same validity")
                        {
                            string origem;
                            origem = lines[contador - 2];
                            rs.origem = origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "if Insurance.")
                        {
                            string Destino;
                            string nrvoo;
                            Destino = lines[contador + 1];
                            nrvoo = Destino.Substring(Destino.Length - 11);
                            rs.Destino = Destino.Substring(0, Destino.Length - 9);
                            rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                            goto BREAK;
                        }
                        else if (s.ToString() == "Agents IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rs.agente = agente.Substring(0, agente.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Code")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            moeda = moeda.Substring(moeda.Length - 7);
                            rs.moedafrete = moeda.Substring(0, moeda.Length - 3);
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rs.tipofrete = tipofrete.Substring(tipofrete.Length - 3);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                        {

                            string embarque;
                            embarque = lines[contador - 1];
                            embarque = embarque.Substring(embarque.Length - 16);
                            string result = Regex.Replace(embarque, "[^0-9]", "");
                            string mes = result.Substring(0, 2);
                            string dia = result.Substring(2, 2);
                            string Ano = result.Substring(4, 4);

                            rs.embarque = dia + "/" + mes + "/" + Ano;
                            rs.emissaoconhecimento = dia + "/" + mes + "/" + Ano;
                            rs.prevembarque = dia + "/" + mes + "/" + Ano;
                            goto BREAK;
                        }

                        else
                        {
                            goto BREAK;
                        }
                    }
                    else //house
                    {
                        Console.Write(" estou no house");
                        if (s.ToString() == "Shippers Name and Address Shippers Account Number")
                        {
                            string numeromaster;
                            numeromaster = lines[contador - 1];
                            numeromaster = numeromaster.Replace(" ", "");
                            rshouse.nrmaster = numeromaster.Substring(0, 14);
                            rshouse.transportador = numeromaster.Substring(0, 3);

                            string numerohouse;
                            numerohouse = lines[contador - 1];
                            numerohouse = numerohouse.Replace(" ", "");
                            rshouse.nrhouse = numerohouse.Substring(numerohouse.Length - 12);
                            rshouse.quantidadeprocesso = contadorhouses.ToString();

                            string Exportador;
                            Exportador = lines[contador + 2];
                            rshouse.Exportador = Exportador.Substring(0, Exportador.Length - 11);
                            goto BREAK;
                        }

                        if (s.ToString() == " REVERSE HEREOF. ALL GOODS MAY BE CARRIED BY ANY OTHER MEANS INCLUDING")
                        {
                            string cliente;
                            cliente = lines[contador + 1];
                            rshouse.cliente = cliente.Substring(0, cliente.Length);
                            goto BREAK;
                        }

                        else if (s.ToString() == "Airport of Departure Adr. of First Carrier and Requested Routing Reference Number Optional Shipping Information")
                        {
                            string origem;
                            origem = lines[contador + 1];
                            string[] partes = Regex.Split(origem, " ");
                            rshouse.origem = partes[0]; //origem.Substring(0, origem.Length);
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "requested in accordance with the conditions thereof, indicate amount")
                        {
                            string Destino;
                            string nrvoo;
                            string nrvoo2;
                            Destino = lines[contador + 1];
                            string[] partes = Regex.Split(Destino, " ");
                            nrvoo = lines[contador + 1];
                            rshouse.Destino = partes[0];///Destino.Substring(0, Destino.Length - 10);
                            // nrvoo2 = partes[1] + partes[2] + partes[3] + partes[4]; //nrvoo.Substring(partes[0].Length - 10);
                            rshouse.nrvoo = partes[8];

                            goto BREAK;
                        }
                        else if (s.ToString() == "Agent´s IATA Code Account No.")
                        {
                            string agente;
                            agente = lines[contador + 1];
                            rshouse.agente = agente.Substring(0, agente.Length-7);
                            goto BREAK;
                        }


                        else if (s.ToString() == "Commodity Item no.")
                        {
                            string qtdvolume;
                            qtdvolume = lines[contador + 2];
                            string[] partes = Regex.Split(qtdvolume, " ");
                            rshouse.quantidade = partes[0];
                            string pesobruto;
                            pesobruto = lines[contador + 2];
                            rshouse.pesobruto = partes[4];
                            string pesovolumetrico;
                            pesovolumetrico = lines[contador + 2];
                            rshouse.pesotaxado = partes[16];
                            goto BREAK;
                        }
                        else if (s.ToString().Trim() == "PPD  COLL PPD  COLL")
                        {
                            string moeda;
                            moeda = lines[contador + 1];
                            string[] moedapart = Regex.Split(moeda, " ");
                            moeda = moedapart[26];
                            rshouse.moedafrete = moedapart[26];
                            string tipofrete;
                            tipofrete = lines[contador + 1];
                            rshouse.tipofrete = moedapart[28];
                            goto BREAK;
                        }

                    }
                // } // fim do for 
                BREAK: ;
                    contador++;
                    contadorhouses++;
                }
                if (master)
                {
                    InserirRegistrosMaster(rs);
                }
                else
                {
                    InserirRegistroshouse(rshouse);
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            //return rs.ToString();

        }



        //Pega todos os registros
        public DataTable GetTodosRegistros()
        { 

            DataTable dt = new DataTable();

            try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from tb_arquivospdfs";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }

            return dt;
        }


        //Pega todos os registros
        public DataTable GetTodosRegistrosPDF()
        {

            DataTable dt = new DataTable();

            try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select descricao from tb_arquivospdfs";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }

            return dt;
        }
        

        //Pega um registro pelo codigo
        public DataTable GetRegistroPorId(int id)
        {

            DataTable dt = new DataTable();

            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from tb_arquivospdfs Where id = " + id;

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }                   
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            return dt;
        }

        public DataTable GetRegistroPorIata()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select codigoiata from tb_modelo_iata";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
            return dt;
        }


        //Inserir registros
        public void InserirRegistros(string nome,string email, int idade)
        {
           
           
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("Insert Into tb_arquivospdfs(descricao,nrdocumentos,numerodocumento) values('{0}','{1}',{2})", nome, email, idade);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }                    
                }
           
        }


        //Inserir registros
        public void InserirRegistrosMaster(string nome, string email, int idade)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Insert Into tb_arquivospdfs(descricao,nrdocumentos,numerodocumento) values('{0}','{1}',{2})", nome, email, idade);

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }

        //Inserir registros dom Master
        public void InserirRegistrosMaster(Master rs)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Insert Into tb_pdfmaster(nrmaster,nrhouse,origem,Destino,agente,transportador,moedafrete,tipofrete,nrvoo,prevembarque,embarque,quantidade,pesobruto,pesotaxado,quantidadeprocesso) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')"
                    , rs.nrmaster, rs.nrhouse,rs.origem,rs.Destino,rs.agente,rs.transportador, rs.moedafrete,rs.tipofrete,rs.nrvoo, rs.prevembarque, rs.embarque, rs.quantidade, rs.pesobruto, rs.pesotaxado, rs.quantidadeprocesso);

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }

        //Inserir registros dom Master
        public void InserirRegistroshouse(House rs)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Insert Into tb_housepdfs(nrhouse,nrmaster,cliente,exportador,icoterm,origem,destino,agente,transportador,moedafrete,tipofrete,nrvoo,emissaoconhecimento,prevembarque,embarque,quantidade,pesobruto,pesotaxado,quantidadeprocesso,tar_vendamin) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')"
                    , rs.nrhouse, rs.nrmaster, rs.cliente, rs.Exportador, rs.icoterm, rs.origem, rs.Destino, rs.agente, rs.transportador, rs.moedafrete, rs.tipofrete, rs.nrvoo, rs.emissaoconhecimento, rs.prevembarque,rs.embarque,rs.quantidade,rs.pesobruto,rs.pesotaxado,rs.quantidadeprocesso,rs.tar_vendaMin);

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }
       
        //Atualiza registros
        public void AtualizarRegistro(int codigo, string email, int idade)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdAtualiza = String.Format("Update tb_arquivospdfs Set descricao = '" + email + "' , numerodocumento = " + idade + " Where id = " + codigo);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdAtualiza, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
        }

        //Deleta registros


         public class Master
         {
             public string nrmaster;
             public string nrhouse;
             public string origem;
             public string Destino;
             public string agente;
             public string transportador;
             public string moedafrete;
             public string tipofrete;
             public string nrvoo;
             public string emissaoconhecimento;
             public string prevembarque;
             public string embarque;
             public string quantidade;
             public string pesobruto;
             public string pesotaxado;
             public string quantidadeprocesso;            

         }

         public class House
         {
             public string nrhouse;
             public string nrmaster;
             public string cliente;
             public string Exportador;
             public string icoterm;
             public string tar_vendaMin;
             public string origem;
             public string Destino;
             public string agente;
             public string transportador;
             public string moedafrete;
             public string tipofrete;
             public string nrvoo;
             public string emissaoconhecimento;
             public string prevembarque;
             public string embarque;
             public string quantidade;
             public string pesobruto;
             public string pesotaxado;
             public string quantidadeprocesso;

         }


        public void DeletarRegistro(string nome)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //abre a conexao                
                    pgsqlConnection.Open();

                    string cmdDeletar = String.Format("Delete From tb_arquivospdfs Where descricao = '{0}'", nome);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdDeletar, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
        }
    }
}

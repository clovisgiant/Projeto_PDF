
using System.Windows.Forms;

using System.Linq;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System;
using System.Data;
using Npgsql;
using System.Text.RegularExpressions;
using System.Security;
using System.Diagnostics;


namespace PostgreSQL_CRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int codigo;
        string Agente;
        DataTable dt;
        DataTable dtDocumentos;
        DAL acesso = new DAL();
        private void btnExibir_Click(object sender, EventArgs e)
        {
            try
            {
                atualizarExibicao();
                dt = acesso.GetRegistroPorIata();
                for (int i = 0; i < dt.Rows.Count; i++)
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        object o = dt.Rows[i].ItemArray[j];
                        //if you want to get the string
                        string s =  dt.Rows[i].ItemArray[j].ToString();
                    }
                dt.Dispose();

                dtDocumentos = acesso.GetTodosRegistrosPDF();

                for (int d = 0; d < dtDocumentos.Rows.Count; d++)
                    for (int c = 0; c < dtDocumentos.Columns.Count; c++)
                    {
                        object ob = dtDocumentos.Rows[d].ItemArray[c];                        
                        string documentos = dtDocumentos.Rows[d].ItemArray[c].ToString();

                        string[] linespdf = Regex.Split(documentos, "\n");
                        string resultado = linespdf[1].ToString();
                        string cabecariopdf = linespdf[0].ToString();              
                                               

                        string result = Regex.Replace(resultado, @"[^\d]", "");

                        if (cabecariopdf == "NNR GLOBAL LOGISTICS USA INC" || resultado == "ADVANCE INFORMATION (AIR )")//layout NNR
                        {
                            bool nnr = true;
                        }
                        else if (cabecariopdf == "Consignee Copy" || resultado == "Not Negotiable")//layout NNR Master
                        {
                            bool master = true;
                            acesso.GetTodosRegistrosNNR(documentos,master);

                        }
                        else if (cabecariopdf == "Not Negotiable")// || !regex.IsMatch(result)) //layout NNR house
                        {
                            bool house = false;
                            acesso.GetTodosRegistrosNNR(documentos, house);
                            
                        }
                        else if (cabecariopdf == "MASTER AWB No. DUCR HAWB No.") //layout Aramex 
                        {
                            bool house = false;
                            acesso.GetTodosRegistrosAramex(documentos, house);
                           
                        }
                        else if (cabecariopdf == "It ") //layout cargo wiser
                        {
                            string[] linespdf2 = Regex.Split(documentos, "\n");
                            bool house = false;
                            acesso.GetTodosRegistroscargowiser(documentos, house);
                            
                        }
                        else if (resultado == "Shippers Name and Address Shippers Account Number Not negotiable") //layout iva non
                        {
                            bool house = false;
                            acesso.GetTodosRegistrosFREIGHTLOG(documentos, house);
                           
                        }

                        else if (resultado == "Shippers Name and Address Shippers Account Number Not Negotiable") //layout Modul AIR
                        {
                            bool house = false;
                            acesso.GetTodosRegistrosModulAIR(documentos, house);
                            
                        }

                        else if (resultado == "Shippers Name and Address Shippers Account Number") //layout CARGO-PARTNER
                        {
                            bool house = false;
                            acesso.GetTodosRegistrosCARGOPARTNER(documentos, house);                            
                        } 

                        else
                        {
                           // MessageBox.Show("Documento PDF não encontrado : " + cabecariopdf + " " + resultado);
                            Console.Write(" estou no master");
                        }
                      
                    }

                dtDocumentos.Dispose();

                MessageBox.Show("Erro :Terminei o Processamento com sucesso ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void atualizarExibicao()
        {
            dgvFunci.DataSource = acesso.GetTodosRegistros();
        }

        private void dgvFunci_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            //var rs = new Master();
           // string Agente ;
            codigo = Convert.ToInt32(dgvFunci.Rows[e.RowIndex].Cells[0].Value);
            Agente =  txtNomeold.Text = Convert.ToString(dgvFunci.Rows[e.RowIndex].Cells[1].Value);
           // txtEmail.Text = Convert.ToString(dgvFunci.Rows[e.RowIndex].Cells[2].Value);
          //  txtIdade.Text = Convert.ToString(dgvFunci.Rows[e.RowIndex].Cells[3].Value);

             //SeparaLinhas(Agente, 25);
            // acesso.AtualizarRegistro(codigo, SeparaLinhas(Agente, 25).ToString(), Convert.ToInt32(txtIdade.Text));
            
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            //if (txtNome.Text == string.Empty || txtEmail.Text == string.Empty || txtIdade.Text == string.Empty)
            //{
            //    txtNome.Focus();
            //    return;
            //}

            try
            {

                List<string> linhas;                
                var rs = new DAL.Master();
                
                string nrmaster;
                String str = Agente;

                int cont = 0;

                string[] words = str.Split(' ');

                string[] lines = Regex.Split(str, "\n");

                  string[] digits = Regex.Split(str, @"\D+");

                  Regex regex = new Regex(@"^\d$");


                  //////////////////foreach (string s in lines)
                  //////////////////{

                  //////////////////    string numeroIata;
                  //////////////////    numeroIata = s.ToString();
                  //////////////////    string input = numeroIata;
                  //////////////////    string result = Regex.Replace(input, @"[^\d]", "");

                  //////////////////    if (result == "0115401")
                  //////////////////        {
                  //////////////////           // acesso.GetTodosRegistrosNNR(str, master);
                  //////////////////        }
                  //////////////////    else if (result == "5747126")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "38478790011")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "06475136006")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "80470961900")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "99910910002")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "20473783175")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "81471034002")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "02359592233")
                  //////////////////        {
                  //////////////////            Mensagem();
                  //////////////////        }
                  //////////////////    else if (result == "17475302773")
                  //////////////////        {
                  //////////////////            Mensagem();

                  //////////////////        }

                  //////////////////    else
                  //////////////////    {
                  //////////////////        //goto BREAK; //MessageBox.Show("Erro : Não foi encontrado o modelo para este AWB " + result.ToString());
                  //////////////////    }


                  //////////////////}

                 // BREAK: ;
                //  MessageBox.Show("Erro : Não foi encontrado o modelo para este AWB " + result.ToString());



                  //int contador = 0;
                  //foreach (
                  //    string s in lines)
                  //{

                  //    for (int i = 0; i < lines.Length; i++)
                  //    {
                  //        if (s.ToString() == "Consignees Name and Address" || s.ToString() == "Consignees Name and Address Consignees Account Number")
                  //        {

                  //            string numeromaster;
                  //            numeromaster = lines[contador + 1];

                  //            var parts = Regex.Matches(numeromaster, @"\d+|\D+")
                  //            .Cast<Match>()
                  //            .Select(m => m.Value)
                  //            .ToList();

                  //            rs.nrmaster = parts[1] + parts[2] + parts[3].ToString();
                  //            rs.transportador = parts[1];
                  //        }
                  //        else if (s.ToString() == "Airport of Departure Addr.of First Carrier and Requested Routing")
                  //        {
                  //            string origem;
                  //            origem = lines[contador + 1];
                  //            rs.origem = origem.Substring(0, origem.Length);
                  //        }
                  //        else if (s.ToString().Trim() == "if Insurance.")
                  //        {
                  //            string Destino;
                  //            string nrvoo;


                  //            Destino = lines[contador + 1];
                  //            nrvoo = Destino.Substring(Destino.Length - 10);
                  //            rs.Destino = Destino.Substring(0, Destino.Length - 10);
                  //            rs.nrvoo = nrvoo.Substring(0, nrvoo.Length - 1);
                  //        }
                  //        else if (s.ToString() == "Issued By")
                  //        {
                  //            string agente;
                  //            agente = lines[contador + 1];
                  //            rs.agente = agente.Substring(0, agente.Length);
                  //        }
                  //        else if (s.ToString().Trim() == "Code")
                  //        {
                  //            string moeda;
                  //            moeda = lines[contador + 1];
                  //            moeda = moeda.Substring(moeda.Length - 7);

                  //            rs.moedafrete = moeda.Substring(0, moeda.Length - 3);
                  //        }
                  //        else if (s.ToString() == "Code")
                  //        {
                  //            string tipofrete;
                  //            tipofrete = lines[contador + 1];
                  //            rs.tipofrete = tipofrete.Substring(tipofrete.Length - 3);
                  //        }
                  //        else if (s.ToString().Trim() == "Executed on Date atplace Signature of the issuing Carrier or its  Agent")
                  //        {
                  //            string embarque;
                  //            embarque = lines[contador - 1];
                  //            embarque = embarque.Substring(embarque.Length - 16);
                  //            rs.embarque = embarque.Substring(0, embarque.Length - 3);
                  //            rs.emissaoconhecimento = embarque.Substring(0, embarque.Length - 3);
                  //            rs.prevembarque = embarque.Substring(0, embarque.Length - 3);
                  //        }

                  //        else
                  //        {
                  //            goto BREAK;
                  //        }
                  //    }
                  //BREAK: ;
                  //    contador++;
                  //}

                  //acesso.InserirRegistrosMaster(rs);

                //rs.nrmaster;
                //rs.origem;
                //rs.Destino;
                //rs.agente;
                //rs.transportador;
                //rs.moedafrete;
                rs.tipofrete = str.Substring(1004, 1);
                rs.nrvoo = str.Substring(1304, 10);
                rs.emissaoconhecimento = str.Substring(2322, 11);
                rs.prevembarque = str.Substring(2323, 11);
                rs.embarque = str.Substring(2323, 11);
                // rs.quantidade =  str.Substring(417, 12);
                // rs.pesobruto =  str.Substring(417, 12);
                // rs.pesotaxado =  str.Substring(417, 12);
                // rs.quantidadeprocesso = str.Substring(417, 12); 

                Console.WriteLine(rs.ToString());


                //acesso.AtualizarRegistro(codigo,txtEmail.Text, Convert.ToInt32(txtIdade.Text));
                atualizarExibicao();
                Mensagem();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
            btnInserir.Enabled = true;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
           // StreamWriter outFile = null;
            var file = txtArquivo.Text;   //"C:\\Users\\Clovis\\Desktop\\PDFs_NNR\\Email - HAWB No_ TLS7G062106.pdf";
           // var outFileName = "testePDF.pdf";
            var output = "C:\\Users\\Clovis\\Downloads\\mawb_copia.txt";

            PdfReader reader = new PdfReader(file);
            //outFile = File.CreateText(outFileName);
            //outFile = new StreamWriter(outFileName, false, System.Text.Encoding.UTF8);
            //Console.Write("Processing: ");
            DateTime localDate = DateTime.Now;
            string data = localDate.ToString();

            var sb = new StringBuilder();
            string texto = sb.ToString();
            //Substring(startIndex, length));

            if (!File.Exists(file)) return;
            var bytes = File.ReadAllBytes(file);

            var numberOfPages = reader.NumberOfPages;
            
            try
            {

                    for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {

                        //Console.Write(ConvertToText(bytes) + "PrimeiroArquivo-----------------" + currentPageIndex.ToString() + "---------------------" + "\n");

                     //   List<string> linhas;
                       // string texto3 = ConvertToText(bytes);
                       // string textoresumido = currentPageIndex.ToString() + "--------------------FIM DO ARQUIVO---------------------------------------";
                       // String str = texto3;
                       // String toFind = textoresumido;
                       // int index = str.IndexOf(textoresumido);
                      //  Console.WriteLine("Achei a linhana posicao: '{0}' in '{1}' at position {2}",
                      //   toFind, str, index);

                       // texto3.Replace("\n", "");
                        //acesso.InserirRegistros(RemoveSpecialChars(ConvertToText(bytes)) + "Inicio-----------------" + currentPageIndex.ToString() + "---------------------inicio" + "\n", txtEmail.Text, currentPageIndex);
                        acesso.InserirRegistros(RemoveSpecialChars(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex)), localDate.ToString(), currentPageIndex);
                      ///  acesso.InserirRegistros(RemoveSpecialChars(texto3.ToString()), txtEmail.Text, currentPageIndex);
                           // Console.WriteLine("Funcao SeparaLinhas" + SeparaLinhas(ConvertToText(bytes), 30));
                        txtArquivo.Text = "";
                   
                    };
                    dgvFunci.DataSource = acesso.GetTodosRegistros();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }      
        }

        public static string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }

        private static string ConvertToText(byte[] bytes)
        {

            var sb = new StringBuilder();
            string texto = sb.ToString();
            //Substring(startIndex, length));


            try
            {
                var reader = new PdfReader(bytes);
                var numberOfPages = reader.NumberOfPages;
                int house = 0;
                string texto2;

                for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                {
                    string textoresumido;
                    sb.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex) + "\n" + currentPageIndex + "--------------------FIM DO ARQUIVO---------------------------------------" + "\n");               




                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }



            return sb.ToString();
        }
		
        public void limpaTextBox(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                if (c.HasChildren)
                {
                    limpaTextBox(c);
                }
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (txtNomeold.Text == string.Empty)
            {
                return;
            }

            try
            {
                acesso.DeletarRegistro(txtNomeold.Text);
                atualizarExibicao();
                Mensagem();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void Mensagem()
        {
            MessageBox.Show("Operação realizada com sucesso !");
        }



        private static List<string> SeparaLinhas(string texto, int largura)
        {
            string tx = null;
            int x = 0;
            int lt = 0;
            int l = 0;
            int j = 0;
            int k = 0;
            string C = null;
            List<string> linhas = new List<string>();
            string linha = null;

            x = 0;
            tx = texto;
        PROCESSA:
            j = 1;
            lt = tx.Length;

            //'retira space, cr e lf superfluos
            while (j < lt)
            {
                C = tx.Substring(j - 1, 1);
                if ((C == " " | C == "\r" | C == "\n"))
                {
                }
                else
                {
                    break; // TODO: might not be correct. Was : Exit While
                }
                j = j + 1;
            }

            if (j != 1)
                tx = tx.Substring(j - 1, lt - (j - 1));

            lt = tx.Length;
            l = lt;
            j = 1;

            if (lt < largura)
            {
                x = 1;
            }
            else
            {
                l = largura;
            }

            //'procura o primeiro separador
            while (j <= l)
            {
                C = tx.Substring(j - 1, 1);
                if ((C == " " | C == "\r" | C == "\n"))
                {
                }
                else
                {
                    j = j + 1;
                    continue;
                }

                //'Separador é cr ou lf
                if (C != " ")
                {
                    j = j - 1;
                    break; // TODO: might not be correct. Was : Exit While
                }
                k = j + 1;

                //'procura o proximo separador
                while (k <= l)
                {
                    C = tx.Substring(k - 1, 1);
                    if ((C == "\r" | C == "\n"))
                    {
                        j = k - 1;
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    if (C == " ")
                        j = k;
                    k = k + 1;
                }

                if (x == 1 & k > l)
                    j = l;
                break; // TODO: might not be correct. Was : Exit While
            }

            if (j > l)
                j = l;

            linha = tx.Substring(0, j);
            linhas.Add(linha);

            if (j >= lt)
            {
            }
            else
            {
                tx = tx.Substring(j, (lt - j));
                if (tx != "\r\n")
                    goto PROCESSA;

            }

            return linhas;
        }



       
       

         private void btnSelecionarArquivos_Click(object sender, EventArgs e)
         {

             //define as propriedades do controle 
             //OpenFileDialog
             this.ofd1.Multiselect = true;
             this.ofd1.Title = "Selecionar Fotos";
             ofd1.InitialDirectory = @"C:\";
             //filtra para exibir somente arquivos de imagens
             ofd1.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
             ofd1.CheckFileExists = true;
             ofd1.CheckPathExists = true;
             ofd1.FilterIndex = 2;
             ofd1.RestoreDirectory = true;
             ofd1.ReadOnlyChecked = true;
             ofd1.ShowReadOnly = true;

             DialogResult dr = this.ofd1.ShowDialog();

             if (dr == System.Windows.Forms.DialogResult.OK)
             {
                 // Le os arquivos selecionados 
                 foreach (String file in ofd1.FileNames)
                 {
                     txtArquivo.Text += file;
                     // cria um PictureBox
                     try
                     {
                         PictureBox pb = new PictureBox();
                        // Image Imagem = Image.FromFile(file);
                         pb.SizeMode = PictureBoxSizeMode.StretchImage;
                         //para exibir as imagens em tamanho natural 
                         //descomente as linhas abaixo e comente as duas seguintes
                         //pb.Height = loadedImage.Height;
                         //pb.Width = loadedImage.Width;
                         pb.Height = 100;
                         pb.Width = 100;
                         //atribui a imagem ao PictureBox - pb
                         //pb.Image = Imagem;
                         //inclui a imagem no containter flowLayoutPanel
                         //flowLayoutPanel1.Controls.Add(pb);
                     }
                     catch (SecurityException ex)
                     {
                         // O usuário  não possui permissão para ler arquivos
                         MessageBox.Show("Erro de segurança Contate o administrador de segurança da rede.\n\n" +
                                         "Mensagem : " + ex.Message + "\n\n" +
                                         "Detalhes (enviar ao suporte):\n\n" + ex.StackTrace);
                     }
                     catch (Exception ex)
                     {
                         // Não pode carregar a imagem (problemas de permissão)
                         MessageBox.Show("Não é possível exibir a imagem : " + file.Substring(file.LastIndexOf('\\'))
                                          + ". Você pode não ter permissão para ler o arquivo , ou " +
                                          " ele pode estar corrompido.\n\nErro reportado : " + ex.Message);
                     }
                 }
             }





         }

         private void button1_Click(object sender, EventArgs e)
         {

             ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Users\\Clovis\\Desktop\\CriandoXML\\bin\\Debug\\CriandoXML.exe");

            // startInfo.Arguments = "header.h"; // your arguments

             Process.Start(startInfo);
         }


				 



    }
}

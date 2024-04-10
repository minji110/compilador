using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDE.Lexico;
using IDE.Parser;
using IDE.Semantico;
using IDE.Intermedio;

namespace IDE
{
    public partial class Saoko_BNF_diseño : Form
    {
        lexer scanner;
        Sintactico parser;
        semantico semantico;
        bool seCompletoParser=true;
        bool seCompletoSemantico = true;
       
        public Saoko_BNF_diseño()
        {
            InitializeComponent();
        }

        public void principal()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void parserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scannerToolStripMenuItem_Click(sender, e);
            parser = new Sintactico();
            try
            {
                seCompletoParser = true;
                parser.parser(scanner.Tokens);
                Mensaje_Sintactico.Text = "Esta bien sintacticamente";
            }
            catch(Exception eex)
            {
                Mensaje_Sintactico.Text="Esta mal sintacticamente"+eex.Message;
                seCompletoParser = false;
                Label_Semantico.Text = " ";
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            dataGridView2.DataSource = null;
            textBoxPrograma.Clear();
            Mensaje_Sintactico.Text = "";
            Label_Semantico.Text = "";
            textBox_intermedio.Text = "";
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            explorador();
        }

        public void explorador()
        {
            // Crear un OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configurar el OpenFileDialog
            openFileDialog.InitialDirectory = @"C:\"; // Directorio inicial
            openFileDialog.Title = "Abrir archivo de texto"; // Título del diálogo
            openFileDialog.Filter = "Documentos de texto (*.txt)|*.txt"; // Solo archivos de texto
            openFileDialog.FilterIndex = 1; // Índice predeterminado del filtro
            openFileDialog.RestoreDirectory = true; // Restaurar el directorio anterior al cerrar el diálogo

            // Mostrar el diálogo y esperar la respuesta del usuario
            DialogResult result = openFileDialog.ShowDialog();

            // Procesar el resultado del diálogo
            if (result == DialogResult.OK) // Si el usuario hace clic en "Abrir"
            {
                try
                {
                    // Obtener la ruta del archivo seleccionado
                    string filePath = openFileDialog.FileName;

                    // Leer el contenido del archivo
                    string fileContent = System.IO.File.ReadAllText(filePath);

                    // Mostrar el contenido del archivo en el TextBox
                    textBoxPrograma.Text = fileContent;
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error al abrir o leer el archivo
                    MessageBox.Show("Error al abrir el archivo: " + ex.Message);
                }
            }
            else if (result == DialogResult.Cancel) // Si el usuario hace clic en "Cancelar" o cierra el diálogo
            {
                MessageBox.Show("Operación cancelada por el usuario.");
            }
        }


        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxPrograma.Text)){
                DialogResult result = MessageBox.Show("¿Desea guardar el contenido del TextBox?", 
                    "Guardar Contenido", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    explorador();
                    Console.WriteLine("Se guardó el contenido del TextBox: " + textBoxPrograma.Text);
                }
            }
            explorador();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void scannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string codigo = textBoxPrograma.Text;
            scanner = new lexer();
            scanner.Cadena_A_Tokens(codigo);
            scanner.Tokens.ForEach(items => Console.WriteLine(items));

            dataGridView2.DataSource = scanner.Tokens;
        }

        private void semanticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            scannerToolStripMenuItem_Click(sender, e);
            parserToolStripMenuItem_Click(sender, e);
            semantico= new semantico();
            if (!seCompletoParser)
            {
                return;
            }
            try
            {
                semantico.semantico1(scanner.Tokens);
                Label_Semantico.Text= "Semanticamente esta bien";
            }
            catch (Exception ex)
            {
                Label_Semantico.Text = "Semanticamente esta mal: "+ex.Message;
            }
        }

        private void textBox_intermedio_TextChanged(object sender, EventArgs e)
        {

        }

        private void intermedioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scannerToolStripMenuItem_Click(sender, e);
            parserToolStripMenuItem_Click(sender, e);
            semanticoToolStripMenuItem_Click(sender, e);

            if (!seCompletoSemantico)
            {
                return;
            }
            /*Direction directorio = new Direction(scanner.Tokens);
            directorio.Run();
            directorio.quadruples.ForEach(Console.WriteLine);*/

            Intermedio1 intermedio = new Intermedio1(scanner.Tokens);
            intermedio.Run();
            intermedio.GetAssembly();
            String asm = String.Join("\r\n", intermedio.GetAssembly());
            Console.WriteLine(asm);
            textBox_intermedio.Text = asm;
        }

        private void todosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

using NLog;

namespace UI
{
    public partial class Main : Form
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Main()
        {
            InitializeComponent();
        }
        
        public TextBox LoggerBox {
            get
            {
                return this.textBox4;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.textBox4.Enabled = false;
            this.button1.Enabled = false;

            var prg = new MVMP3.Program();

            new Thread(() =>
                prg.Start (this.textBox1.Text, this.textBox2.Text,this.Logger)).Start ();

            this.textBox1.Enabled = true;
            this.textBox2.Enabled = true;
            this.textBox4.Enabled = true;
            this.button1.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
    }
}
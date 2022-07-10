using NLog;

namespace UI;

public partial class Main : Form
{
    private readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public Main()
    {
        InitializeComponent();
    }

    public TextBox LoggerBox { get; private set; }

    private void textBox3_TextChanged(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e)
    {
        textBox1.Enabled = false;
        textBox2.Enabled = false;
        LoggerBox.Enabled = false;
        button1.Enabled = false;

        var prg = new MVMP3.Program();

        new Thread(() =>
            prg.Start(textBox1.Text, textBox2.Text, Logger)).Start();

        textBox1.Enabled = true;
        textBox2.Enabled = true;
        LoggerBox.Enabled = true;
        button1.Enabled = true;
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
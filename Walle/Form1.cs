using Geo_Wall_E;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Walle
{
    public partial class MainForm : Form
    {
        private TextBox inputTextBox;
        private Button compileButton;
        private Button runButton;
        private Button stopDrawing;
        private Label writingLabel;
        private Label errorLabel;
        private Panel drawingPanel;
        private PictureBox drawingBox;
        private PictureBox gymWalle;
        private PictureBox programmerWalle;
        private List<IDrawable> drawables;
        private bool isDrawing;
        public MainForm()
        {
            inputTextBox = new TextBox();
            compileButton = new Button();
            runButton = new Button();
            stopDrawing = new Button();
            writingLabel = new Label();
            errorLabel = new Label();
            drawingPanel = new Panel();
            drawingBox = new PictureBox();
            gymWalle = new PictureBox();
            programmerWalle = new PictureBox();
            drawables = new List<IDrawable>();
            isDrawing = false;

            //InitializeComponent();
            compileButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            compileButton.Cursor = System.Windows.Forms.Cursors.Default;
            compileButton.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            compileButton.Click += CompileButton_Click;
            compileButton.Size = new System.Drawing.Size(97, 42);
            compileButton.Location = new System.Drawing.Point(460, 75);
            compileButton.Text = "Compile";
            compileButton.UseVisualStyleBackColor = false;

            runButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            runButton.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            runButton.Click += RunButton_Click;
            runButton.Size = new System.Drawing.Size(97, 42);
            runButton.Location = new System.Drawing.Point(460, 160);
            runButton.Text = "Draw";
            runButton.Enabled = false;
            runButton.UseVisualStyleBackColor = false;

            stopDrawing.BackColor = System.Drawing.SystemColors.ButtonFace;
            stopDrawing.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            stopDrawing.Click += StopDrawing_Click;
            stopDrawing.Location = new System.Drawing.Point(460, 240);
            stopDrawing.Size = new System.Drawing.Size(97, 42);
            stopDrawing.Text = "Stop Drawing";
            stopDrawing.Enabled = false;
            stopDrawing.UseVisualStyleBackColor = false;

           

            inputTextBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            inputTextBox.Location = new System.Drawing.Point(50, 75);
            inputTextBox.Multiline = true;
            inputTextBox.Size = new System.Drawing.Size(322, 350);
            inputTextBox.TextChanged += new EventHandler(TextBoxInput_TextChanged);

            writingLabel.AutoSize = true;
            writingLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            writingLabel.Location = new System.Drawing.Point(165, 46);
            writingLabel.Size = new System.Drawing.Size(38, 15);
            writingLabel.Text = "Write your code";

            errorLabel.AutoSize = false;
            errorLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            errorLabel.Location = new System.Drawing.Point(405, 326);
            errorLabel.Size = new System.Drawing.Size(277, 86);
            errorLabel.Text = "Errores de Compilación";

            drawingPanel.AutoScroll = true;
            drawingPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            drawingPanel.Location = new System.Drawing.Point(690, 78); 
            drawingPanel.Size = new Size(628, 552);

            drawingBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            drawingBox.Location = new System.Drawing.Point(0, 0); 
            drawingBox.Size = new System.Drawing.Size(3500, 3500); 
            drawingBox.TabStop = false;
            drawingBox.Paint += new PaintEventHandler(drawingBox_Paint);

            programmerWalle.BackColor = System.Drawing.SystemColors.ControlLightLight;
            programmerWalle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            programmerWalle.Location = new System.Drawing.Point(37, 480);
            programmerWalle.Size = new System.Drawing.Size(156, 138);
            programmerWalle.TabStop = false;
            programmerWalle.SizeMode = PictureBoxSizeMode.StretchImage;
            programmerWalle.Image = Image.FromFile("OIG (3).jpg");

            gymWalle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            gymWalle.Location = new System.Drawing.Point(208, 550);
            gymWalle.Size = new System.Drawing.Size(156, 138);
            gymWalle.TabStop = false;
            gymWalle.SizeMode = PictureBoxSizeMode.StretchImage;
            gymWalle.Image = Image.FromFile("OIG (2).jpg");



            BackColor = System.Drawing.Color.DimGray;
            WindowState = FormWindowState.Maximized;
            Controls.Add(runButton);
            Controls.Add(compileButton);
            Controls.Add(stopDrawing);
            Controls.Add(inputTextBox);
            Controls.Add(writingLabel);
            Controls.Add(errorLabel);
            Controls.Add(drawingPanel);
            drawingPanel.Controls.Add(drawingBox);
            Controls.Add(gymWalle);
            Controls.Add(programmerWalle);
            Cursor = System.Windows.Forms.Cursors.Default;
            ForeColor = System.Drawing.SystemColors.ControlText;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            ((System.ComponentModel.ISupportInitialize)(this.drawingBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void AddDrawable(IDrawable drawable)
        {
            drawables.Add(drawable);
            // Para que el PictureBox se redibuje
            drawingBox.Invalidate(); 
        }

        private void TextBoxInput_TextChanged(object sender, EventArgs e)
        {
            runButton.Enabled = false;
        }
        private void CompileButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtiene el texto del inputTextBox
                string userInput = inputTextBox.Text;

                // Crea una instancia de la clase Compiler con la entrada del usuario
                Compiler compiler = new Compiler(userInput);

                //Asigna la lista 'result' de la instancia de 'Compiler' a tu lista 'drawables'
                drawables = compiler.Result;
                // Si la compilaci�n es exitosa, habilita el bot�n 'Run'
                runButton.Enabled = true;
                errorLabel.Text = "Errores de Compilación"; // Limpia el texto del label de error
            }
            catch (Exception ex)
            {
                // Si hay un error en la compilaci�n, muestra el mensaje de error en el label
                errorLabel.Text = ex.Message;
                runButton.Enabled = false; // Deshabilita el bot�n 'Run'
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {

            isDrawing = true;
            stopDrawing.Enabled = true;
            drawingBox.Invalidate();
            //DibujarFiguras(drawingBox.CreateGraphics());
            stopDrawing.Enabled = false;
            runButton.Enabled = false;
        }

        private void StopDrawing_Click(object sender, EventArgs e)
        {
            isDrawing = false;
            stopDrawing.Enabled = false;
        }

       
        private void drawingBox_Paint(object sender, PaintEventArgs e)
        {
            
            foreach (var drawable in drawables)
            {
                if (isDrawing == false)
                {
                    return;
                }
                System.Drawing.Color color = SearchColor(drawable);
                Pen pen = new Pen(color);
                if (drawable.Phrase != null)
                {
                    Random random = new Random();
                    // Genera coordenadas aleatorias dentro del PictureBox
                    int x = random.Next(drawingBox.Width);
                    int y = random.Next(drawingBox.Height);

                    // Define el string que quieres dibujar
                    string text = drawable.Phrase;

                    // Define el formato del texto
                    Font font = new Font("Arial", 16);
                    SolidBrush brush = new SolidBrush(color);

                    // Dibuja el string en las coordenadas aleatorias
                    e.Graphics.DrawString(text, font, brush, x, y);
                }

                switch (drawable.Type)
                {
                    case Geo_Wall_E.Point:
                        DrawPoint(e.Graphics, pen, drawable.Type);
                        break;
                    case Line:
                        DrawLine(e.Graphics, pen, drawable.Type);
                        break;
                    case Segment:
                        DrawSegment(e.Graphics, pen, drawable.Type);
                        break;
                    case Ray:
                        DrawRay(e.Graphics, pen, drawable.Type);
                        break;
                    case Circle:
                        DrawCircle(e.Graphics, pen, drawable.Type);
                        break;
                    case Arc:
                        DrawArc(e.Graphics, pen, drawable.Type);
                        break;
                    case Sequence:
                        DrawSequence(e.Graphics, pen, drawable.Type);
                        break;
                    default:
                        errorLabel.Text = "\"Los \" @ drawable.Type @ \"no se pueden dibujar\"";
                        break;
                }
                
            }

        }
       

        private System.Drawing.Color SearchColor(IDrawable drawable)
        {
            return drawable.Color switch
            {
                Geo_Wall_E.Color.RED => System.Drawing.Color.Red,
                Geo_Wall_E.Color.GREEN => System.Drawing.Color.Green,
                Geo_Wall_E.Color.BLUE => System.Drawing.Color.Blue,
                Geo_Wall_E.Color.YELLOW => System.Drawing.Color.Yellow,
                Geo_Wall_E.Color.CYAN => System.Drawing.Color.Cyan,
                Geo_Wall_E.Color.BLACK => System.Drawing.Color.Black,
                Geo_Wall_E.Color.MAGENTA => System.Drawing.Color.Magenta,
                Geo_Wall_E.Color.WHITE => System.Drawing.Color.White,
                Geo_Wall_E.Color.GRAY => System.Drawing.Color.Gray,
            };
        }
        private  void DrawPoint(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
  

            Geo_Wall_E.Point point = (Geo_Wall_E.Point)drawable;
            g.DrawEllipse(pencil, (int)point.X + drawingPanel.HorizontalScroll.Value, (int)point.Y, 5, 5);
        }

        private void DrawLine(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
            Line l = (Line)drawable;
            // Calcular la pendiente de la l�nea
            double m = (l.P2.Y - l.P1.Y) / (l.P2.X - l.P1.X);

            // Calcular los puntos de intersecci�n con los bordes de la pantalla
            int xLeft = 0;
            int yLeft = (int)(l.P1.Y - m * l.P1.X);
            int xRight = drawingBox.Width;
            int yRight = (int)(m * (xRight - l.P1.X) + l.P1.Y);

            // Dibujar una l�nea desde el borde izquierdo hasta el borde derecho de la pantalla
            g.DrawLine(pencil, xLeft, yLeft, xRight, yRight);
        }

        private void DrawSegment(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
            Segment s = (Segment)drawable;
            // Dibujar una l�nea desde el punto inicial hasta el punto final
            g.DrawLine(pencil, (int)s.Start.X, (int)s.Start.Y, (int)s.End.X, (int)s.End.Y);
        }

        private void DrawRay(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
            Ray r = (Ray)drawable;
            // Calcular la pendiente de la l�nea
            double m = (r.Point.Y - r.Start.Y) / (r.Point.X - r.Start.X);

            if (r.Start.Y < r.Point.Y)
            {
                // Si el punto de inicio est� por encima del punto final, dibuja hacia la izquierda
                int xLeft = 0;
                int yLeft = (int)(r.Start.Y - m * r.Start.X);
                g.DrawLine(pencil, (int)r.Start.X, (int)r.Start.Y, xLeft, yLeft);
            }
            else
            {
                // Si el punto de inicio est� por debajo del punto final, dibuja hacia la derecha
                int xRight = drawingBox.Width;
                int yRight = (int)(m * (xRight - r.Start.X) + r.Start.Y);
                g.DrawLine(pencil, (int)r.Start.X, (int)r.Start.Y, xRight, yRight);
            }
        }

        private void DrawCircle(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
            Circle c = (Circle)drawable;
            g.DrawEllipse(pencil, (int)(c.Center.X - c.Radius.Measure_), (int)(c.Center.Y - c.Radius.Measure_), (int)c.Radius.Measure_ * 2, (int)c.Radius.Measure_ * 2);
        }

        private void DrawArc(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
            Arc a = (Arc)drawable;
            // Calcular los �ngulos de inicio y final
            float startAngle = (float)(Math.Atan2(a.Start.Y - a.Center.Y, a.Start.X - a.Center.X) * 180 / Math.PI);
            float endAngle = (float)(Math.Atan2(a.End.Y - a.Center.Y, a.End.X - a.Center.X) * 180 / Math.PI);

            // Asegurarse de que el arco se dibuja en sentido contrario a las manecillas del reloj
            if (endAngle < startAngle)
            {
                endAngle += 360;
            }

            // Calcular el �ngulo de barrido
            float sweepAngle = endAngle - startAngle;

            // Dibujar las semirrectas
            g.DrawLine(pencil, (int)a.Center.X, (int)a.Center.Y, (int)a.Start.X, (int)a.Start.Y);
            g.DrawLine(pencil, (int)a.Center.X, (int)a.Center.Y, (int)a.End.X, (int)a.End.Y);

            // Dibujar el arco
            g.DrawArc(pencil, (int)(a.Center.X - a.Measure.Measure_), (int)(a.Center.Y - a.Measure.Measure_), (int)a.Measure.Measure_ * 2, (int)a.Measure.Measure_ * 2, startAngle, sweepAngle);
        }
        private void DrawSequence(Graphics g, Pen pencil, Geo_Wall_E.Type drawable)
        {
            Sequence sequence = (Sequence)drawable;
            foreach (var item in sequence.Elements)
            {
                switch (item)
                {
                    case Geo_Wall_E.Point:
                        DrawPoint(g, pencil, item);
                        break;
                    case Line:
                        DrawLine(g, pencil, item);
                        break;
                    case Segment:
                        DrawSegment(g, pencil, item);
                        break;
                    case Ray:
                        DrawRay(g, pencil, item);
                        break;
                    case Circle:
                        DrawCircle(g, pencil, item);
                        break;
                    case Arc:
                        DrawArc(g, pencil, item);
                        break;
                    case Sequence:
                        DrawSequence(g, pencil, item);
                        break;
                    default:
                        errorLabel.Text = "\"Los \" @ drawable.Type @ \"no se pueden dibujar\"";
                        break;
                }
            }
        }


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        
    }
}

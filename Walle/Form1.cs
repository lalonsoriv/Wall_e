using Geo_Wall_E;
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
        private Button buttonUp;
        private Button buttonDown;
        private Button buttonRight;
        private Button buttonLeft;
        private Label writingLabel;
        private Label errorLabel;
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
            buttonUp = new Button();
            buttonDown = new Button();
            buttonRight = new Button();
            buttonLeft = new Button();
            writingLabel = new Label();
            errorLabel = new Label();
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
            compileButton.Location = new System.Drawing.Point(94, 240);
            compileButton.Text = "Compile";
            compileButton.UseVisualStyleBackColor = false;

            runButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            runButton.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            runButton.Click += RunButton_Click;
            runButton.Size = new System.Drawing.Size(97, 42);
            runButton.Location = new System.Drawing.Point(223, 240);
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

            buttonUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            buttonUp.Cursor = System.Windows.Forms.Cursors.Default;
            buttonUp.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonUp.Click += buttonUp_Click;
            buttonUp.Size = new System.Drawing.Size(50, 45);
            buttonUp.Location = new System.Drawing.Point(480, 63);
            buttonUp.Text = "up";
            buttonUp.UseVisualStyleBackColor = false;

            buttonDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            buttonDown.Cursor = System.Windows.Forms.Cursors.Default;
            buttonDown.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonDown.Click += buttonDown_Click;
            buttonDown.Size = new System.Drawing.Size(50, 45);
            buttonDown.Location = new System.Drawing.Point(480, 167);
            buttonDown.Text = "down";
            buttonDown.UseVisualStyleBackColor = false;

            buttonRight.BackColor = System.Drawing.SystemColors.ButtonFace;
            buttonRight.Cursor = System.Windows.Forms.Cursors.Default;
            buttonRight.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonRight.Click += buttonRight_Click;
            buttonRight.Size = new System.Drawing.Size(50, 45);
            buttonRight.Location = new System.Drawing.Point(536, 117);
            buttonRight.Text = "right";
            buttonRight.UseVisualStyleBackColor = false;

            buttonLeft.BackColor = System.Drawing.SystemColors.ButtonFace;
            buttonLeft.Cursor = System.Windows.Forms.Cursors.Default;
            buttonLeft.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonLeft.Click += buttonLeft_Click;
            buttonLeft.Size = new System.Drawing.Size(50, 45);
            buttonLeft.Location = new System.Drawing.Point(424, 117);
            buttonLeft.Text = "left";
            buttonLeft.UseVisualStyleBackColor = false;

            inputTextBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            inputTextBox.Location = new System.Drawing.Point(50, 75);
            inputTextBox.Multiline = true;
            inputTextBox.Size = new System.Drawing.Size(322, 159);
            inputTextBox.TextChanged += new EventHandler(TextBoxInput_TextChanged);

            writingLabel.AutoSize = true;
            writingLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //writingLabel.ForeColor = System.Drawing.Color.SeaGreen;
            writingLabel.Location = new System.Drawing.Point(165, 46);
            writingLabel.Size = new System.Drawing.Size(38, 15);
            writingLabel.Text = "Write your code";

            errorLabel.AutoSize = false;
            errorLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //errorLabel.ForeColor = System.Drawing.Color.RosyBrown;
            errorLabel.Location = new System.Drawing.Point(325, 326);
            errorLabel.Size = new System.Drawing.Size(277, 86);
            errorLabel.Text = "Compilation Errors";

            drawingBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            drawingBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            drawingBox.Location = new System.Drawing.Point(690, 78);
            drawingBox.Size = new System.Drawing.Size(628, 552);
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
            Controls.Add(buttonUp);
            Controls.Add(buttonDown);
            Controls.Add(buttonRight);
            Controls.Add(buttonLeft);
            Controls.Add(inputTextBox);
            Controls.Add(writingLabel);
            Controls.Add(errorLabel);
            Controls.Add(drawingBox);
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
                errorLabel.Text = "Errores de Compilaci�n"; // Limpia el texto del label de error
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

        private void buttonUp_Click(object sender, EventArgs e)
        {
            drawingBox.Top -= 10;
        }

        // Bot�n para mover hacia abajo
        private void buttonDown_Click(object sender, EventArgs e)
        {
            drawingBox.Top += 10;

        }

        // Bot�n para mover hacia la izquierda
        private void buttonLeft_Click(object sender, EventArgs e)
        {
            drawingBox.Location = new System.Drawing.Point(drawingBox.Location.X - 10, drawingBox.Location.Y);
        }

        // Bot�n para mover hacia la derecha
        private void buttonRight_Click(object sender, EventArgs e)
        {
            drawingBox.Location = new System.Drawing.Point(drawingBox.Location.X + 10, drawingBox.Location.Y);
        }
        private void drawingBox_Paint(object sender, PaintEventArgs e)
        {
            // Usa el objeto Graphics que se pasa como argumento
            //Graphics g = e.Graphics;
            // Llama al m�todo DibujarFiguras con este objeto Graphics
            //DibujarFiguras(g);
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
                        DrawPoint(e.Graphics, pen, drawable);
                        break;
                    case Line:
                        DrawLine(e.Graphics, pen, drawable);
                        break;
                    case Segment:
                        DrawSegment(e.Graphics, pen, drawable);
                        break;
                    case Ray:
                        DrawRay(e.Graphics, pen, drawable);
                        break;
                    case Circle:
                        DrawCircle(e.Graphics, pen, drawable);
                        break;
                    case Arc:
                        DrawArc(e.Graphics, pen, drawable);
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
        private static void DrawPoint(Graphics g, Pen pencil, IDrawable drawable)
        {
            Geo_Wall_E.Point point = (Geo_Wall_E.Point)drawable.Type;
            g.DrawEllipse(pencil, (int)point.X, (int)point.Y, 5, 5);
        }

        private void DrawLine(Graphics g, Pen pencil, IDrawable drawable)
        {
            Line l = (Line)drawable.Type;
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

        private void DrawSegment(Graphics g, Pen pencil, IDrawable drawable)
        {
            Segment s = (Segment)drawable.Type;
            // Dibujar una l�nea desde el punto inicial hasta el punto final
            g.DrawLine(pencil, (int)s.Start.X, (int)s.Start.Y, (int)s.End.X, (int)s.End.Y);
        }

        private void DrawRay(Graphics g, Pen pencil, IDrawable drawable)
        {
            Ray r = (Ray)drawable.Type;
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

        private void DrawCircle(Graphics g, Pen pencil, IDrawable drawable)
        {
            Circle c = (Circle)drawable.Type;
            g.DrawEllipse(pencil, (int)(c.Center.X - c.Radius.Measure_), (int)(c.Center.Y - c.Radius.Measure_), (int)c.Radius.Measure_ * 2, (int)c.Radius.Measure_ * 2);
        }

        private void DrawArc(Graphics g, Pen pencil, IDrawable drawable)
        {
            Arc a = (Arc)drawable.Type;
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


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        
    }
}

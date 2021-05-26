
//Multiple face detection and recognition in real time
//Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
//Writed by Sergio Andrés Guitérrez Rojas
//"Serg3ant" for the delveloper comunity
// Sergiogut1805@hotmail.com
//Regards from Bucaramanga-Colombia ;)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;
using ConnectCsharpToMysql;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Declaración de variables, listados y HaarCascade
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, name2, names = null;

        private DBConnect DBConnect;

        
        int selectedRow;
     
        public FrmPrincipal()
        {
            
            InitializeComponent();
            
            //Cargado de HaarCascades para detección de rostros
            face = new HaarCascade("haarcascade_frontalface_default.xml");

            try
            {
                //Cargado imagenes previas y nombres
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');

                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    //Cargado de cada rostro
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }


            }
            catch (Exception e)
            {
                //Excepción "Base de datos vacia"
                MessageBox.Show("Nada en la base de datos, por favor agregue un rostro", "Rostros entrenados cargados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void lblDireccion_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Inicia el dispositivo de captura de imagen
            grabber = new Capture();
            grabber.QueryFrame();

            //Inicia el evento FrameGrabber
            Application.Idle += new EventHandler(FrameGrabber);
            button1.Enabled = false;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Contador de caras entrenadas
                ContTrain = ContTrain + 1;

                //Obtener frame en escala de grises de la captura
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Detector de rostros
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Por cada elemento detectado
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //Cambiar el tamaño del rostro detectado para comparar con la imagen del mismo tamaño
                //con cubic método de tipo de interpolación cúbica
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Mostrar rostro agregado en escala de grises
                imageBox1.Image = TrainedFace;

                //Escribir el número de caras entrenadas para futuras cargas
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");



                //Escribir los labes de los rostros entrenados en un archivo para futuras cargas
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show("Rostro de " + textBox1.Text + "´detectado y guardado", "Entrenamiento correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";

            }
            catch
            {
                //Errores de activación de la camara y guardado de información
                MessageBox.Show("Active la detección de rostros", "Entrenamiento Fallado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Obtener el frame actual del dispositivo de captura
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Convertir en escala de grises
                    gray = currentFrame.Convert<Gray, Byte>();

                    //Detector de rostros
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                  face,
                  1.2,
                  10,
                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                  new Size(20, 20));

                    //Acción por cada elemento detectado
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        t = t + 1;
                        result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        //Dibujar la cara detectada en el canal gris con color azul
                        currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                        if (trainingImages.ToArray().Length != 0)
                        {
                        //TermCriteria para reconocimiento facial con números de imagenes entrenadas como maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Reconocedor de rostros de Eigen 
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);
                        name2 = name;

                        //Escribir los labels por cada rostro detectado y reconocido
                        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                        }

                            NamePersons[t-1] = name;
                            NamePersons.Add("");


                        //Establecer el número de caras detectadas
                        label3.Text = facesDetected[0].Length.ToString();

                    }
                        t = 0;

                    //Concatenación de nombres de personas
                    for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                    {
                        names = names + NamePersons[nnn] + ", ";
                    }

                    //Mostrar las caras procesadas y reconocidas
                    imageBoxFrameGrabber.Image = currentFrame;
                    label4.Text = names;
                    names = "";
                    //Limpiar la lista(vector) de nombres
                    NamePersons.Clear();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Donate.html");
        }

    }
}
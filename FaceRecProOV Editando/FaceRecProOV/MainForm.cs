﻿
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
        //Declararation of all variables, vectors and haarcascades
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
        SqlCommandBuilder dtBuild;
        
        SqlDataReader lector, lectorPrimero, lectorSelect;

        


        public FrmPrincipal()
        {
            DataTable dtDatos;
            DataSet ds;
            SqlDataAdapter datos, datosUpdate;
            SqlConnection conexion = new SqlConnection("Data Source=DESKTOP-47369CL\\SEMINARIO;Initial Catalog=master;Integrated Security=True");
            //conexion.Open();
            InitializeComponent();
            
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");

            try
            {
                //Cargado imagenes previas y nombres
                conexion.Open();
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

                //for (int i = 0; i < Labels.Length; i++)
                //{
                //    NumLabels = Convert.ToInt16(Labels[i]);
                //    ContTrain = NumLabels;
                //    string LoadFaces;


                //    string cadenaSelect = "SELECT * FROM socio WHERE socDNI = " + ContTrain;

                //    SqlCommand comando = new SqlCommand(cadenaSelect, conexion);
                //    lectorPrimero = comando.ExecuteReader();
                //    while (lectorPrimero.Read())
                //    {

                //        LoadFaces = lectorPrimero["socDni"].ToString() + ".bmp";
                //        trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                //        labels.Add(lectorPrimero["socDni"].ToString());
                //    }
                //    lectorPrimero.Close();
                //    conexion.Close();
                //}
                //for (int tf = 1; tf < NumLabels + 1; tf++)
                //{
                //    LoadFaces = tf + ".bmp";
                //    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                //    labels.Add(Labels[tf]);


                //}


            }
            catch (Exception e)
            {
                //Base de datos vacia
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Mostrar rostro agregado en escala de grises
                imageBox1.Image = TrainedFace;

                //Escribir el número de caras entrenadas para futuras cargas
                File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", txtDNI.Text.ToString() + "%" + Environment.NewLine);

                

                //Write the labels of triained faces in a file text for further load
              //  for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                //{
                    trainingImages.ToArray()[0].Save(Application.StartupPath + "/TrainedFaces/" + txtDNI.Text.ToString() + ".bmp");
                   // File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", "\n" + labels.ToArray()[0] + "%");
                DataTable dtDatos;
                DataSet ds;
                SqlDataAdapter datos, datosUpdate;
                SqlConnection conexion = new SqlConnection("Data Source=DESKTOP-47369CL\\SEMINARIO;Initial Catalog=master;Integrated Security=True");
                conexion.Open();
                SqlCommand cmd;

                string cadenaSelect = "SELECT socNombre FROM socio WHERE socDNI = '" + txtDNI.Text.ToString() + "'";
                SqlCommand comando = new SqlCommand(cadenaSelect, conexion);
                lector = comando.ExecuteReader();
                ////if (lector.Read())
                ////{
                //String vartempo = lector["socNombre"].ToString();
                //lbltemporal.Text = vartempo;
                //}

                //while (lector.Read())
                //{
                //if(String.IsNullOrEmpty((string) lector.ToString()))
                //if (lector[1].ToString() == "")
                if (lector.HasRows)
                        {
                    string query = "Select socID FROM socio WHERE socioDNI=" + txtDireccion.Text.ToString();
                    comando = new SqlCommand(query, conexion);
                    string temporal = comando.ExecuteScalar().ToString();

                    string cadenaUpdate = ("UPDATE socio SET socDNI = " + txtDNI.Text.ToString() + ", socNombre = '" + textBox1.Text.ToString() + "', socDireccion = '" + txtDireccion.Text.ToString() + "', socTelefono = " + txtTelefono.Text.ToString() + " WHERE socID = " + temporal);
                    MessageBox.Show(cadenaUpdate);
                    cmd = new SqlCommand(cadenaUpdate, conexion);
                    cmd.ExecuteNonQuery();
                        }
                else
                        {
                    string cadenaNuevo = ("INSERT INTO socio (socNombre, socDireccion, socDNI, socTelefono, socEstado) VALUES(" + textBox1.Text.ToString() + ",'" + txtDireccion.Text.ToString() + "','" + txtDNI.Text.ToString() + "'," + txtTelefono.Text.ToString() + "," + 1 + ")");
                    MessageBox.Show(cadenaNuevo);
                    cmd = new SqlCommand(cadenaNuevo, conexion);
                    cmd.ExecuteNonQuery();
                        }
                //}

                //Agregar control existencia ID
                //if(String.IsNullOrEmpty((string) lbltemporal.Contains("")) { 


                //else

                //string cadenaUpdate = ("UPDATE socio SET socDNI = '" + txtDNI + "', socNombre = '" + textBox1 + "', socDireccion = '" + txtDireccion + "', socTelefono = '" + txtTelefono + "' WHERE socID = '" + i + "'");
                //dtBuild = new MySqlCommandBuilder(datos);
                //cmd = new MySqlCommand(cadenaUpdate, conexion);
                //cmd.ExecuteNonQuery();
                lector.Close();
                    conexion.Close();
                //}

                //Mesaje confirmación guardado
                MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Convert it to Grayscale
                    gray = currentFrame.Convert<Gray, Byte>();

                    //Face Detector
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                  face,
                  1.2,
                  10,
                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                  new Size(20, 20));

                    //Action for each element detected
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        t = t + 1;
                        result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        //draw the face detected in the 0th (gray) channel with blue color
                        currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                        if (trainingImages.ToArray().Length != 0)
                        {
                            //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Eigen face recognizer
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);
                        name2 = name;

                            //Draw the label for each face detected and recognized
                        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                        }

                            NamePersons[t-1] = name;
                            NamePersons.Add("");


                        //Set the number of faces detected on the scene
                        label3.Text = facesDetected[0].Length.ToString();
                       
                        /*
                        //Set the region of interest on the faces
                        
                        gray.ROI = f.rect;
                        MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                           eye,
                           1.1,
                           10,
                           Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                           new Size(20, 20));
                        gray.ROI = Rectangle.Empty;

                        foreach (MCvAvgComp ey in eyesDetected[0])
                        {
                            Rectangle eyeRect = ey.rect;
                            eyeRect.Offset(f.rect.X, f.rect.Y);
                            currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                        }
                         */

                    }
                        t = 0;

                        //Names concatenation of persons recognized
                    for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                    {
                        names = names + NamePersons[nnn] + ", ";
                    }
                    //Show the faces procesed and recognized
                    imageBoxFrameGrabber.Image = currentFrame;
                    label4.Text = names;
                    names = "";
                    //Clear the list(vector) of names
                    NamePersons.Clear();

            
            DataTable dtDatos;
            DataSet ds;
            SqlDataAdapter datos, datosUpdate;
            string cadenaSelect = "SELECT socID, socDNI, socDireccion, socTelefono FROM socio WHERE socDNI LIKE '" + name2 + "'";
            
            SqlConnection conexion = new SqlConnection("Data Source=DESKTOP-47369CL\\SEMINARIO;Initial Catalog=master;Integrated Security=True");
            conexion.Open();
            SqlCommand comando = new SqlCommand(cadenaSelect, conexion);
            lectorSelect = comando.ExecuteReader();
            if (lectorSelect.Read())
            {
                //label4.Text = lector["socNombre"].ToString();
                lbldni.Text = lector["socDNI"].ToString();
                lblDireccion.Text = lector["socDireccion"].ToString();
                lblTelefono.Text = lector["socTelefono"].ToString();
            }
            //else
            //{
            //    lbldni.Text = "";
            //    lblDireccion.Text = "";
            //    lblTelefono.Text = "";
            //}
            lectorSelect.Close();
            conexion.Close();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Donate.html");
        }

    }
}
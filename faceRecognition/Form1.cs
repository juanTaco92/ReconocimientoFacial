using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faceRecognition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string ultimoId = "";
        int id = 0;
        private void button1_Click(object sender, EventArgs e)
        {

            //Process.Start("CMD.exe", "python c:/face-recognition/1dataset.py");

            DirectoryInfo dir = new DirectoryInfo(@"C:\Users\Juan Pablo\source\repos\faceRecognition\faceRecognition\bin\Debug\dataset");

            foreach (FileInfo file in dir.GetFiles())

            {
                ultimoId = file.Name;
            }
            try
            {
                id = Convert.ToInt32(ultimoId.Substring(8, 1)) + 1;
            }
            catch
            {
                id = 1;
            }

            //File.WriteAllText(@"C:\Users\IA\source\repos\faceRecognition\faceRecognition\bin\Debug\Usuarios.py", textBox1.Text);

            string usuarios = File.ReadAllText(@"C:\Users\Juan Pablo\source\repos\faceRecognition\faceRecognition\bin\Debug\Usuarios.txt");
            string[] lines = {
                "import cv2",
"import os",
" ",
"cam = cv2.VideoCapture(0)",
"cam.set(3, 640) ",
"cam.set(4, 480) ",
" ",
"face_detector = cv2.CascadeClassifier('haarcascade_frontalface_default.xml')",
" ",
"face_id = "+id,
" ",
" ",
"count = 0",
" ",
"while(True):",
" ",
"    ret, img = cam.read()",
"    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)",
"    faces = face_detector.detectMultiScale(gray, 1.3, 5)",
" ",
"    for (x,y,w,h) in faces:",
" ",
"        cv2.rectangle(img, (x,y), (x+w,y+h), (255,0,0), 2)     ",
"        count += 1",
" ",
"        cv2.imwrite('dataset/Usuario.' + str(face_id) + '.' + str(count) + '.jpg', gray[y:y+h,x:x+w])",
" ",
"        cv2.imshow('image', img)",
" ",
"    k = cv2.waitKey(100) & 0xff ",
"    if k == 27:",
"        break",
"    elif count >= 30: ",
"         break",
" ",
"cam.release()",
"cv2.destroyAllWindows()"
            };
            System.IO.File.WriteAllLines(@"C:\Users\Juan Pablo\source\repos\faceRecognition\faceRecognition\bin\Debug\1dataset.py", lines);

            StreamWriter WriteReportFile = File.AppendText(@"C:\Users\Juan Pablo\source\repos\faceRecognition\faceRecognition\bin\Debug\Usuarios.txt");
            WriteReportFile.WriteLine(",'" + textBox1.Text+"'");
            WriteReportFile.Close();

            string usuariosRec = File.ReadAllText(@"C:\Users\Juan Pablo\source\repos\faceRecognition\faceRecognition\bin\Debug\Usuarios.txt");

            string[] lines2 = {
            "import cv2",
"import numpy as np",
"import os ",
" ",
"recognizer = cv2.face.LBPHFaceRecognizer_create()",
"recognizer.read('trainer/trainer.yml')",
"cascadePath = "+"\""+"haarcascade_frontalface_default.xml"+"\"",
"faceCascade = cv2.CascadeClassifier(cascadePath);",
" ",
"font = cv2.FONT_HERSHEY_SIMPLEX",
" ",
"id = " +id,
" ",
"names = [''"+usuariosRec+"]",
" ",
"cam = cv2.VideoCapture(0)",
"cam.set(3, 640)",
"cam.set(4, 480)",
" ",
"minW = 0.1*cam.get(3)",
"minH = 0.1*cam.get(4)",
" ",
"while True:",
" ",
"    ret, img =cam.read()",
" ",
"    gray = cv2.cvtColor(img,cv2.COLOR_BGR2GRAY)",
" ",
"    faces = faceCascade.detectMultiScale( ",
"        gray,",
"        scaleFactor = 1.2,",
"        minNeighbors = 5,",
"        minSize = (int(minW), int(minH)),",
"       )",
" ",
"    for(x,y,w,h) in faces:",
" ",
"        cv2.rectangle(img, (x,y), (x+w,y+h), (0,255,0), 2)",
" ",
"        id, confidence = recognizer.predict(gray[y:y+h,x:x+w])",
" ",
"        if (confidence < 100):",
"            id = names[id]",
"            confidence = '  {0}%'.format(round(100 - confidence))",
"        else:",
"            id = 'unknown'",
"            confidence = '  {0}%'.format(round(100 - confidence))",
"         ",
"        cv2.putText(img, str(id), (x+5,y-5), font, 1, (255,255,255), 2)",
"        cv2.putText(img, str(confidence), (x+5,y+h-5), font, 1, (255,255,0), 1)  ",
"    ",
"    cv2.imshow('camera',img) ",
" ",
"    k = cv2.waitKey(10) & 0xff",
"    if k == 27:",
"        break",
" ",
"cam.release()",
"cv2.destroyAllWindows()",

        }
            ;
            System.IO.File.WriteAllLines(@"C:\Users\Juan Pablo\source\repos\faceRecognition\faceRecognition\bin\Debug\3recognition.py", lines2);

            sleepClick1();
        }

        private async Task sleepClick1()
        {
            updateStatusExecution("-> Iniciando captura de rostro. Mira la cámara y espera...");
            await Task.Delay(1000);
            executeCommand("python 1dataset.py");
            updateStatusExecution("   Captura OK!");
        }

        private void executeCommand(String commandR)
        {
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + commandR,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                //updateStatusExecution("*** TOMAR IMAGENES DEL USUARIO ***");
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    updateStatusExecution(line);
                }
            }
            catch (Exception e)
            {
                updateStatusExecution("***Error while executing '" + commandR + "'");
                updateStatusExecution("***Exception: '" + e.ToString());
                updateStatusExecution("***Stack Trace: '" + e.StackTrace.ToString());
            }
        }
        private void updateStatusExecution(String textR)
        {
            String currentDateTime = DateTime.Now.ToString(); ;
            listBox1.Items.Add(textR);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            sleepClick2();
        }
        private async Task sleepClick2()
        {
            updateStatusExecution("-> Procesamiento de rostros. Tomará unos segundos. Espere...");
            await Task.Delay(1000);
            executeCommand("python 2training.py");
            updateStatusExecution("   Procesamiento OK!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateStatusExecution("-> Reconocimiento de rostro. Mire la cámara y espera...");
            executeCommand("python 3recognition.py");
            updateStatusExecution("   Reconocimiento OK!");
        }
    }
}

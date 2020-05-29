using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MultilayerPerceptron
{
    // Статический класс для обучения
    static class Learning
    {
        // Обучение и сохранение нейросети в файл
        public static void LearnAndSave(Net net, List<List<double>> inputs, List<List<double>> outputs,
            double learningRate, int epoches, string filename)
        {
            net.Learn(inputs, outputs, learningRate, epoches);

            BinaryFormatter formatter = new BinaryFormatter();

            using(FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, net);
                fs.Close();
            }

            Console.WriteLine("Обучение успешно прошло");
        }

        /// <summary>
        /// Производит обучение по изображением в папке
        /// </summary>
        /// <param name="net"></param>
        /// <param name="path"></param>
        /// <param name="outputs"></param>
        public static void LearnByImages(Net net, string path, List<double> outputs, double learningRate,
            int epoches, string resultPath)
        {
            if (!Directory.Exists(path))
                throw new Exception("Данной папки не существует");

            int length = Directory.GetFiles(path).Length;

            if (length == 0)
                throw new Exception("Файлов в папке не существуют");

            if (net.OutputsCount != outputs.Count)
                throw new Exception("");

            List<string> filenames = Directory.GetFiles(path).ToList<string>();
            List<double> inputs = new List<double>();
            List<List<double>> inputsList = new List<List<double>>();
            List<List<double>> outputsList = new List<List<double>>();

            for (int i = 0; i < length; i++)
            {
                inputs = ImageConverter.Convert(filenames[i]);

                if (inputs.Count != net.InputsCount)
                    throw new Exception("Количество входных данных не совпадает с количеством" +
                        "нейронов на входном слое");

                inputsList.Add(inputs);
                outputsList.Add(outputs);
            }

            LearnAndSave(net, inputsList, outputsList, learningRate, epoches, resultPath);
        }
    }
}

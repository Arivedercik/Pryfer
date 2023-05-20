using System.IO;
namespace Pryfer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберете функция для работы с Кодом Прюфера:\n1. Построение кода Прюфера\n2. Восстановление дерева по коду");
                Console.Write("Ответ: ");
                int answer = Convert.ToInt32(Console.ReadLine());                
                switch (answer)
                {
                    case 1:
                        coding();
                        break;
                    case 2:
                        decoding();
                        break;
                }
                Console.WriteLine("Продолжить работу? (0 - нет | 1 - да)");
                if (Convert.ToInt32(Console.ReadLine()) == 0)
                {
                    break;
                }
            }
        }
        static void showList(List<int> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                Console.Write(list[i]+" ");
            }
            Console.WriteLine();
        }
        static void writeFileDecoding(List<int> finalResult, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                for(int i = 0; i < finalResult.Count; i+=2)
                {
                    sw.WriteLine(finalResult[i] + " " + finalResult[i + 1]);
                }          
            }
        }
        static void writeFileCoding(List<int> finalResult, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                for (int i = 0; i < finalResult.Count; i ++)
                {
                    sw.Write(finalResult[i] + " ");
                }
            }
        }
        static void decoding()
        {
            List<int> kodPryfera = new List<int>();
            string[] text;
            try
            {
                using (StreamReader reader = new StreamReader("dateForDecoding.csv"))
                {
                    text = reader.ReadToEnd().Split(';');
                }
                for (int i = 0; i < text.Length; i++)
                {
                    kodPryfera.Add(Convert.ToInt32(text[i]));
                }
            }
            catch
            {
                Console.WriteLine("Возникло исключение!");
            }             
            List<int> massivVershin = new List<int>();
            for(int i=1;i<kodPryfera.Count+3; i++)
            {
                massivVershin.Add(i);
            }
            Console.WriteLine("Код Прюфера");           
            showList(kodPryfera);
            Console.WriteLine("Доступные вершины");
            showList(massivVershin);
            List<int> finalResult = new List<int>();
            workWhithMas(kodPryfera,massivVershin,finalResult);
            Console.WriteLine("Ответ");
            for(int i = 0; i < finalResult.Count; i+=2)
            {
                Console.WriteLine(finalResult[i]+" " + finalResult[i+1]);
            }
            writeFileDecoding(finalResult, "answerFromDecoding.csv");
        } 
        static void workWhithMas(List<int> kodPryfera, List<int> massivVershin, List<int> finalResult)
        {
            while (true)
            {
                if (massivVershin.Count > 2)
                {
                    List<int> min = new List<int>();                    
                    for (int i = 0; i < massivVershin.Count; i++)
                    {
                        bool flag=true;
                        for (int j = 0; j < kodPryfera.Count; j++)
                        {
                            if (kodPryfera[j] == massivVershin[i])
                            {
                                flag = false;
                            }
                        }
                        if (flag == true)
                        {
                            min.Add(massivVershin[i]);
                        }
                    }
                    int minEl = min[0];                    
                    for (int j = 0; j < min.Count; j++)
                    {
                        if (min[j] < minEl)
                        {
                            minEl = min[j];                           
                        }
                    }
                    finalResult.Add(kodPryfera[0]);
                    finalResult.Add(minEl);                    
                    kodPryfera.RemoveRange(0, 1);
                    int indJ = massivVershin.IndexOf(minEl);
                    massivVershin.RemoveRange(indJ, 1);
                }
                else
                {
                    finalResult.Add(massivVershin[0]);
                    finalResult.Add(massivVershin[1]);
                    break;
                }
            }
        }
        static void coding()
        {          
            List<int> kodPryfera = new List<int>();            
            List<int> textt = new List<int>();
            using (StreamReader reader = new StreamReader("dateForCoding.csv"))
            {
                while (reader.EndOfStream != true)
                {
                    string[] text = reader.ReadLine().Split(';');
                    textt.Add(Convert.ToInt32(text[0]));
                    textt.Add(Convert.ToInt32(text[1]));
                }
            }
            int[,] rebra = new int[textt.Count / 2, 2];          
            for (int i = 0,j=0; i < rebra.GetLength(0); i++,j+=2)
            {
                rebra[i,0] = textt[j];
                rebra[i,1] = textt[j+1];
            }
            show(rebra);
            int yslovie = rebra.GetLength(0);
            show(rebra);
            while (yslovie>1)
            {
                checkListaDereva(rebra, kodPryfera);
                show(rebra);               
                yslovie--;
            }
            Console.Write("Код Прюфера: ");
            for (int i = 0; i < kodPryfera.Count; i++)
            {
                Console.Write(kodPryfera[i] + " ");
            }
            Console.WriteLine();
            writeFileCoding(kodPryfera, "answerFromCoding.csv");
        }
        static void show(int[,] rebra)
        {
            Console.WriteLine("Список допустимых ребер:");
            for(int i=0;i<rebra.GetLength(0);i++)
            {
                if (rebra[i, 0] != 0)
                {
                    Console.WriteLine(rebra[i, 0] + " " + rebra[i, 1]);
                }
            }
        }
        static void checkListaDereva(int[,] rebra, List<int> kodPryfera)
        {
            List<int> lista = new List<int>();
            for (int i = 0; i < rebra.GetLength(0); i++)
            {
                if (rebra[i, 0] != 0)
                {
                    bool flag = true;
                    for (int j = 0; j < rebra.GetLength(0); j++)
                    {
                       if (rebra[i, 1] == rebra[j, 0])
                        {
                            flag = false;
                        }                       
                    }
                    if (flag == true)
                    {
                        lista.Add(rebra[i, 0]);
                        lista.Add(rebra[i, 1]);
                    }
                }
            }
            int minList = lista[1];
            int ind = 0;
            for(int i = 1; i < lista.Count; i+=2)
            {
                if (minList > lista[i])
                {
                    minList = lista[i];
                    ind = i - 1;
                }
            }
            kodPryfera.Add(lista[ind]);
            for(int i=0;i< rebra.GetLength(0); i++)
            {
                if (rebra[i, 0] == lista[ind] && rebra[i,1] == lista[ind + 1])
                {
                    rebra[i, 0] = 0;
                    rebra[i, 1] = 0;
                    break;
                }
            }
        }
    }
}
using System;
using System.Linq;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks.Dataflow;

class Program
{
    static void Main(){
        string path = "Professional Life - Sheet1.tsv";
        string[,] matrix = tsvToMatrix(path);

        // Qualitative
        string[] instrumentsArray = getColumn(matrix,"Play some instruments? Which ones?");
        printAbsolute(instrumentsArray);
        

        // Quantitative discrete
        string[] ambitiousArray = getColumn(matrix,"Ambitious (0-5)");
        printPercentage(ambitiousArray);

        // Quantitative continuous
        string[] weightArray = getColumn(matrix, "weight");
        printAbsoluteIntervals(weightArray, 4);


        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();

    }

    public static string[,] tsvToMatrix(string path){
        string[] lines = File.ReadAllLines(path);

        int numRows = lines.Length;
        int numCols = lines[0].Split('\t').Length;

        string[,] matrix = new string[numRows, numCols];
        for(int i = 0; i < numRows; i++){
            string[] splits = lines[i].Split('\t');
            
            for(int j = 0; j < numCols; j++){
                matrix[i,j] = splits[j];
            }
        }
        return matrix;
    }

    public static string[] getColumn(string[,] matrix, string columname){ 
        string[] column = new string[matrix.GetLength(0)-1];
        
        for (int j = 0; j < matrix.GetLength(1); j++){
           if (matrix[0, j] == columname){
            for (int i = 1; i < matrix.GetLength(0); i++){
                column[i-1] = matrix[i, j];
            }
            break; 
            }
        }
        return column;
    }

    public static int getIndex(string[,] matrix, string variable){
        int index = 0;
        for (int j = 0; j < matrix.GetLength(0); j++) {

            if (matrix[0, j] == variable) {
                index = j;
                break;
            }
        }
        return index;
    }

    public static Dictionary<string, float> toDictionary(string[] array)
    {
        Dictionary<string, float> dictionary = new Dictionary<string, float>();
        foreach (string i in array)
        {
            string item = i.ToLower();
            if (dictionary.ContainsKey(item))
            {
                dictionary[item]++;
            }
            else
            {
                dictionary[item] = 1;
            }

        }
        return dictionary;
    }

    public static void printColumn(string[] column){
        foreach(var values in column){
            Console.WriteLine(values);
        }
    }

    public static void printMatrix(string[,] matrix){
        for (int row = 0; row < matrix.GetLength(0); row++){
            for (int col = 0; col < matrix.GetLength(1); col++){
                Console.Write(matrix[row, col] + " ");
            }
            Console.WriteLine();
        }
    }

    public static void printAbsolute(string[] array){
        Dictionary<string, float> dictionary = toDictionary(array);
        foreach (var kvp in dictionary){
            Console.WriteLine($"Key: {kvp.Key} ---> Absolute frequency: {kvp.Value}");
        }
    }

    public static void printAbsoluteIntervals(string[] array, int numIntervals)
    {
        var validNumbers = array.Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse);

        double max;
        double min;
        if (validNumbers.Any())
        {
            max = validNumbers.Max()+1;
            min = validNumbers.Min()-1;
        }
        else return;

        double intervalSize = (max - min) / numIntervals;

        Dictionary<string, float> dictionary = new Dictionary<string, float>();
        
        for (int i = 0; i < numIntervals; i++)
        {
            double start = min + i * intervalSize;
            double end = min + (i + 1) * intervalSize;
            string intervalKey = $"[{start};{end})";
            dictionary[intervalKey] = 0;
        }

        foreach (double element in validNumbers)
        {
            for (int i = 0; i < numIntervals; i++)
            {
                double start = min + i * intervalSize;
                double end = min + (i + 1) * intervalSize;

                if (element >= start && element < end)
                {
                    string intervalKey = $"[{start};{end})";
                    if (dictionary.ContainsKey(intervalKey))
                        dictionary[intervalKey]++;
                    else
                        dictionary[intervalKey] = 1;
                    break;
                    }
                }
              
            
        }

        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"Key: {kvp.Key} ---> Absolute frequency: {kvp.Value}");
        }
    }

    public static void printRelative(string[] array){
        Dictionary<string, float> dictionary = toDictionary(array);
        foreach (var kvp in dictionary){
             Console.WriteLine($"Key: {kvp.Key} ---> Relative frequency: {kvp.Value/array.Length}");
        }
    }

    public static void printPercentage(string[] array)
    {
        Dictionary<string, float> dictionary = toDictionary(array);
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"Key: {kvp.Key} ---> Percentage frequency: {(kvp.Value / array.Length) * 100}%");
        }
    }

}

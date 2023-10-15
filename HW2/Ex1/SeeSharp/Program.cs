using System;
using System.IO; 

class Program
{
    static void Main(){
        string path = "Professional Life - Sheet1.tsv";
        string[,] matrix = tsvToMatrix(path);

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);


        string[] ambitiousArray = getColumn(matrix,"Ambitious (0-5)");
        printColumn(ambitiousArray);

        Dictionary<string, int> ambitiousDictionary = new Dictionary<string, int>();
        foreach (string item in ambitiousArray){
            if (ambitiousDictionary.ContainsKey(item)){
                ambitiousDictionary[item]++;
            } else {
                ambitiousDictionary[item] = 1;
            }
        }

        // Print the frequency of each string
        foreach (var kvp in ambitiousDictionary)
        {
            Console.WriteLine($"String: {kvp.Key}, Frequency: {kvp.Value}");
        }

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

    public static void printColumn(string[] column){
        foreach(var values in column){
            Console.WriteLine(values);
        }
    }
}

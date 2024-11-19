using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
    //--------------------------------------------------------------------------------
    //- CONFIGURATION ----------------------------------------------------------------
    //--------------------------------------------------------------------------------
        // Chemin du dossier contenant les images
        string imagePath = @"D:\.....\.....";
        // Chemin du dossier contenant les fichiers .app et .apl
        string filePath = @"D:\......\.....\....";
    //--------------------------------------------------------------------------------

        List<string> allImages = ListingAllImages(imagePath);
        List<string> analyzedImages = ListingFile(filePath);

        // Trouver les images non utilisées
        List<string> unusedImages = new List<string>(allImages);
        unusedImages.RemoveAll(img => analyzedImages.Contains(img));

        Console.WriteLine("Images non utilisées:");
        foreach (string img in unusedImages)
        {
            Console.WriteLine(img);
        }
    }

    //--------------------------------------------------------------------------------
    //- ListingImage -----------------------------------------------------------------
    //- Liste l'ensemble des images du dossier et de ses sous-dossiers
    //--------------------------------------------------------------------------------
    static List<string> ListingAllImages(string filepath)
    {
        string[] imageExtensions = { "*.bmp", "*.jpg", "*.jpeg", "*.png", "*.ico" };
        List<string> imagesList = new List<string>();

        foreach (string extension in imageExtensions)
        {
            try
            {
                string[] Images = Directory.GetFiles(filepath, extension, SearchOption.AllDirectories);
                foreach (string Image in Images)
                {
                    imagesList.Add(Path.GetFileName(Image));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture des fichiers: {ex.Message}");
            }
        }

        return imagesList;
    }

    //--------------------------------------------------------------------------------
    //- ListingFile -----------------------------------------------------------------
    //- Liste l'ensemble des fichier .app et .apl du dossier et de ses sous-dossiers
    //--------------------------------------------------------------------------------
    static List<string> ListingFile(string filePath)
    {
        string[] fileExtensions = { "*.app", "*.apl" };
        List<string> imagesList = new List<string>();

        foreach (string extension in fileExtensions)
        {
            try
            {
                string[] files = Directory.GetFiles(filePath, extension, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    imagesList.AddRange(AnalyzeFile(file));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture des fichiers: {ex.Message}");
            }
        }

        // Supprimer les doublons
        imagesList = new HashSet<string>(imagesList).ToList();

        return imagesList;
    }

    //--------------------------------------------------------------------------------
    //- AnalyzeFile ------------------------------------------------------------------
    //- Analyse le contenu d'un fichier pour trouver les images BMP
    //--------------------------------------------------------------------------------
    static List<string> AnalyzeFile(string file)
    {
        List<string> imagesList = new List<string>();

        try
        {
            string fileContent = File.ReadAllText(file);
            Regex bmpRegex = new Regex(@"\b[\w\-_]+\d*\.bmp\b", RegexOptions.IgnoreCase);

            MatchCollection matches = bmpRegex.Matches(fileContent);
            foreach (Match match in matches)
            {
                imagesList.Add(match.Value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'analyse du fichier: {ex.Message}");
        }

        return imagesList;
    }
}
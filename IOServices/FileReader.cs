//p.FileReader=40
namespace Programa.IOServices
{
    using System;
    using System.IO;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Esta clase tiene la responsabilidad de leer un archivo de texto
    /// </summary>
    public class FileReader
    {
        //i.
        /// <summary>
        /// Initializes a new instance of the <see cref="FileReader"/> class.
        /// </summary>
        public FileReader()
        {
        }

        //i.
        /// Método que valida un filepath, y asigna a FileInfo un valor adecuado
        /// @param filepath Es el path del archivo recibido
        /// @param fInfo Es el objeto FileInfo al cu
        /// @return true si el archivo existe y filepath está en formato correcto, sino regresa false.
        public bool TryValidateFile(string filepath, out FileInfo fInfo)
        {
            Contract.Requires(!String.IsNullOrEmpty(filepath), "El filepath no debe ser null ni vacío");
            bool exists = false;
            fInfo = null;
            try
            {
                fInfo = new FileInfo(filepath);
                exists = fInfo.Exists;
            }
            catch (Exception)
            {
            }

            return exists;
        }

        //i.
        /// Método que lee un archivo de texto y regresa el contenido
        /// @param fInfo Es el FileInfo que apunta al archivo a leer
        /// @return el contenido del archivo
        public string ReadTextFile(FileInfo fInfo)
        {
            Contract.Requires(fInfo != null, "fInfo no debe ser nulo");
            Contract.Requires(fInfo.Exists, "El archivo indicado por fInfo debe existir.");
            Contract.Ensures(Contract.Result<string>() != null);

            string content = string.Empty;
            using (FileStream fStream = fInfo.OpenRead())
            {
                TextReader reader = new StreamReader(fStream);
                content = reader.ReadToEnd();
            }

            return content;
        }
    }
}
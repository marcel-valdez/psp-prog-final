//p.ContentParser=31
namespace Programa.IOServices
{
    using System.Diagnostics.Contracts;
    using Programa.Core;
    
    /// Clase encargada de parsear el contenido de un texto
    public class ContentParser
    {
        private static readonly ContentParser parser = new ContentParser();

        //i.
        /// Obtiene el ContentParser 
        public static ContentParser Parser
        {
            get
            {
                return parser;
            }
        }
        
        //i.
        /// Se esconde el constructor para tener solamente 1 instancia
        private ContentParser()
        {
        }

        //i.
        /// Parsea los tokens de numeros en el texto
        /// @param content es el contenido de texto a parsear
        /// @param separator es el separador de tokens en el contenido de texto
        /// @return La lista de los números en el texto
        public IList<double> ParseNumberTokens(string content, char separator)
        {
            Contract.Requires(content != null);
            IList<double> list = ACollectionFactory.Factory.CreateLinkedList<double>();
            string[] tokens = content.Split(separator);
            foreach(string token in tokens)
            {
                double number = double.Parse(token);
                list.Append(number);
            }
            
            return list;
        }
    }
}
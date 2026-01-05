using System.Collections.Generic;
using System.Linq;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;

public partial class Program
{
    // Klasa Tag; może zostać podłączony do Notatki albo Zadania
    public class Tag
    {

        // Nazwa danego tagu
        public string nazwa { get; }

        // Lista Notatek do których przypisany jest dany tag
        public List<Notatka> notatki { get; }

        // Lista Zadań do których przypisany jest dany tag
        //public List<Zadanie> zadania { get; }  // UWAGA: ZADANIA JESZCZE NIE ISTENIEJĄ


        // Konstruktor
        public Tag(string nazwa)
        {
            this.nazwa = nazwa;
        }


        // Nadpisanie ToString(); wypisuje nazwę tagu
        public override string ToString()
        {
            return nazwa;
        }
    }


    // Klasa MenedzerTagow - Singleton
    // Zarządza wszystkimi Tagami
    public class MenedzerTagow
    {
        // Statyczna instancja Singletona; getter korzysta z metody GetterInstancji()
        private static MenedzerTagow instancja
        {
            get { return GetterInstancji(); }
            set { }
        }
        // Lista wszystkich tagów
        List<Tag> tagi;


        // Prywatny konstruktor
        private MenedzerTagow()
        {
            tagi = new List<Tag>();
        }


        // Zwraca instancję Singletona
        public static MenedzerTagow GetterInstancji()
        {
            if (instancja == null)
                instancja = new MenedzerTagow();

            return instancja;
        }


        // Tworzy nowy tag i dodaje go do listy tagów
        public void UtworzTag(string nazwa)
        {
            Tag nowyTag = new Tag(nazwa);
            tagi.Add(nowyTag);
        }

        // Usuwa tag z listy tagów
        public void UsunTag(Tag tag)
        {
            if (tagi.Remove(tag) == true)
            {
                Console.WriteLine("Usunięto tag: " + tag.nazwa);
            }
            else
            {
                Console.WriteLine("Nie znaleziono tagu do usunięcia.");
            }
        }

        // Wypisuje wszystkie tagi w liście tagów
        public void WypiszTagi()
        {
            Console.WriteLine("Tagi: ");
            foreach (Tag tag in tagi)
            {
                Console.Write($"{tag}, ");
            }
        }

    }
}
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using static Program;

public partial class Program
{
    // Klasa Tag; może zostać podłączony do Notatki albo Zadania
    public class Tag
    {

        // Nazwa danego tagu
        public string nazwa { get; }

        // Lista Wpisów do których przypisany jest dany tag
        public List<Wpis> wpisy{ get; }


        // Konstruktor
        public Tag(string nazwa)
        {
            this.nazwa = nazwa;
            wpisy = new List<Wpis>();
        }


        // Dodaje podany Wpis do listy Wpisów w danym Tagu, jeśli dany Wpis jeszcze tam nie występuje
        // Zwraca true jeśli dodanie było sukcesem.
        public bool DodajWpis(Wpis wpis)
        {
            if (wpisy.Contains(wpis) == false)
            {
                wpisy.Add(wpis);
                return true;
            }
            return false;
        }

        // Usuwa podany Wpis z listy Wpisów w danym Tagu.
        // Zwraca true jeśli usunięcie było sukcesem.
        public bool UsunWpis(Wpis wpis)
        {
            return wpisy.Remove(wpis);
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
        private static MenedzerTagow instancja;
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


        // Tworzy nowy tag i dodaje go do listy tagów (jeśli dany tag jeszcze nie istnieje).
        // Zwraca dany Tag jeśli dodanie się powiodło, albo 'null' jeśli nie.
        public Tag UtworzTag(string nazwa)
        {
            // Jeśli takiego Tagu jeszcze nie ma w Liście tagów...
            if (ZwrocTag(nazwa) == null)
            {
                // Tworzymy taki Tag i go zwracamy
                Console.WriteLine();
                Tag nowyTag = new Tag(nazwa);
                tagi.Add(nowyTag);

                return nowyTag;
            }

            return null;
        }

        // Zwraca Tag z Listy Tagów o danej nazwie
        public Tag ZwrocTag(string nazwa)
        {
            return tagi.FirstOrDefault(n => n.nazwa == nazwa);
        }

        // Usuwa tag o podanej nazwie z listy tagów
        public void UsunTag(string nazwa)
        {
            // Pomocniczy wskaźnik na Tag
            Tag tag = ZwrocTag(nazwa);

            // Próba usunięcia Taga
            if (tagi.Remove(tag) == true)
            {
                Console.WriteLine("Usunięto tag: " + tag.nazwa);
            }
            else
            {
                Console.WriteLine("Nie znaleziono tagu do usunięcia.");
            }
        }


        // Dodaje podany Wpis do danego Tagu. Zwraca true jeśli dodanie zakończyło się sukcesem.
        public bool DodajWpisDoTagu(Tag tag, Wpis wpis)
        {
            return tag.DodajWpis(wpis);
        }

        // Usuwa podany Wpis z danego Tagu. Zwraca true jeśli usunięcie zakończyło się sukcesem.
        public bool UsunWpisZTagu(Tag tag, Wpis wpis)
        {
            return tag.UsunWpis(wpis);
        }


        // Wypisuje wszystkie tagi w liście tagów po przecinku
        public void Wypisztagi()
        {
            Console.WriteLine("tagi: ");
            foreach (Tag tag in tagi)
            {
                Console.Write($"{tag}, ");
            }
        }
    }
}
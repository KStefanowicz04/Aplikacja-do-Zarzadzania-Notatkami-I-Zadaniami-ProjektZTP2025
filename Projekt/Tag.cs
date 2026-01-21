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

        // Lista Notatek do których przypisany jest dany tag
        public List<Notatka> notatki { get; }

        // Lista Zadań do których przypisany jest dany tag
        public List<Zadanie> zadania { get; }


        // Konstruktor
        public Tag(string nazwa)
        {
            this.nazwa = nazwa;
        }


        // Dodaje podaną Notatkę do listy Notatek w danym Tagu, jeśli dana Notatka jeszcze tam nie występuje
        // Zwraca true jeśli dodanie było sukcesem.
        public bool DodajNotatke(Notatka notatka)
        {
            if (notatki.Contains(notatka) == false)
            {
                notatki.Add(notatka);
                return true;
            }
            return false;
        }

        // Usuwa podaną Notatkę z listy Notatek w danym Tagu.
        // Zwraca true jeśli usunięcie było sukcesem.
        public bool UsunNotatke(Notatka notatka)
        {
            return notatki.Remove(notatka);
        }


        // Dodaje podane Zadanie do listy Zadań w danym Tagu, jeśli dane Zadanie jeszcze tam nie występuje
        // Zwraca true jeśli dodanie było sukcesem.
        public bool DodajZadanie(Zadanie zadanie)
        {
            if (zadania.Contains(zadanie) == false)
            {
                zadania.Add(zadanie);
                return true;
            }
            return false;
        }

        // Usuwa podaną Notatkę z listy Notatek w danym Tagu.
        // Zwraca true jeśli usunięcie było sukcesem.
        public bool UsunZadanie(Zadanie zadanie)
        {
            return zadania.Remove(zadanie);
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


        // Dodaje podaną Notatkę do danego Tagu. Zwraca true jeśli dodanie zakończyło się sukcesem.
        public bool DodajNotatkeDoTagu(Tag tag, Notatka notatka)
        {
            return tag.DodajNotatke(notatka);
        }

        // Usuwa podaną Notatkę z danego Tagu. Zwraca true jeśli usunięcie zakończyło się sukcesem.
        public bool UsunNotatkeZTagu(Tag tag, Notatka notatka)
        {
            return tag.UsunNotatke(notatka);
        }

        // Dodaje podane Zadanie do danego Tagu. Zwraca true jeśli dodanie zakończyło się sukcesem.
        public bool DodajZadanieDoTagu(Tag tag, Zadanie zadanie)
        {
            return tag.DodajZadanie(zadanie);
        }

        // Usuwa podane Zadanie z danego Tagu. Zwraca true jeśli usunięcie zakończyło się sukcesem.
        public bool UsunZadanieZTagu(Tag tag, Zadanie zadanie)
        {
            return tag.UsunZadanie(zadanie);
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
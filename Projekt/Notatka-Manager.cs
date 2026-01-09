using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using static Program;

public partial class Program
{
    // Klasa Notatka dziedziczy po klasie abstrakcyjnej Wpis
    // Reprezentuje pojedyncz¹ notatkê w systemie.
    public class Notatka : Wpis
    {
        public bool Ulubiona { get; private set; }  // Okreœla, czy notatka jest oznaczona jako ulubiona

        // Konstruktor klasy Notatka
        public Notatka(string tytul, string tresc, List<Tag> tagi) : base(tytul, tresc)
        {
            // Manager notatek decyduje jaki numer ID zostanie przypisany do danej Notatki
            id = MenedzerNotatek.GetterInstancji().WybierzIDNotatki();

            if (tagi != null)
                this.tagi = tagi;
            else
                this.tagi = new List<Tag>();

            Ulubiona = false; // domyœlnie notatka nie jest ulubiona
        }

        // Ustawia lub usuwa status ulubionej notatki
        public void UstawUlubione(bool ustawienie)
        {
            Ulubiona = ustawienie;
        }

        // Nadpisanie metody abstrakcyjnej WypiszInformacje() z klasy Wpis
        // Zwraca wszystkie informacje o notatce w formie stringa (oprócz tagów, do tego s³u¿y DekoratorTagowy!)
        public override string WypiszInformacje()
        {
            return $"[NOTATKA] ID: {id} | Tytu³: {tytul} | Treœæ: {tresc} | Ulubiona: {Ulubiona}";
        }

        // Nadpisanie ToString() dla wygodnego wypisywania notatki
        public override string ToString()
        {
            return WypiszInformacje();
        }
    }


    // Klasa FabrykaNotatek - dziedziczy po abstrakcyjnej klasie FabrykaWpisow
    // Wzorzec Factory Method - odpowiada za tworzenie instancji Notatek
    public class FabrykaNotatek : FabrykaWpisow
    {
        // Nadpisanie metody abstrakcyjnej UtworzWpis
        // Zwraca now¹ instancjê klasy Notatka. Przyjmuje Listê stringów 'tagi', która jest przekazywana do MenedzeraTagów
        public override Wpis UtworzWpis(string tytul, string tresc, List<string> nazwyTagow)
        {
            List<Tag> tagi = null;
            if (nazwyTagow != null)
            {
                // Mened¿erTagów zajmuje siê znalezieniem i zwróceniem odpowiednich Tagów.
                tagi = new List<Tag>();  // Rzeczywista Lista Tagów, przekazywana do Notatki
                foreach (string nazwaTagu in nazwyTagow)
                {
                    // Pytamy Mened¿eraTagów o zwrócenie wskaŸnika na dany Tag.
                    // Jeœli zwróci 'null', dany Tag nie istnieje, wiêc go nie dodajemy
                    Tag tag = MenedzerTagow.GetterInstancji().ZwrocTag(nazwaTagu);
                    if (tag != null)
                    {
                        tagi.Add(tag);
                    }
                }
            }

            // W³aœciwe utworzenie Notatki
            return new Notatka(tytul, tresc, tagi);
        }
    }


    // Klasa MenedzerNotatek - Singleton
    // Zarz¹dza wszystkimi notatkami w systemie
    public class MenedzerNotatek
    {

        private static MenedzerNotatek instancja;  // Statyczna instancja Singletona        
        private FabrykaNotatek fabryka;  // Fabryka do tworzenia notatek
        private List<Notatka> notatki;  // Lista wszystkich notatek w systemie
        private HashSet<int> IDNotatek = new();  // HashSet unikalnych ID Notatek. ID siê nie powtarzaj¹.

        // Prywatny konstruktor
        private MenedzerNotatek()
        {
            fabryka = new FabrykaNotatek();
            notatki = new List<Notatka>();
        }
        // Zwraca instancjê Singletona
        public static MenedzerNotatek GetterInstancji()
        {
            if (instancja == null)
                instancja = new MenedzerNotatek();

            return instancja;
        }

        // Tworzy now¹ notatkê przez fabrykê i dodaje j¹ do listy
        // Domyœlnie Lista tagów jest 'null'; to oznacza, ¿e Notatka nie musi mieæ ¿adnych tagów.
        public void UtworzNotatkePrzezFabryke(string tytul, string tresc, List<string> tagi = null)
        {
            // Lista stringów 'tagi' podana do metody zawiera nazwy Tagów, które powinny zostaæ przypisane do
            // nowo utworzonej notatki. Te tagi nie koniecznie istniej¹, wiêc zajmie siê tym Fabryka.

            Notatka notatka = (Notatka)fabryka.UtworzWpis(tytul, tresc, tagi);
            notatki.Add(notatka);
        }

        // Metoda wybieraj¹ca unikalne ID dla notatki, zwraca to ID.
        public int WybierzIDNotatki()
        {
            int id = 0;  // Nowe ID zaczyna odliczanie od 0
            // Pêtla od 0 w górê, przez HashSet ID, a¿ znajdziemy nieu¿yte ID.
            while(IDNotatek.Contains(id))
            {
                id++;
            }

            // Dane ID nie jest u¿ywane, dodajemy je do HashSetu.
            IDNotatek.Add(id);
            return id;
        }

        // Usuwa notatkê z listy i wypisuje jej zawartoœæ
        public void UsunNotatke(Notatka notatka)
        {
            if (notatki.Remove(notatka))
            {
                Console.WriteLine("Usuniêto notatkê:");
                WypiszNotatke(notatka);
            }
            else
            {
                Console.WriteLine("Nie znaleziono notatki do usuniêcia.");
            }
        }

        // Wywo³uje WypiszInformacje() na podanym obiekcie Notatka
        public void WypiszNotatke(Notatka notatka)
        {
            if (notatka != null)
            {
                Console.WriteLine(notatka.WypiszInformacje());
            }
        }

        // Wyszukiwanie notatki po ID
        public Notatka WyszukajNotatke(int id)
        {
            foreach (var n in notatki)
            {
                if (n.id == id)
                    return n;
            }
            return null;
        }

        // Wyszukiwanie notatek po s³owach kluczowych w tytule lub treœci
        public List<Notatka> WyszukajNotatke(List<string> zawiera)
        {
            List<Notatka> wynik = new List<Notatka>();

            foreach (var n in notatki)
            {
                foreach (var s in zawiera)
                {
                    if (n.tytul.Contains(s) || n.tresc.Contains(s))
                    {
                        wynik.Add(n);
                        break; // nie dodajemy tej samej notatki kilka razy
                    }
                }
            }

            return wynik;
        }
        // Wyszukiwanie notatek po konkretnym tagu
        public List<Notatka> WyszukajNotatke(Tag danyTag)
        {
            List<Notatka> wynik = new List<Notatka>();

            foreach (var notatka in notatki)
            {
                foreach (Tag tag in notatka.tagi)
                {
                    if (tag.nazwa == danyTag.nazwa)
                    {
                        wynik.Add(notatka);
                        break;
                    }
                }
            }

            return wynik;
        }

        // Wypisuje wszystkie Notatki
        public void WypiszNotatki()
        {
            // Dzia³a tylko gdy Lista Notatek nie jest pusta
            if (notatki.Count > 0)
            {
                foreach (Notatka n in notatki)
                {
                    WypiszNotatke(n);
                }
            }
        }

    }
}
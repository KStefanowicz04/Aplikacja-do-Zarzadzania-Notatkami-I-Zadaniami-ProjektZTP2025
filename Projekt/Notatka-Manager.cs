using System.Collections.Generic;
using System.Linq;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;

public partial class Program
{
    // Klasa Notatka dziedziczy po klasie abstrakcyjnej Wpis
    // Reprezentuje pojedyncz¹ notatkê w systemie.
    public class Notatka : Wpis
    {

        public bool Ulubiona { get; private set; }  // Okreœla, czy notatka jest oznaczona jako ulubiona
        public List<Tag> Tagi { get; set; }  // Lista tagów przypisanych do notatki

        // Konstruktor klasy Notatka
        public Notatka(string tytul, string tresc, List<Tag> tagi) : base(tytul, tresc)
        {
            if (tagi != null)
                Tagi = tagi;
            else
                Tagi = new List<Tag>();

            Ulubiona = false; // domyœlnie notatka nie jest ulubiona
        }

        // Ustawia lub usuwa status ulubionej notatki
        public void UstawUlubione(bool ustawienie)
        {
            Ulubiona = ustawienie;
        }

        // Nadpisanie metody abstrakcyjnej WypiszInformacje() z klasy Wpis
        // Zwraca wszystkie informacje o notatce w formie stringa
        public override string WypiszInformacje()
        {
            string tagiStr;

            if (Tagi.Count > 0)
                tagiStr = string.Join(", ", Tagi.Select(t => t.nazwa));
            else
                tagiStr = "Brak tagów";

            return $"ID: {id}, Tytu³: {tytul}, Treœæ: {tresc}, Ulubiona: {Ulubiona}, Tagi: {tagiStr}";
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
        // Zwraca now¹ instancjê klasy Notatka
        public override Wpis UtworzWpis(string tytul, string tresc, List<Tag> tagi)
        {
            return new Notatka(tytul, tresc, tagi);
        }
    }


    // Klasa MenedzerNotatek - Singleton
    // Zarz¹dza wszystkimi notatkami w systemie
    public class MenedzerNotatek
    {

        private static MenedzerNotatek instancja;// Statyczna instancja Singletona        
        private FabrykaNotatek fabryka;// Fabryka do tworzenia notatek
        private List<Notatka> notatki;// Lista wszystkich notatek w systemie

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
        public void UtworzNotatkePrzezFabryke(string tytul, string tresc, List<Tag> tagi)
        {
            Notatka nowa = (Notatka)fabryka.UtworzWpis(tytul, tresc, tagi);
            notatki.Add(nowa);
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
        // Wywo³uje ToString() na obiekcie Notatka
        public void WypiszNotatke(Notatka notatka)
        {
            Console.WriteLine(notatka.ToString());
        }

        // Wyszukiwanie notatki po ID
        public Notatka WyszukajNotatki(int id)
        {
            foreach (var n in notatki)
            {
                if (n.id == id)
                    return n;
            }
            return null;
        }
        // Wyszukiwanie notatek po s³owach kluczowych w tytule lub treœci
        public List<Notatka> WyszukajNotatki(List<string> zawiera)
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
        public List<Notatka> WyszukajNotatki(Tag tag)
        {
            List<Notatka> wynik = new List<Notatka>();

            foreach (var n in notatki)
            {
                foreach (var t in n.Tagi)
                {
                    if (t.nazwa == tag.nazwa)
                    {
                        wynik.Add(n);
                        break;
                    }
                }
            }

            return wynik;
        }
        public void WypiszWszystkieNotatki()
        {
            foreach (var n in notatki)
                WypiszNotatke(n);
        }

    }
}
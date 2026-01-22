using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using Projekt;
using static Program;
namespace Projekt;
public partial class Program
{
    // Interfejs stan�w Zadania (Wzorzec State)
    public interface IStanZadania
    {
        // Metody wykorzysywane przez ka�dy Stan
        public void wykonane(Zadanie zadanie);
        public void aktywne(Zadanie zadanie);
        public void zalegle(Zadanie zadanie);
    }


    public enum Priorytet
    {
        Niski,
        Sredni,
        Wysoki
    }

    // Klasa Zadanie, pochodna od Wpis
    public class Zadanie : Wpis
    {
        private IStanZadania stan;  // Obecny stan zadania (wykonane/aktywne/zaleg�e)
        private Priorytet priorytet;  // Obecny priorytet zadania
        private DateTime termin;  // Termin wykonania zadania


        // Konstruktor
        public Zadanie(string tytul, string tresc, IStanZadania stan, Priorytet priorytet, DateTime termin, List<Tag> tagi) : base(tytul, tresc)
        {
            // Manager zada� decyduje jaki numer ID zostanie przypisany do danego Zadania
            id = MenedzerZadan.GetterInstancji().WybierzIDZadania();

            // Domy�lnie stan jest Aktywny - mo�e nale�y to usun�� z parametr�w Konstruktora?
            if (stan != null) this.stan = stan;
            else this.stan = new StanAktywne();

            this.stan = stan;
            this.priorytet = priorytet;
            this.termin = termin;

            if (tagi != null)
                this.tagi = tagi;
            else
                this.tagi = new List<Tag>();
        }


        // Metody

        public Priorytet Priorytet
        {
            get { return priorytet; }
        }
        // Zwraca termin wykonania zadania.
        // pole "termin" jest prywatne i nie mo�e by� bezpo�rednio odczytywane poza klas� Zadanie (np. przez Mened�eraZada�).
        public DateTime ZwrocTermin()
        {
            return termin;
        }
        // Edytowanie zadania
        public override void Edytuj(string tytul, string tresc, Priorytet priorytet, DateTime termin)
        {
            base.Edytuj(tytul, tresc);  // Wezwanie bazowej metody Edytuj w klasie Wpis
            this.priorytet = priorytet;
            this.termin = termin;
        }

        // Zmienia stan Zadania na wykonane
        public void OznaczJakoWykonane()
        {
            //stan = IStanZadania.wykonane;
            //ZmienStan();
        }


        // Zwr�cenie nazwy obecnego stanu Zadania w formie string
        public string ZwrocStan()
        {
            return stan.GetType().Name;
        }

        // Zmiana obecnego stanu zadania
        public void ZmienStan(IStanZadania stan)
        {
            this.stan = stan;
        }

        // Zmienia obecny stan za pomoc� IStanZadania na Wykonane
        public void StanWykonane()
        {
            stan.wykonane(this);
        }

        // Zmienia obecny stan za pomoc� IStanZadania na Aktywne
        public void StanAktywne()
        {
            stan.aktywne(this);
        }

        // Zmienia obecny stan za pomoc� IStanZadania na Zaleg�e
        public void StanZalegle()
        {
            stan.zalegle(this);
        }

        // Zwraca true/false zale�nie od obecnego stanu zadania
        public bool SprawdzCzyZalegle()
        {
            // Por�wnanie obecnego typu zmiennej stan z klas� StanWykonane;
            // sprawdzamy czy obecny stan jest stanem Wykonane
            if (stan.GetType() == typeof(StanWykonane))
                return false;

            // Por�wnuje czas obecny z wyznaczonym terminem zadania
            return DateTime.Now > termin;
        }

        // Wypisuje podstawowe informacje o zadaniu
        public override string WypiszInformacje()
        {
            return $"[ZADANIE] ID: {id} | Tytu�: {tytul} | Tre��: {tresc} | Priorytet: {priorytet} | Termin: {termin:d}";
        }

        // Nadpisanie ToString() dla wygodnego wypisywania notatki
        public override string ToString()
        {
            return WypiszInformacje();
        }
    }

    // Konkretne stany Zadania
    //
    // Zadanie wykonane
    public class StanWykonane : IStanZadania
    {
        // Ponowienie tego stanu
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie ju� zosta�o wykonane");
        }

        // Zmiana stanu z Wykonane na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z wykonane na aktywne");
        }

        // Zmiana stanu z Wykonane na Zaleg�e
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z wykonane na zaleg�e");
        }
    }

    // Zadanie aktywne
    public class StanAktywne : IStanZadania
    {
        // Zmiana stanu z Aktywne na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z aktywne na wykonane");
        }

        // Ponowienie tego stanu
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie pozostaje aktywne");
        }

        // Zmiana stanu z Aktywne na Zaleg�e
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z aktywne na zaleg�e");
        }
    }

    // Zadanie zaleg�e
    public class StanZalegle : IStanZadania
    {
        // Zmiana stanu z Zaleg�e na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z zaleg�e na wykonane");
        }

        // Zmiana stanu z Zaleg�e na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z zaleg�e na aktywne");
        }

        // Ponowienie tego stanu
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie pozostaje zaleg�e");
        }
    }




    // Klasa Mened�er Zada� (Wzorzec Singleton)
    public class MenedzerZadan
    {
       
        // Pola
        private static MenedzerZadan instancja;  // Singleton; instancja Fabryki Zada�
        private FabrykaZadan fabryka;  // Wska�nik na Fabryk� zada�
        private List<Zadanie> zadania;  // Lista wszystkich Zada� w programie
        public List<Zadanie> Zadania  // Publiczny getter
        {
            get { return zadania; }
        }
        private HashSet<int> IDZadan = new();  // HashSet unikalnych ID Zada�. ID si� nie powtarzaj�.

        private ZadanieBuilder _builder;

        // Prywatny Konstruktor
        private MenedzerZadan(ZadanieBuilder builder)
        {
            _builder = builder;
            fabryka = new FabrykaZadan();
            zadania = new List<Zadanie>();
        }


        // Metody
        public static MenedzerZadan GetterInstancji()
        {
            if (instancja == null)
            {
                instancja = new MenedzerZadan();
            }
            return instancja;
        }


        // Utworzenie nowego Zadania poprzez Fabryk� 
        public void UtworzZadaniePrzezFabryke(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<string> tagi = null)
        {
            // Lista string�w 'tagi' podana do metody zawiera nazwy Tag�w, kt�re powinny zosta� przypisane do
            // nowo utworzonej notatki. Te tagi nie koniecznie istniej�, wi�c zajmie si� tym Fabryka.

            // Utworzenie Zadania poprzez Fabryk�
            Zadanie zadanie = (Zadanie)fabryka.UtworzWpis(tytul, tresc, priorytet, termin, tagi);
            // Dodajemy nowe Zadanie do listy mened�era
            zadania.Add(zadanie);
        }

        // Metoda wybieraj�ca unikalne ID dla Zadania, zwraca to ID.
        public int WybierzIDZadania()
        {
            int id = 0;  // Nowe ID zaczyna odliczanie od 0
            // P�tla od 0 w g�r�, przez HashSet ID, a� znajdziemy nieu�yte ID.
            while (IDZadan.Contains(id))
            {
                id++;
            }

            // Dane ID nie jest u�ywane, dodajemy je do HashSetu.
            IDZadan.Add(id);
            return id;
        }

        // Usuwa Zadanie z listy i wypisuje jego zawarto��
        public void UsunZadanie(Zadanie zadanie)
        {
            if (zadania.Remove(zadanie))
            {
                // Usuni�cie danej Notatki z Listy Notatek Tag�w przypisanych do danej Notatki
                foreach (Tag tag in zadanie.tagi)
                {
                    tag.UsunWpis(zadanie);
                }

                Console.WriteLine("Usuni�to zadanie:");
                WypiszZadanie(zadanie);
            }
            else
            {
                Console.WriteLine("Nie znaleziono zadania do usuni�cia.");
            }
        }


        // Dodaje podany Tag do danego Zadania. Zwraca true je�li dodanie zako�czy�o si� sukcesem.
        public bool DodajTagDoZadania(Zadanie zadanie, Tag tag)
        {
            return zadanie.DodajTag(tag);
        }

        // Usuwa podany Tag z danego Zadania. Zwraca true je�li usuni�cie zako�czy�o si� sukcesem.
        public bool UsunTagZZadania(Zadanie zadanie, Tag tag)
        {
            return zadanie.UsunTag(tag);
        }


        // Wypisanie zawarto�ci danego zadania
        public void WypiszZadanie(Zadanie zadanie)
        {
            if (zadanie != null)
            {
                Console.WriteLine(zadanie.WypiszInformacje());
            }
        }

        // Wyszukiwanie Zadania po ID
        public Zadanie WyszukajZadanie(int id)
        {
            foreach (Zadanie z in zadania)
            {
                if (z.id == id)
                {
                    return z;
                }
            }
            return null;
        }

        // Wyszukuje zadanie po podanej frazie
        public List<Zadanie> SzukajZadan(string fraza)
        {
            List<Zadanie> wynik = new List<Zadanie>();

            foreach (var z in zadania)
            {
                // zak�adamy, �e Wpis ma dost�p do tytulu (getter)
                if (z.WypiszInformacje().Contains(fraza))
                {
                    wynik.Add(z);
                }
            }

            return wynik;
        }
        public List<Zadanie> SzukajPoTerminach(DateTime od, DateTime doDaty)
        {
            List<Zadanie> wynik = new List<Zadanie>();

            foreach (Zadanie z in zadania)
            {
                if (z != null)
                {
                    if (z.SprawdzCzyZalegle() == false &&
                        z.ZwrocTermin() >= od &&
                        z.ZwrocTermin() <= doDaty)
                    {
                        wynik.Add(z);
                    }
                }
            }

            return wynik;
        }
        public void SortujZadaniaPoTerminach(List<Zadanie> lista)
        {
            lista.Sort(delegate (Zadanie a, Zadanie b)
            {
                return a.ZwrocTermin().CompareTo(b.ZwrocTermin());
            });
        }

        // Sortowanie listy zada� po priorytecie malej�co (wysoki priorytet pierwszy)
        public void SortujZadaniaPoPriorytecie(List<Zadanie> lista)
        {
            lista.Sort(delegate (Zadanie a, Zadanie b)
            {
                return b.Priorytet.CompareTo(a.Priorytet);
            });
        }



        // Oznacza Zadania z podanej Listy Zada� jako wykonane
        public void OznaczZadaniaJakowykonane(List<Zadanie> lista)
        {
            foreach (var z in lista)
            {
                z.OznaczJakoWykonane();
            }
        }

        // Zwraca List� Zada� zaleg�ych
        public List<Zadanie> WybierzZalegle()
        {
            // Lista pomocnicza
            List<Zadanie> zalegle = new List<Zadanie>();

            // Dodanie zaleg�ych Zada� do Listy pomocniczej
            foreach (var z in zadania)
            {
                if (z.SprawdzCzyZalegle())
                    zalegle.Add(z);
            }

            return zalegle;
        }

        // Wypisuje wszystkie Zadania
        public void WypiszZadania()
        {
            // Dzia�� tylko gdy Lista zada� nie jest pusta
            if (zadania.Count > 0)
            {
                foreach (Zadanie z in zadania)
                {
                    WypiszZadanie(z);
                }
            }
        }
    }


    // Fabryka Zada�
    public class FabrykaZadan : FabrykaWpisow
    {
        // Konstruktor
        public FabrykaZadan() : base() { }


        // Nadpisanie metody fabrykuj�cej wpis
        public override Wpis UtworzWpis(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<string> nazwyTagow)
        {
            List<Tag> tagi = null;
            if (nazwyTagow != null)
            {
                // Mened�erTag�w zajmuje si� znalezieniem i zwr�ceniem odpowiednich Tag�w.
                tagi = new List<Tag>();  // Rzeczywista Lista Tag�w, przekazywana do Zadania
                foreach (string nazwaTagu in nazwyTagow)
                {
                    // Pytamy Mened�eraTag�w o zwr�cenie wska�nika na dany Tag.
                    // Je�li zwr�ci 'null', dany Tag nie istnieje, wi�c go nie dodajemy
                    Tag tag = MenedzerTagow.GetterInstancji().ZwrocTag(nazwaTagu);
                    if (tag != null)
                    {
                        tagi.Add(tag);
                    }
                }
            }

            IStanZadania domyslnyStan = new StanAktywne();


            // W�a�ciwe utworzenie Zadania na podstawie powy�szych danych
            Zadanie zadanie = new Zadanie(tytul, tresc, domyslnyStan, priorytet, termin, tagi);
            // Dodanie Zadania do listy Zada� wszystkich wybranych Tag�w
            if (tagi != null)
            {
                foreach (Tag tag in tagi)
                {
                    tag.DodajWpis(zadanie);
                }
            }


            // Utworzenie i zwr�cenie nowego Zadania 
            return zadanie;
        }
    }
}

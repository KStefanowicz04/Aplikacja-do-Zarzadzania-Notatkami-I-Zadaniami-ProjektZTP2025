using System.Collections.Generic;
using System.Linq;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;

public partial class Program
{
    // Interfejs stanów Zadania (Wzorzec State)
    public interface IStanZadania
    {
        // Metody wykorzysywane przez ka¿dy Stan
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
        private IStanZadania stan;  // Obecny stan zadania (wykonane/aktywne/zaleg³e)
        private Priorytet priorytet;  // Obecny priorytet zadania
        private DateTime termin;  // Termin wykonania zadania


        // Konstruktor
        public Zadanie(string tytul, string tresc, IStanZadania stan, Priorytet priorytet, DateTime termin) : base(tytul, tresc)
        {
            this.stan = stan;
            this.priorytet = priorytet;
            this.termin = termin;
        }


        // Metody
        //
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

        // Zmiana obecnego stanu zadania
        public void ZmienStan(IStanZadania stan)
        {
            this.stan = stan;
        }

        // Zmienia obecny stan za pomoc¹ IStanZadania na Wykonane
        public void StanWykonane()
        {
            stan.wykonane(this);
        }

        // Zmienia obecny stan za pomoc¹ IStanZadania na Aktywne
        public void StanAktywne()
        {
            stan.aktywne(this);
        }

        // Zmienia obecny stan za pomoc¹ IStanZadania na Zaleg³e
        public void StanZalegle()
        {
            stan.zalegle(this);
        }

        // Zwraca true/false zale¿nie od obecnego stanu zadania
        public bool SprawdzCzyZalegle()
        {
            // Porównanie obecnego typu zmiennej stan z klas¹ StanWykonane;
            // sprawdzamy czy obecny stan jest stanem Wykonane
            if (stan.GetType() == typeof(StanWykonane))
                return false;

            // Porównuje czas obecny z wyznaczonym terminem zadania
            return DateTime.Now > termin;
        }

        // Wypisuje podstawowe informacje o zadaniu
        public override string WypiszInformacje()
        {
            return $"[ZADANIE] {tytul} | {priorytet} | Termin: {termin:d} | Stan: {stan.GetType().Name}";
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
            Console.WriteLine("Zadanie ju¿ zosta³o wykonane");
        }

        // Zmiana stanu z Wykonane na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z wykonane na aktywne");
        }

        // Zmiana stanu z Wykonane na Zaleg³e
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z wykonane na zaleg³e");
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

        // Zmiana stanu z Aktywne na Zaleg³e
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z aktywne na zaleg³e");
        }
    }

    // Zadanie zaleg³e
    public class StanZalegle : IStanZadania
    {
        // Zmiana stanu z Zaleg³e na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z zaleg³e na wykonane");
        }

        // Zmiana stanu z Zaleg³e na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z zaleg³e na aktywne");
        }

        // Ponowienie tego stanu
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie pozostaje zaleg³e");
        }
    }




    // Klasa Mened¿er Zadañ (Wzorzec Singleton)
    public class MenedzerZadan
    {
        // Pola
        private static MenedzerZadan instancja;  // Singleton; instancja Fabryki Zadañ
        private FabrykaZadan fabryka;  // WskaŸnik na Fabrykê zadañ
        private List<Zadanie> zadania;  // Lista wszystkich Zadañ w programie
       

        // Prywatny Konstruktor
        private MenedzerZadan()
        {
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

        // Utworzenie nowego Zadania poprzez Fabrykê 
        public void UtworzZadaniePrzezFabryke(string tytul,string tresc,Priorytet priorytet,DateTime termin,List<Tag> tagi)
        {
            // Wywo³ujemy fabrykê, tworzymy Zadanie
            var zadanie = (Zadanie)fabryka.UtworzWpis(tytul, tresc, priorytet, termin, tagi);

            // Dodajemy do listy mened¿era
            zadania.Add(zadanie);
        }

        // Usuniêcie danego Zadania z 
        public void UsunZadanie(Zadanie zadanie)
        {
            zadania.Remove(zadanie);
        }

        // Wypisanie zawartoœci wszystkich zadañ
        public void WypiszZadanie()
        {
            foreach (var z in zadania)
            {
                Console.WriteLine(z.WypiszInformacje());
            }
        }

        // Wyszukuje zadanie po podanej frazie
        public List<Zadanie> SzukajZadan(string fraza)
        {
            List<Zadanie> wynik = new List<Zadanie>();

            foreach (var z in zadania)
            {
                // zak³adamy, ¿e Wpis ma dostêp do tytulu (getter)
                if (z.WypiszInformacje().Contains(fraza))
                {
                    wynik.Add(z);
                }
            }

            return wynik;
        }

        // Oznacza Zadania z podanej Listy Zadañ jako wykonane
        public void OznaczZadaniaJakowykonane(List<Zadanie> lista)
        {
            foreach (var z in lista)
            {
                z.OznaczJakoWykonane();
            }
        }

        // Zwraca Listê Zadañ zaleg³ych
        public List<Zadanie> WybierzZalegle()
        {
            // Lista pomocnicza
            List<Zadanie> zalegle = new List<Zadanie>();

            // Dodanie zaleg³ych Zadañ do Listy pomocniczej
            foreach (var z in zadania)
            {
                if (z.SprawdzCzyZalegle())
                    zalegle.Add(z);
            }

            return zalegle;
        }
    }


    // Fabryka Zadañ
    public class FabrykaZadan : FabrykaWpisow
    {
        // Konstruktor
        public FabrykaZadan() : base()
        {

        }


        // Nadpisanie metody fabrykuj¹cej wpis
        public override Wpis UtworzWpis(string tytul, string tresc, List<Tag> tagi)
        {
            // Domyœlne wartoœci Zadania, mo¿na póŸniej rozszerzyæ parametry
            IStanZadania domyslnyStan = new StanAktywne();
            Priorytet domyslnyPriorytet = Priorytet.Niski;
            DateTime domyslnyTermin = DateTime.Now.AddDays(7); // Domyœlnie tydzieñ od dzisiaj

            return new Zadanie(tytul, tresc, domyslnyStan, domyslnyPriorytet, domyslnyTermin);
        }

       
    }
} 
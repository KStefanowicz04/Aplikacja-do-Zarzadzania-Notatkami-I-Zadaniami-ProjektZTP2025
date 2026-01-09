using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Program;

public partial class Program
{

    // Abstrakcyjny DekoratorWpisów dla Wpisów (Notatek i Zadań) z którego dziedziczą pozostałe Dekoratory.
    public abstract class DekoratorWpisow : IWpis
    {
        // Wskaźnik na Wpis, który dekoruje dany Dekorator
        protected Wpis wpis;


        // Konstruktor; jedynie zapisuje wskaźnik na podany Wpis.
        public DekoratorWpisow(Wpis wpis) : base()
        {
            this.wpis = wpis;
        }


        // Metoda, którą dekoruje dany Dekorator. Zwraca pusty string, powinna zostać nadpisana przez Dekoratory pochodne.
        public virtual string WypiszInformacje()
        {
            //return wpis.WypiszInformacje();
            return "";
        }
    }

    // DekoratorTagowy; przy wywołaniu WypiszInformacje wypisuje również Tagi przypisanego do danego Wpisu
    public class DekoratorTagowy : DekoratorWpisow
    {
        // Konstruktor - pusty, sprawdź Konstrukor DekoratorWpisow
        public DekoratorTagowy(Wpis wpis) : base(wpis) { }


        // Wywołuje metodę WypiszInformacje() podanego Wpisu oraz wypisuje Tagi danego Wpisu.
        public override string WypiszInformacje()
        {
            string returnString = this.wpis.WypiszInformacje();  // Wyowłanie metody w podanym wpisie

            // Tagi danego Wpisu zostaną zapisane w tym stringu
            string tagiStr;
            // Określenie wszystkich tagów
            if (this.wpis.tagi != null && this.wpis.tagi.Count > 0)
                tagiStr = string.Join(", ", this.wpis.tagi.Select(t => t.nazwa));
            else
                tagiStr = "Brak tagów";

            returnString += $" | Tagi: {tagiStr}";
            return returnString;  // Dodanie funkcjonalności przez Dekorator
        }
    }
}

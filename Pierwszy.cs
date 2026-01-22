public class partial Program {
public class Zadanie
{
    public string Tytuł { get; set; }
    public string Treść { get; set; }
    public IStanZadania Stan { get; set; } 
    public Priorytet Priorytet { get; set; } 
    public DateTime Termin { get; set; }
    public List<Tag> Tagi { get; set; } = new List<Tag>();

    public override string ToString()
    {
        return $"Zadanie: {Tytuł}, Termin: {Termin.ToShortDateString()}, Tagi: {string.Join(", ", Tagi)}";
    }
}
public interface IZadanieBuilder
{
    ZadanieBuilder UstawTytuł(string tytuł);
    ZadanieBuilder UstawTreść(string treść);
    ZadanieBuilder UstawStan(IStanZadania stan);
    ZadanieBuilder UstawPriorytet(Priorytet priorytet);
    ZadanieBuilder UstawTermin(DateTime termin);
    ZadanieBuilder UstawTagi(List<Tag> tagi);
    Zadanie Build(); // Metoda kończąca budowanie
}

public class ZadanieBuilder : IZadanieBuilder
{
    private Zadanie _zadanie = new Zadanie();

    public ZadanieBuilder UstawTytuł(string tytuł)
    {
        _zadanie.Tytuł = tytuł;
        return this;
    }

    public ZadanieBuilder UstawTreść(string treść)
    {
        _zadanie.Treść = treść;
        return this;
    }

    public ZadanieBuilder UstawStan(IStanZadania stan)
    {
        _zadanie.Stan = stan;
        return this;
    }

    public ZadanieBuilder UstawPriorytet(Priorytet priorytet)
    {
        _zadanie.Priorytet = priorytet;
        return this;
    }

    public ZadanieBuilder UstawTermin(DateTime termin)
    {
        _zadanie.Termin = termin;
        return this;
    }

    public ZadanieBuilder UstawTagi(List<Tag> tagi)
    {
        _zadanie.Tagi = tagi;
        return this;
    }

    public Zadanie Build()
    {
        Zadanie wynik = _zadanie;
        _zadanie = new Zadanie();
        return wynik;
    }
}
}

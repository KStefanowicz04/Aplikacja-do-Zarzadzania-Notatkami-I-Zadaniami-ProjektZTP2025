
// Program główny
// "partial class" oznacza jedynie, że kod całego programu jest porozsiewany po wielu plikach.
// Wszystkie pliki w projekcie powinny zaczynać się od "public partial class Program"
public partial class Program
{

    // Funkcja Main - wykonuje program
    static void Main(string[] args)
    {

        // Menu główne
        while (true)
        {
            Console.WriteLine("\n\nWybierz komendę:\n1: (co robi 1?)\n2: (co robi 2?)\n3: (co robi 3?)");
            string command = Console.ReadLine();  // Odczytuje komendę z klawiatury - może to być liczba albo słowo.

            switch (command)
            {
                // Przykładowa komenda 1
                case "1":
                    //tu_idzie_nazwa_funkcji();  // przykładowe wywołanie funkcji
                    Console.WriteLine("Wywołano komendę 1");
                    break;

                // Przykładowa komenda 2
                case "2":
                    //tu_idzie_nazwa_funkcji();  // przykładowe wywołanie funkcji
                    Console.WriteLine("Wywołano komendę 2");
                    break;

                // Przykładowa komenda 3
                case "3":
                    //tu_idzie_nazwa_funkcji();  // przykładowe wywołanie funkcji
                    Console.WriteLine("Wywołano komendę 3");
                    break;

                // W przypadku podania nieodpowiedniej komendy, należy wpisać coś innego.
                default:
                    Console.WriteLine("Podaj poprawną komendę");
                    break;
            }
        }
    }
}
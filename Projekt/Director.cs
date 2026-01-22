public partial class Program
{
   private ZadanieBuilder _builder;

   public Director(ZadanieBuilder builder)
   {
      _builder = builder;
   }
   
   public void KonstruujZadaniePilne(string tytul, string tresc)
   {
      _builder
         .UstawTytuł(tytul)
         .UstawTreść(tresc)
         .UstawPriorytet("Wysoki")
         .UstawStan("Nowe")
         .UstawTermin(DateTime.Now.AddDays(1));
   }
   
   public void KonstruujZadanieDlugoterminowe(string tytul, string tresc)
   {
      _builder
         .UstawTytuł(tytul)
         .UstawTreść(tresc)
         .UstawPriorytet("Niski")
         .UstawStan("Planowane")
         .UstawTermin(DateTime.Now.AddMonths(1));
   }
   
   public void KonstruujZadanieKrotkoterminowe(string tytul, string tresc)
   {
      _builder
         .UstawTytuł(tytul)
         .UstawTreść(tresc)
         .UstawPriorytet("Niski")
         .UstawStan("Planowane")
         .UstawTermin(DateTime.Now.AddDays(1));
   }
}

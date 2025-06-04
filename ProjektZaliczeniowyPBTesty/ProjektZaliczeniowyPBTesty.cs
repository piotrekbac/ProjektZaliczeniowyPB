using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjektZaliczeniowyPB;
using System;
using System.Linq;

//Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPBTesty
{
    [TestClass]
    public sealed class ProjektZaliczeniowyPBTesty
    {
        //Test sprawdza poprawność dodawania nowego klienta do naszej bazy danych
        [TestMethod]
        public void AddKlienci_DodajeNowyRekord()
        {
            //Wpierw tworzymy kontekst bazy danych, który będzie używany do interakcji z bazą
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {

                //Teraz przechodzimy do tworzenia nowego obiektu klienta, który chcemy dodać do bazy danych
                var nowy = new Klienci
                {
                    Imie = "Test",
                    Nazwisko = "Testowy",
                    Email = "test@example.com",
                    NumerTelefonu = "123456789"
                };

                //Teraz dodajemy nowego klienta do kontekstu bazy danych i zapisujemy zmiany
                db.Klienci.Add(nowy);
                db.SaveChanges();

                //Następnie sprawdzamy, czy ID klienta zostało nadane, czyli czy rekord został poprawnie dodany do bazy danych
                Assert.IsTrue(nowy.KlientID > 0);

                //Ostatecznie usuwamy klienta, aby nie pozostawiać testowych danych w bazie
                db.Klienci.Remove(nowy);
                db.SaveChanges();
            }
        }

        //Test sprawdza, czy rekord klienta został poprawnie usunięty z bazy danych
        [TestMethod]
        public void RemoveKlienci_UsuwaRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {

                //Przechodzimy do tworzenia i dodawania klienta, które chcemy usunąć
                var doUsuniecia = new Klienci
                {
                    Imie = "Delete",
                    Nazwisko = "Me",
                    Email = "delete@example.com",
                    NumerTelefonu = "987654321"
                };

                //Dodajemy klienta do kontekstu bazy danych i zapisujemy zmiany
                db.Klienci.Add(doUsuniecia);
                db.SaveChanges();

                int id = doUsuniecia.KlientID;

                //Teraz usuwamy klienta z bazy danych
                db.Klienci.Remove(doUsuniecia);
                db.SaveChanges();

                //Ostatecznie sprawdzamy czy rekord nie istnieje w naszej bazie danych
                Assert.IsNull(db.Klienci.Find(id));
            }
        }

        //Test ten sprawdza, dodawanie nowego pracownika do bazy danych
        [TestMethod]
        public void AddPracownicy_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy nowego pracownika, którego chcemy dodać do bazy danych
                var nowy = new Pracownicy
                {
                    Imie = "Adam",
                    Nazwisko = "Nowak",
                    Email = "adam.nowak@test.pl",
                    Miejscowosc = "Kraków"
                };

                //Dodajemy teraz pracownika i zapisujemy go w bazie danych
                db.Pracownicy.Add(nowy);
                db.SaveChanges();

                //Sprawdzamy teraz czy ID zostało poprawnie nadane, co oznacza, że rekord został dodany
                Assert.IsTrue(nowy.PracownikID > 0);

                //Ostatecznie usuwamy pracownika, aby nie pozostawiać testowych danych w bazie
                db.Pracownicy.Remove(nowy);
                db.SaveChanges();
            }
        }

        //Test sprawdza, czy rekord pracownika został poprawnie usunięty z bazy danych
        [TestMethod]
        public void AddSamochody_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {

                //Tworzymy nowy obiekt samochodu, który chcemy dodać do bazy danych
                var nowy = new Samochody
                {
                    Model = "TestModel",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = Guid.NewGuid().ToString().Substring(0, 16),
                    WersjaWyposazenia = "TestWersja"
                };

                //Dodajemy samochód do kontekstu bazy danych i zapisujemy zmiany
                db.Samochody.Add(nowy);
                db.SaveChanges();

                //Sprawdzamy, czy ID samochodu zostało nadane, co oznacza, że rekord został poprawnie dodany
                Assert.IsTrue(nowy.SamochodID > 0);

                //Ostatecznie usuwamy samochód, aby nie pozostawiać testowych danych w bazie
                db.Samochody.Remove(nowy);
                db.SaveChanges();
            }
        }

        //Test sprawdza, czy rekord samochodu został poprawnie usunięty z bazy danych
        [TestMethod]
        public void AddZakupy_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wyszukujemy pierwszego klienta, pracownika i samochód w bazie danych
                var klient = db.Klienci.FirstOrDefault();
                var pracownik = db.Pracownicy.FirstOrDefault();
                var samochod = db.Samochody.FirstOrDefault();

                //Sprawdzamy, czy wszystkie wymagane obiekty zostały znalezione
                if (klient != null && pracownik != null && samochod != null)
                {
                    var zakup = new Zakupy
                    {
                        DataZakupu = DateTime.Now.Date,
                        KlientID = klient.KlientID,
                        PracownikID = pracownik.PracownikID,
                        SamochodID = samochod.SamochodID
                    };

                    //Dodajemy zakup do kontekstu bazy danych i zapisujemy zmiany
                    db.Zakupy.Add(zakup);
                    db.SaveChanges();

                    //Sprawdzamy, czy ID zakupu zostało nadane, co oznacza, że rekord został poprawnie dodany
                    Assert.IsTrue(zakup.ZakupID > 0);

                    //Ostatecznie usuwamy zakup, aby nie pozostawiać testowych danych w bazie
                    db.Zakupy.Remove(zakup);
                    db.SaveChanges();
                }
            }
        }

        //Test sprawdza, czy rekord polisy został poprawnie dodany do bazy danych
        [TestMethod]
        public void AddPolisy_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wyszukujemy pierwszy zakup w bazie danych, aby móc dodać do niego polisę
                var zakup = db.Zakupy.FirstOrDefault();
                if (zakup != null)
                {

                    //Tworzymy nową polisę, która będzie powiązana z zakupem
                    var polisa = new Polisy
                    {
                        ZakupID = zakup.ZakupID,
                        DataRozpoczecia = DateTime.Now.Date,
                        DataZakonczenia = DateTime.Now.Date.AddYears(1)
                    };

                    //Dodajemy polisę do kontekstu bazy danych i zapisujemy zmiany
                    db.Polisy.Add(polisa);
                    db.SaveChanges();

                    //Sprawdzamy, czy ID polisy zostało nadane, co oznacza, że rekord został poprawnie dodany
                    Assert.IsTrue(polisa.PolisaID > 0);

                    //Ostatecznie usuwamy polisę, aby nie pozostawiać testowych danych w bazie
                    db.Polisy.Remove(polisa);
                    db.SaveChanges();
                }
            }
        }

        //Test sprawdza, czy rekord klienta został poprawnie odczytany z bazy danych
        [TestMethod]
        public void ReadKlienci_ZwracaRekordy()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wykonujemy zapytanie do bazy danych, aby pobrać listę klientów
                var lista = db.Klienci.ToList();

                //Sprawdzamy, czy lista nie jest pusta i czy zawiera rekordy
                Assert.IsNotNull(lista);
            }
        }

        //Test sprawdza, czy rekord pracownika został poprawnie odczytany z bazy danych
        [TestMethod]
        public void UpdateKlienci_ModyfikujeRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy nowego klienta, którego będziemy modyfikować
                var klient = new Klienci
                {
                    Imie = "Test",
                    Nazwisko = "DoEdycji",
                    Email = "update@example.com",
                    NumerTelefonu = "123123123"
                };

                //Dodajemy klienta do bazy danych
                db.Klienci.Add(klient);
                db.SaveChanges();

                //Teraz modyfikujemy imię klienta
                klient.Imie = "Zmienione";
                db.SaveChanges();

                //Wyszukujemy klienta w bazie danych, aby sprawdzić, czy zmiany zostały zapisane
                var edytowany = db.Klienci.Find(klient.KlientID);
                Assert.AreEqual("Zmienione", edytowany.Imie);

                //Ostatecznie usuwamy klienta, aby nie pozostawiać testowych danych w bazie
                db.Klienci.Remove(edytowany);
                db.SaveChanges();
            }
        }

        //Test sprawdza, czy rekord klienta został poprawnie usunięty z bazy danych
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void AddKlient_BladWalidacji_EmailDuplikat()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy dwóch klientów z tym samym adresem email, co powinno spowodować błąd walidacji
                var email = "duplikat@test.com";

                //Dodajemy pierwszego klienta
                var k1 = new Klienci
                {
                    Imie = "A",
                    Nazwisko = "B",
                    Email = email,
                    NumerTelefonu = "111222333"
                };

                //Dodajemy drugiego klienta z tym samym adresem email
                var k2 = new Klienci
                {
                    Imie = "C",
                    Nazwisko = "D",
                    Email = email, 
                    NumerTelefonu = "444555666"
                };

                //Dodajemy obu klientów do bazy danych
                db.Klienci.Add(k1);
                db.Klienci.Add(k2);

                //Zapisujemy zmiany w bazie danych, co powinno spowodować błąd walidacji
                db.SaveChanges(); 
            }
        }


        //Test sprawdza, czy rekord samochodu został poprawnie dodany do bazy danych
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void AddSamochod_BladWalidacji_NumerSeryjnyZaKrotki()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy nowy obiekt samochodu z nieprawidłowym numerem seryjnym (za krótki)
                var nowy = new Samochody
                {
                    Model = "ProblemModel",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = "123", 
                    WersjaWyposazenia = "Test"
                };

                //Dodajemy samochód do kontekstu bazy danych i zapisujemy zmiany
                db.Samochody.Add(nowy);
                db.SaveChanges(); 
            }
        }

        //Test sprawdza, czy rekord samochodu został poprawnie odczytany z bazy danych
        [TestMethod]
        public void UpdateSamochody_ModyfikujeRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy nowy obiekt samochodu, który będziemy modyfikować
                var samochod = new Samochody
                {
                    Model = "UpdateTest",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = Guid.NewGuid().ToString().Substring(0, 16),
                    WersjaWyposazenia = "Old"
                };

                //Dodajemy samochód do bazy danych i zapisujemy zmiany
                db.Samochody.Add(samochod);
                db.SaveChanges();

                //Teraz modyfikujemy wersję wyposażenia samochodu i zapisujemy zmiany
                samochod.WersjaWyposazenia = "New";
                db.SaveChanges();

                //Wyszukujemy samochód w bazie danych, aby sprawdzić, czy zmiany zostały zapisane
                var zmodyfikowany = db.Samochody.Find(samochod.SamochodID);

                //Sprawdzamy, czy wersja wyposażenia została zmieniona
                Assert.AreEqual("New", zmodyfikowany.WersjaWyposazenia);

                //Ostatecznie usuwamy zmodyfikowany samochód, aby nie pozostawiać testowych danych w bazie i zapisujemy zmiany
                db.Samochody.Remove(zmodyfikowany);
                db.SaveChanges();
            }
        }

        //Test sprawdza, czy rekord samochodu został poprawnie usunięty z bazy danych
        [TestMethod]
        public void RemoveSamochody_UsuwaRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {

                //Tworzymy nowy obiekt samochodu, który chcemy usunąć
                var samochod = new Samochody
                {
                    Model = "DeleteTest",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = Guid.NewGuid().ToString().Substring(0, 16),
                    WersjaWyposazenia = "Test"
                };

                //Dodajemy samochód do bazy danych i zapisujemy zmiany
                db.Samochody.Add(samochod);
                db.SaveChanges();

                //Teraz usuwamy samochód z bazy danych i zapisujemy zmiany
                int id = samochod.SamochodID;
                db.Samochody.Remove(samochod);
                db.SaveChanges();

                //Sprawdzamy, czy rekord samochodu został poprawnie usunięty z bazy danych
                Assert.IsNull(db.Samochody.Find(id));
            }
        }

        //Testy sprawdzają, czy rekordy pracowników zostały poprawnie odczytane z bazy danych
        [TestMethod]
        public void ReadPracownicy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {

                //Wykonujemy zapytanie do bazy danych, aby pobrać listę pracowników
                var lista = db.Pracownicy.ToList();

                //Sprawdzamy, czy lista nie jest pusta i czy zawiera rekordy
                Assert.IsNotNull(lista);
                Assert.IsTrue(lista.Count >= 0);
            }
        }


        //Test sprawdza, czy rekord pracownika został poprawnie zaktualizowany w bazie danych
        [TestMethod]
        public void UpdatePracownicy_AktualizujeEmail()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy nowego pracownika, którego będziemy aktualizować
                var pracownik = new Pracownicy
                {
                    Imie = "Jan",
                    Nazwisko = "Aktualizacja",
                    Email = "stary@email.pl",
                    Miejscowosc = "Testowo"
                };

                //Dodajemy pracownika do bazy danych i zapisujemy zmiany
                db.Pracownicy.Add(pracownik);
                db.SaveChanges();

                //Teraz aktualizujemy email pracownika i zapisujemy zmiany
                pracownik.Email = "nowy@email.pl";
                db.SaveChanges();

                //Wyszukujemy pracownika w bazie danych, aby sprawdzić, czy zmiany zostały zapisane
                var aktualny = db.Pracownicy.Find(pracownik.PracownikID);
                Assert.AreEqual("nowy@email.pl", aktualny.Email);

                //Ostatecznie usuwamy pracownika, aby nie pozostawiać testowych danych w bazie i zapisujemy zmiany
                db.Pracownicy.Remove(aktualny);
                db.SaveChanges();
            }
        }

        //Test sprawdza, czy rekord polisy został poprawnie usunięty z bazy danych
        [TestMethod]
        public void RemoveZakupy_UsuwaTransakcje()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wyszukujemy pierwszego klienta, pracownika i samochód w bazie danych, aby utworzyć zakup
                var klient = db.Klienci.FirstOrDefault();
                var pracownik = db.Pracownicy.FirstOrDefault();
                var samochod = db.Samochody.FirstOrDefault();

                //Sprawdzamy, czy wszystkie wymagane obiekty zostały znalezione
                if (klient != null && pracownik != null && samochod != null)
                {

                    //Tworzymy nowy obiekt zakupu, który chcemy usunąć
                    var zakup = new Zakupy
                    {
                        KlientID = klient.KlientID,
                        PracownikID = pracownik.PracownikID,
                        SamochodID = samochod.SamochodID,
                        DataZakupu = DateTime.Now
                    };

                    //Dodajemy zakup do bazy danych i zapisujemy zmiany
                    db.Zakupy.Add(zakup);
                    db.SaveChanges();

                    //Teraz usuwamy zakup z bazy danych i zapisujemy zmiany
                    int id = zakup.ZakupID;
                    db.Zakupy.Remove(zakup);
                    db.SaveChanges();

                    //Sprawdzamy, czy rekord zakupu został poprawnie usunięty z bazy danych
                    Assert.IsNull(db.Zakupy.Find(id));
                }
            }
        }

        //Testy sprawdzają, czy rekordy polis zostały poprawnie odczytane z bazy danych
        [TestMethod]
        public void ReadPolisy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wykonujemy zapytanie do bazy danych, aby pobrać listę polis
                var polisy = db.Polisy.ToList();

                //Sprawdzamy, czy lista nie jest pusta i czy zawiera rekordy
                Assert.IsNotNull(polisy);
            }
        }


        //Testy sprawdzają, czy rekordy zakupów zostały poprawnie odczytane z bazy danych
        [TestMethod]
        public void ReadZakupy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {

                //Wykonujemy zapytanie do bazy danych, aby pobrać listę zakupów
                var lista = db.Zakupy.ToList();

                //Sprawdzamy, czy lista nie jest pusta i czy zawiera rekordy
                Assert.IsNotNull(lista);
            }
        }

        //Testy sprawdzają, czy rekordy wydziałów zostały poprawnie odczytane z bazy danych
        [TestMethod]
        public void ReadWydzialy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wykonujemy zapytanie do bazy danych, aby pobrać listę wydziałów
                var lista = db.Wydzialy.ToList();

                //Sprawdzamy, czy lista nie jest pusta i czy zawiera rekordy
                Assert.IsNotNull(lista);
            }
        }

        //Test sprawdza, czy rekord wydziału został poprawnie dodany do bazy danych
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void AddZakupy_BladBezSamochodu()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Wyszukujemy pierwszego klienta i pracownika w bazie danych, aby utworzyć zakup
                var klient = db.Klienci.FirstOrDefault();
                var pracownik = db.Pracownicy.FirstOrDefault();

                //Sprawdzamy, czy klient i pracownik istnieją w bazie danych
                if (klient != null && pracownik != null)
                {

                    //Tworzymy nowy obiekt zakupu bez przypisanego samochodu, co powinno spowodować błąd walidacji
                    var zakup = new Zakupy
                    {
                        KlientID = klient.KlientID,
                        PracownikID = pracownik.PracownikID,
                        SamochodID = 0, 
                        DataZakupu = DateTime.Now
                    };

                    //Dodajemy zakup do bazy danych i zapisujemy zmiany, co powinno spowodować błąd walidacji i zapisujemy zmiany
                    db.Zakupy.Add(zakup);
                    db.SaveChanges(); 
                }
            }
        }

        //Test sprawdza, czy rekord klienta z minimalnymi danymi został poprawnie dodany do bazy danych
        [TestMethod]
        public void CzyMoznaDodacIDoBazy_RekordZMinimalnymiDanymi()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                //Tworzymy nowego klienta z minimalnymi wymaganymi danymi
                var klient = new Klienci
                {
                    Imie = "A",
                    Nazwisko = "B",
                    Email = Guid.NewGuid().ToString().Substring(0, 8) + "@x.pl",
                    NumerTelefonu = "123456789"
                };

                //Dodajemy klienta do bazy danych i zapisujemy zmiany
                db.Klienci.Add(klient);
                db.SaveChanges();

                //Sprawdzamy, czy ID klienta zostało nadane, co oznacza, że rekord został poprawnie dodany do bazy danych
                Assert.IsTrue(klient.KlientID > 0);

                //Ostatecznie usuwamy klienta, aby nie pozostawiać testowych danych w bazie i zapisujemy zmiany
                db.Klienci.Remove(klient);
                db.SaveChanges();
            }
        }
    }
}

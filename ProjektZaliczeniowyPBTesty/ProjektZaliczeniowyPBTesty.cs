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
        [TestMethod]
        public void AddKlienci_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var nowy = new Klienci
                {
                    Imie = "Test",
                    Nazwisko = "Testowy",
                    Email = "test@example.com",
                    NumerTelefonu = "123456789"
                };
                db.Klienci.Add(nowy);
                db.SaveChanges();

                Assert.IsTrue(nowy.KlientID > 0);
                db.Klienci.Remove(nowy);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void RemoveKlienci_UsuwaRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var doUsuniecia = new Klienci
                {
                    Imie = "Delete",
                    Nazwisko = "Me",
                    Email = "delete@example.com",
                    NumerTelefonu = "987654321"
                };
                db.Klienci.Add(doUsuniecia);
                db.SaveChanges();

                int id = doUsuniecia.KlientID;
                db.Klienci.Remove(doUsuniecia);
                db.SaveChanges();

                Assert.IsNull(db.Klienci.Find(id));
            }
        }

        [TestMethod]
        public void AddPracownicy_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var nowy = new Pracownicy
                {
                    Imie = "Adam",
                    Nazwisko = "Nowak",
                    Email = "adam.nowak@test.pl",
                    Miejscowosc = "Kraków"
                };
                db.Pracownicy.Add(nowy);
                db.SaveChanges();

                Assert.IsTrue(nowy.PracownikID > 0);
                db.Pracownicy.Remove(nowy);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void AddSamochody_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var nowy = new Samochody
                {
                    Model = "TestModel",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = Guid.NewGuid().ToString().Substring(0, 16),
                    WersjaWyposazenia = "TestWersja"
                };
                db.Samochody.Add(nowy);
                db.SaveChanges();

                Assert.IsTrue(nowy.SamochodID > 0);
                db.Samochody.Remove(nowy);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void AddZakupy_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                
                var klient = db.Klienci.FirstOrDefault();
                var pracownik = db.Pracownicy.FirstOrDefault();
                var samochod = db.Samochody.FirstOrDefault();

                if (klient != null && pracownik != null && samochod != null)
                {
                    var zakup = new Zakupy
                    {
                        DataZakupu = DateTime.Now.Date,
                        KlientID = klient.KlientID,
                        PracownikID = pracownik.PracownikID,
                        SamochodID = samochod.SamochodID
                    };
                    db.Zakupy.Add(zakup);
                    db.SaveChanges();

                    Assert.IsTrue(zakup.ZakupID > 0);
                    db.Zakupy.Remove(zakup);
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void AddPolisy_DodajeNowyRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var zakup = db.Zakupy.FirstOrDefault();
                if (zakup != null)
                {
                    var polisa = new Polisy
                    {
                        ZakupID = zakup.ZakupID,
                        DataRozpoczecia = DateTime.Now.Date,
                        DataZakonczenia = DateTime.Now.Date.AddYears(1)
                    };
                    db.Polisy.Add(polisa);
                    db.SaveChanges();

                    Assert.IsTrue(polisa.PolisaID > 0);
                    db.Polisy.Remove(polisa);
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void ReadKlienci_ZwracaRekordy()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var lista = db.Klienci.ToList();
                Assert.IsNotNull(lista);
            }
        }

        [TestMethod]
        public void UpdateKlienci_ModyfikujeRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var klient = new Klienci
                {
                    Imie = "Test",
                    Nazwisko = "DoEdycji",
                    Email = "update@example.com",
                    NumerTelefonu = "123123123"
                };
                db.Klienci.Add(klient);
                db.SaveChanges();

                klient.Imie = "Zmienione";
                db.SaveChanges();

                var edytowany = db.Klienci.Find(klient.KlientID);
                Assert.AreEqual("Zmienione", edytowany.Imie);

                db.Klienci.Remove(edytowany);
                db.SaveChanges();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void AddKlient_BladWalidacji_EmailDuplikat()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var email = "duplikat@test.com";

                var k1 = new Klienci
                {
                    Imie = "A",
                    Nazwisko = "B",
                    Email = email,
                    NumerTelefonu = "111222333"
                };

                var k2 = new Klienci
                {
                    Imie = "C",
                    Nazwisko = "D",
                    Email = email, 
                    NumerTelefonu = "444555666"
                };

                db.Klienci.Add(k1);
                db.Klienci.Add(k2);
                db.SaveChanges(); 
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void AddSamochod_BladWalidacji_NumerSeryjnyZaKrotki()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var nowy = new Samochody
                {
                    Model = "ProblemModel",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = "123", 
                    WersjaWyposazenia = "Test"
                };

                db.Samochody.Add(nowy);
                db.SaveChanges(); 
            }
        }

        [TestMethod]
        public void UpdateSamochody_ModyfikujeRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var samochod = new Samochody
                {
                    Model = "UpdateTest",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = Guid.NewGuid().ToString().Substring(0, 16),
                    WersjaWyposazenia = "Old"
                };
                db.Samochody.Add(samochod);
                db.SaveChanges();

                samochod.WersjaWyposazenia = "New";
                db.SaveChanges();

                var zmodyfikowany = db.Samochody.Find(samochod.SamochodID);
                Assert.AreEqual("New", zmodyfikowany.WersjaWyposazenia);

                db.Samochody.Remove(zmodyfikowany);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void RemoveSamochody_UsuwaRekord()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var samochod = new Samochody
                {
                    Model = "DeleteTest",
                    RokProdukcji = DateTime.Now.Year,
                    NumerSeryjny = Guid.NewGuid().ToString().Substring(0, 16),
                    WersjaWyposazenia = "Test"
                };
                db.Samochody.Add(samochod);
                db.SaveChanges();

                int id = samochod.SamochodID;
                db.Samochody.Remove(samochod);
                db.SaveChanges();

                Assert.IsNull(db.Samochody.Find(id));
            }
        }

        [TestMethod]
        public void ReadPracownicy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var lista = db.Pracownicy.ToList();
                Assert.IsNotNull(lista);
                Assert.IsTrue(lista.Count >= 0);
            }
        }

        [TestMethod]
        public void UpdatePracownicy_AktualizujeEmail()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var pracownik = new Pracownicy
                {
                    Imie = "Jan",
                    Nazwisko = "Aktualizacja",
                    Email = "stary@email.pl",
                    Miejscowosc = "Testowo"
                };
                db.Pracownicy.Add(pracownik);
                db.SaveChanges();

                pracownik.Email = "nowy@email.pl";
                db.SaveChanges();

                var aktualny = db.Pracownicy.Find(pracownik.PracownikID);
                Assert.AreEqual("nowy@email.pl", aktualny.Email);

                db.Pracownicy.Remove(aktualny);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void RemoveZakupy_UsuwaTransakcje()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var klient = db.Klienci.FirstOrDefault();
                var pracownik = db.Pracownicy.FirstOrDefault();
                var samochod = db.Samochody.FirstOrDefault();

                if (klient != null && pracownik != null && samochod != null)
                {
                    var zakup = new Zakupy
                    {
                        KlientID = klient.KlientID,
                        PracownikID = pracownik.PracownikID,
                        SamochodID = samochod.SamochodID,
                        DataZakupu = DateTime.Now
                    };

                    db.Zakupy.Add(zakup);
                    db.SaveChanges();

                    int id = zakup.ZakupID;
                    db.Zakupy.Remove(zakup);
                    db.SaveChanges();

                    Assert.IsNull(db.Zakupy.Find(id));
                }
            }
        }

        [TestMethod]
        public void ReadPolisy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var polisy = db.Polisy.ToList();
                Assert.IsNotNull(polisy);
            }
        }

        [TestMethod]
        public void ReadZakupy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var lista = db.Zakupy.ToList();
                Assert.IsNotNull(lista);
            }
        }

        [TestMethod]
        public void ReadWydzialy_ZwracaListe()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var lista = db.Wydzialy.ToList();
                Assert.IsNotNull(lista);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void AddZakupy_BladBezSamochodu()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var klient = db.Klienci.FirstOrDefault();
                var pracownik = db.Pracownicy.FirstOrDefault();

                if (klient != null && pracownik != null)
                {
                    var zakup = new Zakupy
                    {
                        KlientID = klient.KlientID,
                        PracownikID = pracownik.PracownikID,
                        SamochodID = 0, 
                        DataZakupu = DateTime.Now
                    };

                    db.Zakupy.Add(zakup);
                    db.SaveChanges(); 
                }
            }
        }

        [TestMethod]
        public void CzyMoznaDodacIDoBazy_RekordZMinimalnymiDanymi()
        {
            using (var db = new ProjektZaliczeniowyBazaSamochodowEntities())
            {
                var klient = new Klienci
                {
                    Imie = "A",
                    Nazwisko = "B",
                    Email = Guid.NewGuid().ToString().Substring(0, 8) + "@x.pl",
                    NumerTelefonu = "123456789"
                };
                db.Klienci.Add(klient);
                db.SaveChanges();

                Assert.IsTrue(klient.KlientID > 0);

                db.Klienci.Remove(klient);
                db.SaveChanges();
            }
        }

    }
}

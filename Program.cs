using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HegyekCSHARP
{
    class Hegy
    {
        public string nev { get; set; }
        public string hegyseg { get; set; }
        public int magassag { get; set; }

        public Hegy(string nev, string hegyseg, int magassag)
        {
            this.nev = nev;
            this.hegyseg = hegyseg;
            this.magassag = magassag;
        }
    }
    class Program
    {
        public static List<Hegy> hegyLista = beolvas();
        public static List<Hegy> beolvas()
        {
            List<Hegy> lista = new List<Hegy>();
            try
            {
                using (StreamReader sr=new StreamReader(new FileStream("hegyekMo.txt",FileMode.Open),Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var split = sr.ReadLine().Split(';');
                        var o = new Hegy(
                                split[0],
                                split[1],
                                Convert.ToInt32(split[2])
                            );
                        lista.Add(o);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine( "Hiba a beolvasásnál. "+e);
            }
            return lista;
        }

        static void Main(string[] args)
        {
            #region 3. feladat
            Console.WriteLine("3. feladat: Hegycsúcsok száma: "+beolvas().Count()+" db");
            #endregion

            #region 4. feladat
            var atlag = Math.Round(hegyLista.Average(x=>x.magassag),2);
            Console.WriteLine("4. feladat: Hegycsúcsok átlagos magassága: {0} m",atlag);
            #endregion

            #region 5. feladat
            var legmag = hegyLista.OrderByDescending(x => x.magassag).First();
            Console.WriteLine("5. feladat: A legmagasabb hegycsúcs adatai: ");
            Console.WriteLine($"\tNév:{legmag.nev}");
            Console.WriteLine($"\tHegység:{legmag.hegyseg}");
            Console.WriteLine($"\tMagasság:{legmag.magassag}");
            #endregion

            #region 6. feladat
            Console.Write("6. feladat: Kérek egy magasságot:");
            var beker = Convert.ToInt32(Console.ReadLine());
            var valasz = "Nincs "+beker+"m-nél magasabb hegycsúcs a Börzsönyben!";
            foreach (var item in hegyLista)
            {
                if (item.magassag>beker)
                {
                    valasz ="Van "+beker+"m - nél magasabb hegycsúcs a Börzsönyben!";
                    break;
                }
            }
            Console.WriteLine("\t"+valasz);
            #endregion

            #region 7. feladat
            var db = hegyLista.Where(x => x.magassag > (double)(3000 / 3.280839895)).Count();
            Console.WriteLine("7. feladat: 3000 lábnál magasabb hegycsúcsok száma: "+db);
            #endregion

            #region 8. feladat
            Console.WriteLine("8. feladat: Hegység statisztika");
            var stat = hegyLista.GroupBy(x => x.hegyseg).
                Select(y => 
                new 
                    { 
                        key=y.Key,
                        value=y.Count()
                    }
                ).ToList();
            stat.ForEach(x=>
                    Console.WriteLine("\t"+x.key+" - "+x.value+" db")
                );
            #endregion

            #region 9. feladat
            var bukkList = hegyLista.Where(x => x.hegyseg.Contains("Bükk")).OrderBy(x=>x.nev).ToList();
            using (StreamWriter sw=new StreamWriter(new FileStream("bukk-videk.txt",FileMode.Create),Encoding.UTF8))
            {
                sw.WriteLine("Hegycsúcs neve;Magasság láb");
                bukkList.ForEach(x=> {
                    sw.WriteLine(x.nev+";"+Math.Round(x.magassag*3.280839895,1).ToString().Replace(',','.'));
                });
            }
                Console.WriteLine("9. feladat: bukk-videk.txt");
            #endregion
            Console.ReadKey();
        }
    }
}

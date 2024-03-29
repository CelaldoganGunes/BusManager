﻿namespace otobus
{
    internal class Program
    {
        public static Random random = new Random();
        public const int YOLCU_SAYISI = 40;

        public static double[,] uzaklikMatrisi = new double[YOLCU_SAYISI, YOLCU_SAYISI];
        public static string[] yolcuIsimleri = new string[YOLCU_SAYISI];
        public static int[] koltuklar = new int[YOLCU_SAYISI];  // Koltuk Numarası, Kişinin yolcuIsimleri listesindeki indeksi
        public static bool[] yerlesenYolcular = new bool[YOLCU_SAYISI]; //Yerleşenler true, diğerleri false
        public static double[] mesafeToplamlari = new double[YOLCU_SAYISI];

        static void Main(string[] args)
        {
            #region Uzaklik Matrisini Double ile Doldur ve Yolcu Listesini Doldur

            for (int satir = 0; satir < YOLCU_SAYISI; satir++)
            {
                for (int sutun = 0; sutun < YOLCU_SAYISI; sutun++)
                {
                    if (satir == sutun)
                    {
                        //Eğer kendisi ise 0 yazıyoruz.
                        uzaklikMatrisi[satir, sutun] = 0;
                    }
                    else if (uzaklikMatrisi[sutun, satir] == 0)
                    {
                        //Eğer ters taraftan bakınca değer girilmemişse kendisine değer atıyoruz.
                        uzaklikMatrisi[satir, sutun] = CreateRandomizedDouble();
                    }
                    else
                    {
                        //Eğer ters taraftan bakınca değer girilmişse o değerin aynısını yazıyoruz.
                        uzaklikMatrisi[satir, sutun] = uzaklikMatrisi[sutun, satir];
                    }

                    //string s = string.Format("{0:N2}", uzaklikMatrisi[satir, sutun]);
                    //Console.Write(s + "   ");
                }
                //Console.WriteLine("");
                //Console.WriteLine("");

            }


            // İsimleri ayarla


            for (int i = 0; i < YOLCU_SAYISI; i++)
            {
                string isim = "Yolcu_" + i.ToString();
                yolcuIsimleri[i] = isim;
                //Console.WriteLine(yolcuIsimleri[i]);

            }

            #endregion


            #region Yolcuları Yerleştirme



            // İlk yolcuyu yerleştir
            int ilkYolcununNumarasi = random.Next(YOLCU_SAYISI);
            koltuklar[0] = ilkYolcununNumarasi;
            yerlesenYolcular[ilkYolcununNumarasi] = true;

            // Diğer yolcuları yerleştir

            for (int koltukNumarasi = 1; koltukNumarasi < koltuklar.Length; koltukNumarasi++) //Ana Döngü
            {
                int secilenKisininIndeksi = -1;
                double minimumUzaklik = 41; //Random double maksimum 10 oluyordu, en fazla 4 komşu hesaba katıldığından 40 olur. Garanti için 1 fazlasını aldım.

                for (int kisininIndeksi = 0; kisininIndeksi < YOLCU_SAYISI; kisininIndeksi++)
                {
                    double kisininUzakligi = 0;
                    if (yerlesenYolcular[kisininIndeksi] == true)
                    {
                        continue;
                    }

                    if (koltukNumarasi % 4 == 0) // En sol taraf
                    {
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 4);
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 3);
                    }
                    else if (koltukNumarasi % 4 == 2) // Koridorun sağ tarafı
                    {
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 5);
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 4);
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 3);
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 1);
                    }
                    else // Koridorun solu ve en sol taraf
                    {
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 5);
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 4);
                        kisininUzakligi += UzaklikHesapla(kisininIndeksi, koltukNumarasi - 1);
                    }


                    if (kisininUzakligi < minimumUzaklik)
                    {
                        minimumUzaklik = kisininUzakligi;
                        secilenKisininIndeksi = kisininIndeksi;
                    }
                }

                koltuklar[koltukNumarasi] = secilenKisininIndeksi;
                yerlesenYolcular[secilenKisininIndeksi] = true;
                mesafeToplamlari[secilenKisininIndeksi] = minimumUzaklik;

            }

            #endregion

            // c maddesi, Otobüs dizilimini ekrana yazdır.
            Console.WriteLine("Yolcuların Oturuş Sıraları:");
            Console.WriteLine(" ");
            for (int koltukNumarasi = 0; koltukNumarasi < YOLCU_SAYISI; koltukNumarasi++)
            {
                Console.Write("[Koltuk No: " + (koltukNumarasi + 1) + " Yolcu No: " + koltuklar[koltukNumarasi] + " Yolcu Adi: " + yolcuIsimleri[koltuklar[koltukNumarasi]] + "] ");
                if (koltukNumarasi % 4 == 3)
                {
                    Console.WriteLine(" ");
                }
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");


            // d maddesi, mesafeleri ekrana yazdır.
            Console.WriteLine("Yolcuların Uzaklıkları:");
            Console.WriteLine(" ");
            for (int koltukNumarasi = 0; koltukNumarasi < YOLCU_SAYISI; koltukNumarasi++)
            {
                int kisininIndeksi = koltuklar[koltukNumarasi];

                string uzaklik = string.Format("{0:N2}", mesafeToplamlari[kisininIndeksi]);
                Console.Write("[Koltuk No: " + (koltukNumarasi+1) + " Yolcu No: " + koltuklar[koltukNumarasi] + " Uzaklik: " + uzaklik + "] ");
                if (koltukNumarasi % 4 == 3)
                {
                    Console.WriteLine(" ");
                }
            }
            
            // e maddesi
            Console.WriteLine("");
            Console.WriteLine("Toplam Uzaklık: " + mesafeToplamlari.Sum());

            Console.ReadKey();
        }

        public static double CreateRandomizedDouble()
        {

            double uzaklikRandom = random.NextDouble();
            double tamUzaklik = uzaklikRandom * 10;
            return tamUzaklik;
        }

        public static double UzaklikHesapla(int kisininIndeksi, int koltukIndeksi)
        {
            if (koltukIndeksi < 0) //İndeks dışına çıkıyorsa o kişi yoktur ve uzaklık 0 dır.
            {
                return 0;
            }
            return uzaklikMatrisi[kisininIndeksi, koltuklar[koltukIndeksi]];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // Memanggil method utama permainan
                MainPermainan();

                // Memeriksa apakah pengguna ingin merestart permainan
                if (!MintaRestart())
                    break;
            }
        }

        private static void MainPermainan()
        {
            // Inisialisasi ukuran layar dan objek random
            Console.WindowHeight = 16;
            Console.WindowWidth = 37;
            int lebarLayar = Console.WindowWidth;
            int tinggiLayar = Console.WindowHeight;
            Random nomorAcak = new Random();

            // Inisialisasi variabel permainan
            int skor = 0;
            int gameover = 0;
            int kecepatanAwal = 100; // Kecepatan awal ular (m/s)
            int kecepatanPermainan = kecepatanAwal;
            int penambahanKecepatan = 5; // Penambahan kecepatan setiap kali makan buah
            int kecepatanMinimum = 50; // Kecepatan minimum yang dapat dicapai
            int kecepatanMaksimum = 300; // Kecepatan maksimum yang dapat dicapai
            int ukuranUlar = 1;

            // Inisialisasi objek Pixel sebagai kepala ular
            Pixel kepalaUlar = new Pixel();
            kepalaUlar.xpos = lebarLayar / 2;
            kepalaUlar.ypos = tinggiLayar / 2;
            kepalaUlar.warnaScherm = ConsoleColor.Blue;

            // Arah awal ular
            string arah = "RIGHT";

            // List untuk menyimpan posisi badan ular
            List<int> xposBadanUlar = new List<int>();
            List<int> yposBadanUlar = new List<int>();

            // Posisi awal buah
            int posisiBuahX = nomorAcak.Next(1, lebarLayar - 2);
            int posisiBuahY = nomorAcak.Next(1, tinggiLayar - 2);

            // Warna awal buah
            ConsoleColor warnaBuah = DapatkanWarnaKonsolAcak();

            // Waktu untuk mengukur kecepatan permainan
            DateTime waktu = DateTime.Now;
            DateTime waktu2;

            // Flag untuk menangkap input tombol
            string tombolDitekan = "no";

            // Menggambar border pada layar
            GambarBorder(lebarLayar, tinggiLayar);

            while (true)
            {
                // Membersihkan konsol kecuali border
                BersihkanKonsol(lebarLayar, tinggiLayar);

                // Memeriksa kondisi game over (menabrak dinding atau tubuh ular)
                if (kepalaUlar.xpos == lebarLayar - 1 || kepalaUlar.xpos == 0 || kepalaUlar.ypos == tinggiLayar - 1 || kepalaUlar.ypos == 0)
                {
                    gameover = 1;
                }

                // Memeriksa apakah kepala ular memakan buah
                Console.ForegroundColor = ConsoleColor.Magenta;
                if (posisiBuahX == kepalaUlar.xpos && posisiBuahY == kepalaUlar.ypos)
                {
                    skor++;
                    ukuranUlar++;

                    // Mencari posisi baru untuk buah yang tidak bertabrakan dengan badan ular
                    do
                    {
                        posisiBuahX = nomorAcak.Next(1, lebarLayar - 2);
                        posisiBuahY = nomorAcak.Next(1, tinggiLayar - 2);
                    } while (xposBadanUlar.Contains(posisiBuahX) && yposBadanUlar.Contains(posisiBuahY));

                    // Mengubah warna buah secara acak
                    warnaBuah = DapatkanWarnaKonsolAcak();

                    // Menyesuaikan kecepatan permainan berdasarkan skor
                    kecepatanPermainan = Math.Max(kecepatanMinimum, kecepatanPermainan + penambahanKecepatan);


                    // Menampilkan skor dan kecepatan saat ini
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"Skor: {skor}");

                    Console.SetCursorPosition(lebarLayar - 15, tinggiLayar - 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"Kecepatan: {kecepatanPermainan}   ");
                }

                // Menggambar badan ular
                for (int i = 0; i < xposBadanUlar.Count; i++)
                {
                    Console.SetCursorPosition(xposBadanUlar[i], yposBadanUlar[i]);
                    Console.Write("O"); // Menggunakan karakter lain untuk badan ular

                    // Memeriksa tabrakan dengan badan ular
                    if (xposBadanUlar[i] == kepalaUlar.xpos && yposBadanUlar[i] == kepalaUlar.ypos)
                    {
                        gameover = 1;
                    }
                }

                // Menyelesaikan permainan jika game over
                if (gameover == 1)
                {
                    break;
                }

                // Menggambar kepala ular
                Console.SetCursorPosition(kepalaUlar.xpos, kepalaUlar.ypos);
                Console.ForegroundColor = kepalaUlar.warnaScherm;
                Console.Write("O"); // Menggunakan karakter lain untuk kepala ular

                // Menggambar buah
                Console.SetCursorPosition(posisiBuahX, posisiBuahY);
                Console.ForegroundColor = warnaBuah;
                Console.Write("■");

                // Menyembunyikan kursor
                Console.CursorVisible = false;
                waktu = DateTime.Now;
                tombolDitekan = "no";

                // Loop menunggu input tombol dengan interval waktu tertentu
                while (true)
                {
                    waktu2 = DateTime.Now;
                    // Menggunakan rumus invers kecepatanPermainan untuk menentukan waktu tunggu
                    int waktuTunggu = (int)(kecepatanMaksimum - kecepatanPermainan + kecepatanMinimum);
                    if (waktu2.Subtract(waktu).TotalMilliseconds > waktuTunggu) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo tombol = Console.ReadKey(true);
                        if (tombol.Key.Equals(ConsoleKey.UpArrow) && arah != "DOWN" && tombolDitekan == "no")
                        {
                            arah = "UP";
                            tombolDitekan = "yes";
                        }
                        if (tombol.Key.Equals(ConsoleKey.DownArrow) && arah != "UP" && tombolDitekan == "no")
                        {
                            arah = "DOWN";
                            tombolDitekan = "yes";
                        }
                        if (tombol.Key.Equals(ConsoleKey.LeftArrow) && arah != "RIGHT" && tombolDitekan == "no")
                        {
                            arah = "LEFT";
                            tombolDitekan = "yes";
                        }
                        if (tombol.Key.Equals(ConsoleKey.RightArrow) && arah != "LEFT" && tombolDitekan == "no")
                        {
                            arah = "RIGHT";
                            tombolDitekan = "yes";
                        }
                    }
                }

                // Menyimpan posisi sebelumnya dan menambahkannya ke list
                xposBadanUlar.Add(kepalaUlar.xpos);
                yposBadanUlar.Add(kepalaUlar.ypos);

                // Memastikan badan ular tidak lebih panjang dari ukuran yang ditentukan
                while (xposBadanUlar.Count > ukuranUlar)
                {
                    xposBadanUlar.RemoveAt(0);
                    yposBadanUlar.RemoveAt(0);
                }

                // Menggerakkan kepala ular sesuai arah
                switch (arah)
                {
                    case "UP":
                        kepalaUlar.ypos--;
                        break;
                    case "DOWN":
                        kepalaUlar.ypos++;
                        break;
                    case "LEFT":
                        kepalaUlar.xpos--;
                        break;
                    case "RIGHT":
                        kepalaUlar.xpos++;
                        break;
                }

                // Menyesuaikan kecepatan minimum dan maksimum
                kecepatanMinimum = Math.Max(kecepatanMinimum - 1, 50);
                kecepatanMaksimum = Math.Min(kecepatanMaksimum + 1, 150);
            }

            // Menampilkan pesan game over dan skor setelah permainan selesai
            Console.SetCursorPosition(lebarLayar / 5, tinggiLayar / 2);
            Console.WriteLine("Game over, Skor: " + skor);
            Console.SetCursorPosition(lebarLayar / 5, tinggiLayar / 2 + 1);
            Console.WriteLine("Tekan Spasi untuk Restart");
        }

        // Method untuk meminta pengguna merestart permainan
        private static bool MintaRestart()
        {
            ConsoleKeyInfo tombol;
            do
            {
                tombol = Console.ReadKey(true);
            } while (tombol.Key != ConsoleKey.Spacebar && tombol.Key != ConsoleKey.Escape);

            return tombol.Key == ConsoleKey.Spacebar;
        }

        // Method untuk membersihkan konsol kecuali border
        private static void BersihkanKonsol(int lebarLayar, int tinggiLayar)
        {
            var garisHitam = string.Join("", new byte[lebarLayar - 2].Select(b => " ").ToArray());
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 1; i < tinggiLayar - 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(garisHitam);
            }

            Console.ResetColor();
        }

        // Method untuk menggambar border pada layar
        private static void GambarBorder(int lebarLayar, int tinggiLayar)
        {
            var garisHorizontal = string.Join("", new byte[lebarLayar].Select(b => "■").ToArray());

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(garisHorizontal);
            Console.SetCursorPosition(0, tinggiLayar - 1);
            Console.Write(garisHorizontal);

            for (int i = 0; i < tinggiLayar; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
                Console.SetCursorPosition(lebarLayar - 1, i);
                Console.Write("#");
            }

            Console.ResetColor();
        }

        // Method untuk mendapatkan warna konsol acak dari daftar warna buah yang ditentukan
        private static ConsoleColor DapatkanWarnaKonsolAcak()
        {
            Array nilai = Enum.GetValues(typeof(ConsoleColor));
            Random acak = new Random();

            // Tetapkan warna yang diinginkan
            ConsoleColor[] warnaBuah = { ConsoleColor.Red, ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Green };

            // Dapatkan warna acak dari daftar yang ditentukan
            return warnaBuah[acak.Next(warnaBuah.Length)];
        }

        // Kelas yang merepresentasikan satu piksel atau elemen dalam permainan
        class Pixel
        {
            public int xpos { get; set; }
            public int ypos { get; set; }
            public ConsoleColor warnaScherm { get; set; }
        }
    }
}

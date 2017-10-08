using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClassSnatcher1._3
{
    class Program
    {
        static void Main(string[] args)
        {
            string subject, term, Crn; // HTTP GET Request parameters

            Console.WriteLine("What is your subject name abbreviated? Enter in all CAPS. Ex: PHYSICS = PHYS, MATHEMATICS = MATH.");
            subject = Console.ReadLine();

            Console.WriteLine("What is the term for the course? Concatenate the year + the Season Query Code.");
            Console.WriteLine("Season Query Codes:");
            Console.WriteLine("Spring = 03");
            Console.WriteLine("Summer = 05");
            Console.WriteLine("Fall = 07");
            Console.WriteLine("Ex: 2017 Summer = 2017 + 05 = 201705");
            term = Console.ReadLine();

            Console.WriteLine("What is the class's CRN?. Ex: 50408");
            Crn = Console.ReadLine();
            int crn = Convert.ToInt32(Crn);

            string url = "https://ssb.vcccd.edu/prod/pw_pub_sched.p_course_popup?vsub=" + subject + "&vterm=" + term + "&vcrn=" + crn; // concactanate website
            Console.WriteLine(url);

            Console.WriteLine("Check remaining number of seats every __ seconds! Ex: 10 'for every 10 seconds");
            string s = Console.ReadLine();
            int seconds = Convert.ToInt32(s);
            int ms = seconds * 1000;

            Console.WriteLine("Allow __ notifications max (prevents email spam) Ex: 10");
            string notifications = Console.ReadLine();
            int bells = Convert.ToInt32(notifications);

            string general = Parse_Page_General(url);
            string waitlist = Parse_Page_Waitlist(url);
            int gen = Convert.ToInt32(general);
            int wait = Convert.ToInt32(waitlist);

            string genslots = "999"; // error codes
            string waitslots = "996";
            int genspaces = 994;
            int waitspaces = 993;
            int counter = 0; // bells counter

            do
            {
                genslots = "998";
                waitslots = "995";

                genslots = Parse_Page_General(url);
                // ADD IF ELSE FUNCTION THAT HAS A LOOP INSIDE OF IT THAT AUTO TRIES TO RECONNECT IF IT LOSES CONNECTION AND LOOP TILL IT CONNECTS ---------------------------------
                int genseats = Convert.ToInt32(genslots); // conversion to int
                waitslots = Parse_Page_Waitlist(url);
                // ADD IF ELSE FUNCTION THAT HAS A LOOP INSIDE OF IT THAT AUTO TRIES TO RECONNECT IF IT LOSES CONNECTION AND LOOP TILL IT CONNECTS --------------------------------------
                int waitseats = Convert.ToInt32(waitslots); // conversion to int

                if (genseats != gen) // 68 != 67
                {
                    // email/txtmsg function
                    Send_Email(genslots, subject, crn); // add method parameter asking what email they want me to email them their notification. add this feature to final version when published online
                    Console.WriteLine("EMAIL SENT!");
                    genspaces = genseats;
                    counter++;
                    gen = genseats; // 68 = 68  // resets new norm to prevent email spam
                }
                else
                {
                    genspaces = gen; // 68 = 68
                }

                if (waitseats != wait)
                {
                    // email/txtmsg function
                    Send_Email(waitslots, subject, crn); // add method parameter asking what email they want me to email them their notification. add this feature to when going public
                    Console.WriteLine("EMAIL SENT!");
                    waitspaces = waitseats;
                    counter++;
                    wait = waitseats; // resets new norm to prevent email spam
                }
                else
                {
                    waitspaces = wait;
                }

                Console.WriteLine(genspaces + " seats still available in " + subject + " " + crn); // show  68
                Console.WriteLine(waitspaces + " waitlist seats still available in " + subject + " " + crn);
                System.Threading.Thread.Sleep(ms);

            } while (counter <= bells);

            Console.WriteLine("END");

        } // end of program

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
        }

        static void Send_Email(string seats, string subject, int crn)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("spamemail111131@gmail.com");
            mailMessage.Subject = "The amount of seats available has changed in " + subject + " " + crn;
            mailMessage.Body = seats;
            mailMessage.To.Add(new MailAddress("sd.johnsmith@gmail.com"));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("spamemail111131@gmail.com", "abc123123");
            smtp.Credentials = NetworkCred;
            smtp.Timeout = 10000; // can this timeout be greater than sleep function and still work? try it out later. <---------------------------------------------------------------
            smtp.Send(mailMessage);
        }

        static string Parse_Page_General(string url)
        {
            string Url = url; // for schedule of classes, input variable url after search params executed
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            string number = doc.DocumentNode.SelectNodes("/html/body/table[3]/tr[3]/td[3]")[0].InnerText; // edit nodes for remaining slots

            return number;
        }

        static string Parse_Page_Waitlist(string url)
        {
            string Url = url; // for schedule of classes, input variable url after search params executed
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            string number = doc.DocumentNode.SelectNodes("/html/body/table[3]/tr[3]/td[6]")[0].InnerText; // edit nodes for remaining slots

            return number;
        }
    }
}

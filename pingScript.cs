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
            /*
                These parameters are valid, but need to be modified in order to run on the web and take input from
                a webform of sorts. For debugging purposes, many of the Console.IO lines should be changed to Debug
                via a preprocessor format.



                DESIGN CHOICE:  Should the current WriteLines be kept for offline personal computer use?
                                -Maybe they should be commented out online in the mean time
                                -Possibly scrap them in order to save on file size?
                                -Keep two separate version. In the meantime, lets comment them out.

             */

            /*
            *Use a JSON Query to set the variables.
            *Doing so will permit for minimal editting of the script for server use
            *Any comments or suggestions against this?
             */
            string subject, term, Crn; // HTTP GET Request parameters


            //Console.WriteLine("What is your subject name abbreviated? Enter in all CAPS. Ex: PHYSICS = PHYS, MATHEMATICS = MATH.");

            //THIS NEEDS TO BE ABLE TO READ FROM WEBPAGE QUERY
            subject = Console.ReadLine();

            /*
            Console.WriteLine("What is the term for the course? Concatenate the year + the Season Query Code.");
            Console.WriteLine("Season Query Codes:");
            Console.WriteLine("Spring = 03");
            Console.WriteLine("Summer = 05");
            Console.WriteLine("Fall = 07");
            Console.WriteLine("Ex: 2017 Summer = 2017 + 05 = 201705");
            */

            //THIS NEEDS TO BE ABLE TO READ FROM WEBPAGE QUERY
            term = Console.ReadLine();


            //Console.WriteLine("What is the class's CRN?. Ex: 50408");

            //THIS NEEDS TO BE ABLE TO READ FROM WEBPAGE QUERY
            Crn = Console.ReadLine();
            int crn = Convert.ToInt32(Crn);

            string url = "https://ssb.vcccd.edu/prod/pw_pub_sched.p_course_popup?vsub=" + subject + "&vterm=" + term + "&vcrn=" + crn; // concactanate website
            //Console.WriteLine(url);

            //Console.WriteLine("Check remaining number of seats every __ seconds! Ex: 10 'for every 10 seconds");

            //THIS NEEDS TO BE ABLE TO READ FROM WEBPAGE QUERY
            string s = Console.ReadLine();
            int seconds = Convert.ToInt32(s);
            int ms = seconds * 1000;

            //Console.WriteLine("Allow __ notifications max (prevents email spam) Ex: 10");
            //THIS NEEDS TO BE ABLE TO READ FROM WEBPAGE QUERY
            string notifications = Console.ReadLine();

            //Line commented out because this is a deprecated error, used for debugging for offline use
            //int bells = Convert.ToInt32(notifications);


            int bells = 1;  //Max one notification per instance, per person, per crn





            //Pings VCCCD Website for amount of seats available
            //waitlisted seats, and then converts them to integers.
            //NOTE: Can be done in two lines of code.
            string general = Parse_Page_General(url);
            string waitlist = Parse_Page_Waitlist(url);
            int gen = Convert.ToInt32(general);
            int wait = Convert.ToInt32(waitlist);

            string genslots = "999";    // error codes
            string waitslots = "996";   //Used during debugging as these numbers are impossible to be queried

            int genspaces = 994;    //Total amount of seats left
            int waitspaces = 993;   //Total amount of waitlisted seats available
            int counter = 0; // bells counter !!!

            /*
                @Pre:
                @Loop:
                @Post:
             */
            do
            {
                /*These variables are reinitialized to the following numbers to determine if a ping was successful or not.
                *Should we scrap this method of and have a function for checking the ping?
                *This would eliminate the allocated resources for the two types, yes?
                *In turn, the max resources dedicated would be an error handler.
                *Min resources would then be a boolean comparison to check for connectivity every run through.
                */
                genslots = "998";
                waitslots = "995";


                genslots = Parse_Page_General(url);
                // ADD IF ELSE FUNCTION THAT HAS A LOOP INSIDE OF IT THAT AUTO TRIES TO RECONNECT IF IT LOSES CONNECTION AND LOOP TILL IT CONNECTS ---------------------------------
                int genseats = Convert.ToInt32(genslots); // conversion to int
                waitslots = Parse_Page_Waitlist(url);
                // ADD IF ELSE FUNCTION THAT HAS A LOOP INSIDE OF IT THAT AUTO TRIES TO RECONNECT IF IT LOSES CONNECTION AND LOOP TILL IT CONNECTS --------------------------------------
                int waitseats = Convert.ToInt32(waitslots); // conversion to int



                /*
                *The if-statement blocks were condensed into one to combine genseats and waitseats into one function call
                *This saves the amount of emails/text sent per a pass-through and the amount of comparisons
                *Saving computing resources is important
                 */
                if (genseats != gen || waitseats != wait) // 68 != 67
                {
                    // email/txtmsg function
                    Send_Email(genslots, waitslots, subject, crn); // add method parameter asking what email they want me to email them their notification. add this feature to final version when published online
                    Console.WriteLine("EMAIL SENT!");
                    genspaces = genseats;
                    counter++;
                    gen = genseats; // 68 = 68  // resets new norm to prevent email spam

                    waitspaces = waitseats;
                    wait = waitseats; // resets new norm to prevent email spam

                }
                else
                {
                    genspaces = gen; // 68 = 68
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


        /*
            @Pre:   Takes in @param arugments (string, string, string, int)
                    corresponding to seats available, waitlist available, course subject and course number
            @Func:  Processes an email and sends it to the user based off of passed parameters.
            @Post:  Returns nothing

         */
        static void Send_Email(string genseats, string waitseats, string subject, int crn)
        {

            //Creates an uninitialized MailMessage form object to send out to the user.
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("spamemail111131@gmail.com");    //The from email parameter
            mailMessage.Subject = "The amount of seats available has changed in " + subject + " " + crn;    //Declares the subject of the email

            mailMessage.Body = "There are " + genseats + " available. The waitlist has " + waitseats + " available";  //subject of the email.


            //May want to add a parameter to make the email a changeable entry
            mailMessage.To.Add(new MailAddress("sd.johnsmith@gmail.com")); //email to user
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("spamemail111131@gmail.com", "abc123123");
            smtp.Credentials = NetworkCred;
            smtp.Timeout = 10000; // can this timeout be greater than sleep function and still work? try it out later. <---------------------------------------------------------------
            smtp.Send(mailMessage);
        }


        /*
            @Pre:   Passed @param (string) containing the url fo specified webpage to be parsed
            @Func:  Parses specified webpage to get the amount of available seats for the selected course
            @Post:  Returns a string with a integer value of the amount seats available in the crn passed through
         */
        static string Parse_Page_General(string url)
        {
            string Url = url; // for schedule of classes, input variable url after search params executed
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            string number = doc.DocumentNode.SelectNodes("/html/body/table[3]/tr[3]/td[3]")[0].InnerText; // edit nodes for remaining slots

            return number;
        }

        /*
            @Pre:   Passed @param (string) containing the url fo specified webpage to be parsed
            @Func:  Parses specified webpage to get the amount of waitlist spots for the selected course
            @Post:  Returns a string with a integer value of the waitlisted spot available in the crn passed through
        */
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

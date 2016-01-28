using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackSize
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            string[] classArray = { "Intro to Arguing on the Internet: Godwin’s Law", "Understanding Circular Logic: Intro to Arguing on the Internet", "Godwin’s Law: Understanding Circular Logic", "Introduction to Paper Airplanes: ", "Advanced Throwing Techniques: Introduction to Paper Airplanes", "History of Cubicle Siege Engines: Rubber Band Catapults 101", "Advanced Office Warfare: History of Cubicle Siege Engines", "Rubber Band Catapults 101: ", "Paper Jet Engines: Introduction to Paper Airplanes" };
            Console.WriteLine(p.GetCourseOrder(classArray));
            Console.Read();
        }

        public string GetCourseOrder(string[] classArray)
        {
            string result = String.Empty;
            List<string> errors = new List<string>();
            //use a string list to store the courses
            List<string> courseList = new List<string>();

            try
            {
                //feed into a jagged array
                string[][] prereqsArray = new string[classArray.Count()][];
                for (int i = 0; i <= classArray.Count() - 1; i++)
                {
                    List<string> subList = classArray[i].Split(new[] { ": " }, StringSplitOptions.None).ToList();
                    string[] subArray = subList.FindAll(x => x.Trim() != String.Empty).ToArray();
                    prereqsArray[i] = subArray;
                }

                //loop through the courses
                foreach (string[] course in prereqsArray)
                {
                    for (int i = course.Count() - 1; i >= 0; i--)
                    {
                        string thisCourse = course[i].Trim();

                        //course isn't in the list yet, go ahead and add it
                        if (!courseList.Contains(thisCourse))
                        {
                            courseList.Add(thisCourse);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                errors.Add(exc.InnerException.ToString());
            }

            result = String.Join(", ", courseList);
            if (errors.Count > 0)
            {
                result = String.Concat(result, ". ERROR(s): ", String.Join(", ", errors));
            }
            return result;
        }
    }
}

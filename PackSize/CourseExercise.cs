using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackSize
{
    public class CourseExercise
    {
        static void Main(string[] args)
        {
            string[] classArray = { "Intro to Arguing on the Internet: Godwin’s Law", "Understanding Circular Logic: Intro to Arguing on the Internet", "Godwin’s Law: Understanding Circular Logic", "Introduction to Paper Airplanes: ", "Advanced Throwing Techniques: Introduction to Paper Airplanes", "History of Cubicle Siege Engines: Rubber Band Catapults 101", "Advanced Office Warfare: History of Cubicle Siege Engines", "Rubber Band Catapults 101: ", "Paper Jet Engines: Introduction to Paper Airplanes" };
            Console.WriteLine(GetCourseOrder(classArray));
            Console.Read();
        }

        public static string GetCourseOrder(string[] classArray)
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

                //first list courses with no prereqs
                string[][] noPrereqs = prereqsArray.Where(x => x.Count() == 1).ToArray();
                foreach (string[] course in noPrereqs)
                {
                    //add if it's not in the list already
                    if (!courseList.Contains(course[0]))
                    {
                        courseList.Add(course[0]);
                    }
                }

                //now handle the courses which have prereqs
                string[][] hasPrereqs = prereqsArray.Where(x => x.Count() > 1).ToArray();
                //loop through the courses
                foreach (string[] course in hasPrereqs)
                {
                    //start at the prereq and work backwards
                    for (int i = course.Count() - 1; i >= 0; i--)
                    {
                        string thisCourse = course[i].Trim();
                        string parentCourse = (i != 0) ? course[i - 1].Trim() : String.Empty;

                        if (parentCourse != String.Empty && courseList.Contains(thisCourse) && courseList.Contains(parentCourse))
                        {
                            //both classes already exist - possibility of circular dependencies
                            if (courseList.IndexOf(parentCourse) < courseList.IndexOf(thisCourse))
                            {
                                //parent course is earlier in the list than this course => circular reference                                
                                //are we failing the whole thing if a circular reference exists? If so, throw exception and handle
                                //otherwise, comment out "throw" statement, continue processing valid input, and return a note at end that a circular reference existed
                                errors.Add(String.Format("Circular reference exists: {0}: {1}", parentCourse, thisCourse));
                                //for now just throw a generic exception
                                throw new Exception("Circular Reference exists");
                            }
                        }
                        //course isn't in the list yet, go ahead and add it
                        if (!courseList.Contains(thisCourse))
                        {
                            
                            if (parentCourse != String.Empty && courseList.IndexOf(parentCourse) > -1)
                            {
                                //if it has a parent course, insert it before the parent course
                                courseList.Insert(courseList.IndexOf(parentCourse), thisCourse);
                            }
                            else
                            {
                                //otherwise just add it
                                courseList.Add(thisCourse);
                            }
                        }
                    }
                }

                result = String.Join(", ", courseList);
                if (errors.Count > 0)
                {
                    result = String.Concat(result, ". ERROR(s): ", String.Join(", ", errors));
                }
                return result;
            }
            catch (Exception exc)
            {
                //for now assuming we're here because of circular reference
                return exc.Message.ToString();
            }            
        }
    }
}

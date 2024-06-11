using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver_s_Course_Terminal
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string LicenseCategory { get; set; }  // Категории прав (A, B, C, D, E)
        public List<Attendance> Attendances { get; set; }
        public List<DrivingPractice> DrivingPractices { get; set; }
        public List<DrivingErrors> DrivingErrors { get; set; }
        public List<TestResults> TestResults { get; set; }
        public List<ExamResults> ExamResults { get; set; }

        public Student(int studentId, string name, string category)
        {
            StudentId = studentId;
            Name = name;
            LicenseCategory = category;
            Attendances = new List<Attendance>();
            DrivingPractices = new List<DrivingPractice>();
            DrivingErrors = new List<DrivingErrors>();
            TestResults = new List<TestResults>();
            ExamResults = new List<ExamResults>();
        }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public int DurationInDays { get; set; }
        public List<Student> Students { get; set; }

        public Course(int courseId, string name, double cost, int durationInDays)
        {
            CourseId = courseId;
            Name = name;
            Cost = cost;
            DurationInDays = durationInDays;
            Students = new List<Student>();
        }

        // Виртуальные методы, которые могут быть переопределены в наследниках
        public virtual double GetPopularity()
        {
            return Students.Count;
        }

        public virtual double GetQuality()
        {
            double passedCount = Students
                .SelectMany(s => s.ExamResults)
                .Count(er => er.IsPassed);

            return Students.Count > 0 ? passedCount / Students.Count : 0;
        }

        public virtual double GetCostEffectiveness()
        {
            return DurationInDays > 0 ? Cost / DurationInDays : 0;
        }
    }

    public class CourseHasStudents
    {
        public int CourseHasStudentsId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public CourseHasStudents(int courseHasStudentsId, int studentId, int courseId)
        {
            CourseHasStudentsId = courseHasStudentsId;
            StudentId = studentId;
            CourseId = courseId;
        }
    }

    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        public Attendance(int attendanceId, int studentId, int courseId, DateTime date, bool isPresent)
        {
            AttendanceId = attendanceId;
            StudentId = studentId;
            CourseId = courseId;
            Date = date;
            IsPresent = isPresent;
        }
    }

    public class DrivingPractice
    {
        public int PracticeId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string Instructor { get; set; }
        public double DurationInHours { get; set; }

        public DrivingPractice(int practiceId, int studentId, DateTime date, string instructor, double durationInHours)
        {
            PracticeId = practiceId;
            StudentId = studentId;
            Date = date;
            Instructor = instructor;
            DurationInHours = durationInHours;
        }
    }

    public class DrivingErrors
    {
        public int ErrorId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string ErrorDescription { get; set; }

        public DrivingErrors(int errorId, int studentId, DateTime date, string errorDescription)
        {
            ErrorId = errorId;
            StudentId = studentId;
            Date = date;
            ErrorDescription = errorDescription;
        }
    }

    public class TestResults
    {
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public double Score { get; set; }

        public TestResults(int testId, int studentId, DateTime date, double score)
        {
            TestId = testId;
            StudentId = studentId;
            Date = date;
            Score = score;
        }
    }

    public class ExamResults
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPassed { get; set; }

        public ExamResults(int examId, int studentId, DateTime date, bool isPassed)
        {
            ExamId = examId;
            StudentId = studentId;
            Date = date;
            IsPassed = isPassed;
        }
    }

    public class CourseAnalysis
    {
        public static List<Course> Courses { get; set; }

        public CourseAnalysis()
        {
            Courses = new List<Course>();
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }

        public void AddCoursehasStudent(int courseId, Student student)
        {
            foreach (Course course in Courses)
            {
                if (course.CourseId == courseId)
                {
                    course.Students.Add(student);
                    break;
                }
            }
        }

        public void AddAttendance(int courseId, int studentId, Attendance attendance)
        {
            foreach (Course course in Courses)
            {
                if (course.CourseId == courseId)
                {
                    foreach (Student student in course.Students)
                    {
                        if (student.StudentId == studentId)
                        {
                            student.Attendances.Add(attendance);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void AddDrivingPractice(int studentId, DrivingPractice practice)
        {
            bool added = false;
            foreach (Course course in Courses)
            {
                if (added)
                    break;
                foreach (Student student in course.Students)
                {
                    if (student.StudentId == studentId)
                    {
                        student.DrivingPractices.Add(practice);
                        added = true;
                        break;
                    }
                }
            }
        }

        public void AddDrivingErrors(int studentId, DrivingErrors errors)
        {
            bool added = false;
            foreach (Course course in Courses)
            {
                if (added)
                    break;
                foreach (Student student in course.Students)
                {
                    if (student.StudentId == studentId)
                    {
                        student.DrivingErrors.Add(errors);
                        added = true;
                        break;
                    }
                }
            }
        }

        public void AddTestResults(int studentId, TestResults test)
        {
            bool added = false;
            foreach (Course course in Courses)
            {
                if (added)
                    break;
                foreach (Student student in course.Students)
                {
                    if (student.StudentId == studentId)
                    {
                        student.TestResults.Add(test);
                        added = true;
                        break;
                    }
                }
            }
        }

        public void AddExamResults(int studentId, ExamResults exam)
        {
            bool added = false;
            foreach (Course course in Courses)
            {
                if (added)
                    break;
                foreach (Student student in course.Students)
                {
                    if (student.StudentId == studentId)
                    {
                        student.ExamResults.Add(exam);
                        added = true;
                        break;
                    }
                }
            }
        }

        public double GetCoursePopularity(int courseId)
        {
            Course course = Courses.FirstOrDefault(c => c.CourseId == courseId);
            return course != null ? course.GetPopularity() : 0;
        }

        public double GetCourseQuality(int courseId)
        {
            Course course = Courses.FirstOrDefault(c => c.CourseId == courseId);
            return course != null ? course.GetQuality() : 0;
        }

        public double GetCourseCostEffectiveness(int courseId)
        {
            Course course = Courses.FirstOrDefault(c => c.CourseId == courseId);
            return course != null ? course.GetCostEffectiveness() : 0;
        }
    }
}

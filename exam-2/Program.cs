
        
           
public class Answer
        {
            public int AnswerId { get; set; }
            public string AnswerText { get; set; }
            public override string ToString() => $"{AnswerId}: {AnswerText}";
        }

        public abstract class Question : ICloneable, IComparable<Question>
        {
            public string Header { get; set; }
            public string Body { get; set; }
            public int Mark { get; set; }
            public List<Answer> Answers { get; set; } = new List<Answer>();
            public Answer CorrectAnswer { get; set; }

            public object Clone()
            {
                var clone = (Question)this.MemberwiseClone(); 
                clone.Answers = new List<Answer>(this.Answers);
                return clone;
            }

            public int CompareTo(Question other)
                => other == null ? 1 : this.Mark.CompareTo(other.Mark); 

            public override string ToString()
                => $"[{GetType().Name}] {Header} - {Mark} Marks";
        }

       
        public class TrueFalseQuestion : Question { }
        public class MCQQuestion : Question { }

        public abstract class Exam
        {
            public TimeSpan Time { get; set; }
            public int NumberOfQuestions
                => Questions.Count;
            public List<Question> Questions { get; set; } = new List<Question>();

            public abstract void ShowExam();
        }

        public class FinalExam : Exam
        {
            public override void ShowExam()
            {
                Console.WriteLine("=== Final Exam ===");
                foreach (var q in Questions)
                {
                    Console.WriteLine(q);
                    q.Answers.ForEach(a => Console.WriteLine($"  - {a}"));
                }
                Console.WriteLine($"Total Grade: {Questions.Sum(q => q.Mark)}");
            }
        }

      
        public class PracticalExam : Exam
        {
            public override void ShowExam()
            {
                Console.WriteLine("=== Practical Exam ===");
                foreach (var q in Questions)
                {
                    Console.WriteLine(q);
                    Console.WriteLine("Correct Answer: " + q.CorrectAnswer);
                }
            }
        }

        public class Subject
        {
            public int SubjectId { get; set; }
            public string SubjectName { get; set; }
            public Exam Exam { get; private set; }

            public Subject(int id, string name)
            {
                SubjectId = id;
                SubjectName = name;
            }

            public void CreateExam(bool isFinal)
                => Exam = isFinal ? new FinalExam() : new PracticalExam();
        }


class Program
{

    static void Main()
    {
        var subj = new Subject(1, "OOP C#");
        subj.CreateExam(isFinal: true);

        subj.Exam.Time = TimeSpan.FromMinutes(60);
        subj.Exam.Questions.Add(new MCQQuestion
        {
            Header = "What is encapsulation?",
            Body = "Explain encapsulation in OOP",
            Mark = 5,
            Answers = {
                new Answer { AnswerId = 1, AnswerText = "Hiding internal details" },
                new Answer { AnswerId = 2, AnswerText = "Dividing code into methods" }
            },
            CorrectAnswer = new Answer { AnswerId = 1, AnswerText = "Hiding internal details" }
        });

        subj.Exam.ShowExam();
        Console.ReadKey();
    }
}

    



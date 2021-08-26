using System;
using System.Collections.Generic;
using System.Text;

namespace HocTuVung.Learning
{
    using Models;
    using System.Windows;

    public enum LearningType
    {
        EngToVie,
        VieToEng,
    }

    public class SchedulingAlgorithm
    {
        public Queue<Vocab> VocabQueue { get; set; } = new Queue<Vocab>();
        public LearningType LearningType { get; set; } = new LearningType();

        private int TotalAnswer = 0;
        private int AcceptAnswer = 0;

        public SchedulingAlgorithm()
        {
            LearningType = LearningType.VieToEng;
        }

        public void Reset()
        {
            VocabQueue.Clear();
            AcceptAnswer = TotalAnswer = 0;
        }

        public void BuildQueue(IList<Vocab> vocabs, int repeat)
        {
            //Shuffle
            IList<Vocab> temp = new List<Vocab>();
            foreach (var item in vocabs)
            {
                temp.Add(item.Clone());
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int step = 0; step < (1 << 4); ++step)
            {
                for (int i = 1; i < temp.Count; ++i)
                {
                    int j = random.Next() % i;
                    Vocab tmp = temp[i];
                    temp[i] = temp[j];
                    temp[j] = tmp;
                }
            }

            //Build
            for (int i = 0; i < repeat; ++i)
            {
                for (int j = 0; j < temp.Count; ++j)
                {
                    this.VocabQueue.Enqueue(temp[j]);
                }
            }

            AcceptAnswer = 0;
            TotalAnswer = repeat * temp.Count;
        }

        public bool HasMoreVocab()
        {
            return this.VocabQueue.Count != 0;
        }

        public bool CheckAnswer(string answer)
        {
            if (this.VocabQueue.Count == 0)
                return true;
            switch (LearningType)
            {
                case LearningType.VieToEng:
                    {
                        //Check eng
                        string tmp_answer = answer.Trim().ToLower();
                        string jud_answer = VocabQueue.Peek().English.ToLower();
                        if (tmp_answer != jud_answer)
                            return false;
                        AcceptAnswer++;
                        return true;
                    }
                    break;
                case LearningType.EngToVie:
                    {
                        string tmp_answer = answer.Trim().ToLower();
                        string jud_answer = VocabQueue.Peek().Vietnam.ToLower();
                        if (tmp_answer != jud_answer)
                            return false;
                        AcceptAnswer++;
                        return true;
                    }
                    break;
            }
            return true;
        }

        public string CurrentQuestion()
        {
            if (this.HasMoreVocab())
            {
                switch (this.LearningType)
                {
                    case LearningType.EngToVie:
                        return this.VocabQueue.Peek().English;
                    case LearningType.VieToEng:
                        return this.VocabQueue.Peek().Vietnam;
                }
            }
            return "";
        }

        public string GetPercent()
        {
            return AcceptAnswer.ToString() + "/" + TotalAnswer.ToString();
        }

        public void Next()
        {
            if (this.HasMoreVocab())
            {
                this.VocabQueue.Dequeue();
            }
        }
    }
}
